using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace ConsoleApp39
{
    class GenderProcessor
    {
        public int FirstIndex { get; set; }
        public int LastIndex { get; set; }
        public string Gender { get; set; }

        public static List<string> MoviesID = new List<string>();

        public static List<int> RatingsSum = new List<int>();

        public static List<int> Count = new List<int>();

        public static List<double> Avg = new List<double>();

        static object baton = new object();

        public void ArrayProcessor(int firstIndex, int lastIndex)
        {
            FirstIndex = firstIndex;
            LastIndex = lastIndex;
        }

        public void CountRatings()
        {

            IEnumerable<DataRecord> datarecords;
            DataRecord[] datarecordsArray = new DataRecord[0];
            IEnumerable<UserRecord> userrecords;
            UserRecord[] userrecordsArray = new UserRecord[0];

            using (var sr = new StreamReader(@"D:\Psiog programs\ml-100k\u.data"))
            using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
            {
                csvreader.Configuration.HasHeaderRecord = false;
                csvreader.Configuration.Delimiter = "	";

                datarecords = csvreader.GetRecords<DataRecord>();
                datarecordsArray = datarecords.ToArray();
                //Console.WriteLine(recordsArray.Length);
            }

            using (var sr = new StreamReader(@"D:\Psiog programs\ml-100k\u.user"))
            using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
            {
                csvreader.Configuration.HasHeaderRecord = false;
                csvreader.Configuration.Delimiter = "|";

                userrecords = csvreader.GetRecords<UserRecord>();
                userrecordsArray = userrecords.ToArray();
                //Console.WriteLine(recordsArray.Length);
            }

            for (int i = FirstIndex; i <= LastIndex; i++)
            {

                //int userage = 0;

                foreach (var userrec in userrecordsArray)
                {
                    if (datarecordsArray[i].userid==userrec.userid & Gender==userrec.gender)
                    {
                        lock (baton)
                        {
                            if (!MoviesID.Contains(datarecordsArray[i].itemid))
                            {
                                MoviesID.Add(datarecordsArray[i].itemid);
                                RatingsSum.Add(datarecordsArray[i].rating);
                                Count.Add(1);
                            }

                            else
                            {
                                int pos = MoviesID.IndexOf(datarecordsArray[i].itemid);
                                RatingsSum[pos] += datarecordsArray[i].rating;
                                Count[pos]++;

                            }
                        }

                    }

                }


            }

        }

    }
}
