using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.Threading;

namespace ConsoleApp39
{
    class DataProcessor
    {
        
        public int FirstIndex { get; set; }
        public int LastIndex { get; set; }

        public List<string> ItemID = new List<string>();

        public List<int> RatingsSum = new List<int>();

        public List<int> Count = new List<int>();

        IEnumerable<DataRecord> records;
        DataRecord[] recordsArray = new DataRecord[0];

        public void ArrayProcessor(int firstIndex, int lastIndex)
        {
            FirstIndex = firstIndex;
            LastIndex = lastIndex;
            
        }

        public void CountRatings()
        {
            using (var sr = new StreamReader(@"D:\Psiog programs\ml-100k\u.data"))
            using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
            {
                csvreader.Configuration.HasHeaderRecord = false;
                csvreader.Configuration.Delimiter = "	";

                records = csvreader.GetRecords<DataRecord>();
                recordsArray = records.ToArray();
                //Console.WriteLine(recordsArray.Length);
            }

            for(int i=FirstIndex; i<=LastIndex;i++)
            {
                if(!ItemID.Contains(recordsArray[i].itemid))
                {
                    ItemID.Add(recordsArray[i].itemid);
                    RatingsSum.Add(recordsArray[i].rating);
                    Count.Add(1);
                }

                else
                {
                    int pos = ItemID.IndexOf(recordsArray[i].itemid);
                    RatingsSum[pos] += recordsArray[i].rating;
                    Count[pos]++;

                }
            }

            
        }

    }
}
