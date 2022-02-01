using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp39
{
    class OverallMovieRatings
    {
        string filepath = @"D:\Psiog programs\Set 0 solns\ml-100k\u.data";
        int indexPerThread = 15000;

        List<string> MoviesID = new List<string>();

        List<int> RatingsSum = new List<int>();

        List<int> Count = new List<int>();

        List<double> Avg = new List<double>();

        List<double> CountPercentage = new List<double>();

        List<double> WeightAvg = new List<double>();
        public void Top10Ratings()
        {

            Console.Write("{0,-10}", "");
            Console.Write("{0,-40}", "Overall Top 10 Movies ");
            Console.WriteLine("\n");


            DataProcessor[] dataProcessors = new DataProcessor[7];
            Thread[] threads = new Thread[7];

            for (int i = 0; i < 7; i++)
            {
                dataProcessors[i] = new DataProcessor();
                int firstindex = i * 15000;
                int lastindex = firstindex + indexPerThread - 1;
                if (i == 100000 / indexPerThread & 100000 % indexPerThread > 0)
                {
                    lastindex = firstindex + 9999;
                }
                dataProcessors[i].ArrayProcessor(firstindex, lastindex);

                threads[i] = new Thread(dataProcessors[i].CountRatings);
                threads[i].Start();

            }

            for (int i = 0; i < 7; i++)
            {
                threads[i].Join();
            }




            //Thread[] threads = new Thread[7];
            int k = 1;

            while (k < 1683)
            {

                MoviesID.Add(k.ToString());
                int position = MoviesID.IndexOf(k.ToString());
                RatingsSum.Add(0);
                Count.Add(0);
                CountPercentage.Add(0);

                for (int i = 0; i < 7; i++)
                {
                    if (dataProcessors[i].ItemID.Contains(k.ToString()))
                    {

                        int pos = dataProcessors[i].ItemID.IndexOf(k.ToString());
                        RatingsSum[position] += dataProcessors[i].RatingsSum[pos];
                        Count[position] += dataProcessors[i].Count[pos];
                    }


                }

                k++;
            }


            for (int i = 0; i < MoviesID.Count(); i++)
            {
                
                double Tempavg = (double)(RatingsSum[i]) / (double)(Count[i]);
                double WeightTempavg = (Tempavg) * ((double)(Count[i]));
                Avg.Add(Tempavg);
                WeightAvg.Add(WeightTempavg);
               
            }

            for (int i = 0; i < WeightAvg.Count(); i++)
            {
                for (int j = 0; j < WeightAvg.Count() - 1; j++)
                    if (WeightAvg[j].CompareTo(WeightAvg[j + 1]) < 0)
                    {
                        double Weighttempavg = WeightAvg[j];
                        WeightAvg[j] = WeightAvg[j + 1];
                        WeightAvg[j + 1] = Weighttempavg;

                        string tempmovieid = MoviesID[j];
                        MoviesID[j] = MoviesID[j + 1];
                        MoviesID[j + 1] = tempmovieid;

                        double tempavg = Avg[j];
                        Avg[j] = Avg[j + 1];
                        Avg[j + 1] = tempavg;

                        int tempcount = Count[j];
                        Count[j] = Count[j + 1];
                        Count[j + 1] = tempcount;


                    }

            }

            Console.Write("{0,-20}", "MovieID");
            Console.Write("{0,-20}", "Avg Rating");
            Console.Write("{0,-20}", "No of times rated");
            Console.WriteLine("\n");

            for(int i=0; i<Count.Count();i++)
            {
                CountPercentage[i] = ((double)(Count[i]) / (double)(100000)) * (double)(100);
            }

            for (int i = 0; i < 10; i++)
            {
                Avg[i] = Math.Round(Avg[i], 2);
                Console.Write("{0,-20}", MoviesID[i]);
                Console.Write("{0,-20}", Avg[i]);
                Console.Write("{0,-20}", Count[i]);
                Console.WriteLine("\n");
            }


        }

        public void AgeRatings()
        {
            AgeProcessor[] ageProcessors = new AgeProcessor[7];
            Thread[] threads = new Thread[7];

            for(int k=1;k<=61;k+=20)
            {

                Console.Write("{0,-10}", "");
                Console.Write("{0,-40}", "Top 5 Movies for Age Category: "+k+"-"+Convert.ToString(((int)k+(int)19))+":");
                Console.WriteLine("\n");

                Avg = new List<double>();
                WeightAvg = new List<double>();
                AgeProcessor.MoviesID = new List<string>();
                AgeProcessor.RatingsSum = new List<int>();
                AgeProcessor.Count = new List<int>();

                for (int i = 0; i < 7; i++)
                {
                    ageProcessors[i] = new AgeProcessor();
                    ageProcessors[i].MinAge = k;
                    ageProcessors[i].MaxAge = k+19;
                    int firstindex = i * 15000;
                    int lastindex = firstindex + indexPerThread - 1;
                    if (i == 100000 / indexPerThread & 100000 % indexPerThread > 0)
                    {
                        lastindex = firstindex + 9999;
                    }
                    ageProcessors[i].ArrayProcessor(firstindex, lastindex);

                    threads[i] = new Thread(ageProcessors[i].CountRatings);
                    threads[i].Start();

                }

                for (int i = 0; i < 7; i++)
                {
                    threads[i].Join();
                }

                MoviesID = AgeProcessor.MoviesID;
                RatingsSum = AgeProcessor.RatingsSum;
                Count = AgeProcessor.Count;

                for (int i = 0; i < MoviesID.Count(); i++)
                {                   
                    double Tempavg = (double)(RatingsSum[i]) / (double)(Count[i]);
                    double WeightTempavg = (Tempavg) * ((double)(Count[i]));
                    Avg.Add(Tempavg);
                    WeightAvg.Add(WeightTempavg);
                    CountPercentage.Add(0);
                }

                for (int i = 0; i < WeightAvg.Count(); i++)
                {
                    for (int j = 0; j < WeightAvg.Count() - 1; j++)
                        if (WeightAvg[j].CompareTo(WeightAvg[j + 1]) < 0)
                        {
                            double Weighttempavg = WeightAvg[j];
                            WeightAvg[j] = WeightAvg[j + 1];
                            WeightAvg[j + 1] = Weighttempavg;

                            string tempmovieid = MoviesID[j];
                            MoviesID[j] = MoviesID[j + 1];
                            MoviesID[j + 1] = tempmovieid;

                            double tempavg = Avg[j];
                            Avg[j] = Avg[j + 1];
                            Avg[j + 1] = tempavg;

                            int tempcount = Count[j];
                            Count[j] = Count[j + 1];
                            Count[j + 1] = tempcount;


                        }

                }

                Console.Write("{0,-20}", "MovieID");
                Console.Write("{0,-20}", "Avg Rating");
                Console.Write("{0,-20}", "No of times Rated");
                Console.WriteLine("\n");

                for (int i = 0; i < Count.Count(); i++)
                {
                    CountPercentage[i] = ((double)(Count[i]) / (double)(100000)) * (double)(100);
                }

                for (int i = 0; i < 5; i++)
                {
                    Avg[i] = Math.Round(Avg[i], 2);
                    Console.Write("{0,-20}", MoviesID[i]);
                    Console.Write("{0,-20}", Avg[i]);
                    Console.Write("{0,-20}", Count[i]);
                    Console.WriteLine("\n");
                }

            }

        }

        public void GenderRatings()
        {
            GenderProcessor[] genderProcessors = new GenderProcessor[7];
            Thread[] threads = new Thread[7];

            List<string> GenderList = new List<string>() { "M", "F" };
            foreach(var gender in GenderList)
            {
                Console.Write("{0,-10}", "");
                Console.Write("{0,-40}", "Top 5 Movies for Gender Category: " + gender );
                Console.WriteLine("\n");

                Avg = new List<double>();
                WeightAvg = new List<double>();
                GenderProcessor.MoviesID = new List<string>();
                GenderProcessor.RatingsSum = new List<int>();
                GenderProcessor.Count = new List<int>();

                for (int i = 0; i < 7; i++)
                {
                    genderProcessors[i] = new GenderProcessor();
                    genderProcessors[i].Gender = gender;

                    int firstindex = i * 15000;
                    int lastindex = firstindex + indexPerThread - 1;
                    if (i == 100000 / indexPerThread & 100000 % indexPerThread > 0)
                    {
                        lastindex = firstindex + 9999;
                    }
                    genderProcessors[i].ArrayProcessor(firstindex, lastindex);

                    threads[i] = new Thread(genderProcessors[i].CountRatings);
                    threads[i].Start();

                }

                for (int i = 0; i < 7; i++)
                {
                    threads[i].Join();
                }

                
                MoviesID = GenderProcessor.MoviesID;
                RatingsSum = GenderProcessor.RatingsSum;
                Count = GenderProcessor.Count;

                for (int i = 0; i < MoviesID.Count(); i++)
                {
                    double Tempavg = (double)(RatingsSum[i]) / (double)(Count[i]);
                    double WeightTempavg = (Tempavg) * ((double)(Count[i]));
                    Avg.Add(Tempavg);
                    WeightAvg.Add(WeightTempavg);
                    CountPercentage.Add(0);
                }

                for (int i = 0; i < WeightAvg.Count(); i++)
                {
                    for (int j = 0; j < WeightAvg.Count() - 1; j++)
                        if (WeightAvg[j].CompareTo(WeightAvg[j + 1]) < 0)
                        {
                            double Weighttempavg = WeightAvg[j];
                            WeightAvg[j] = WeightAvg[j + 1];
                            WeightAvg[j + 1] = Weighttempavg;

                            string tempmovieid = MoviesID[j];
                            MoviesID[j] = MoviesID[j + 1];
                            MoviesID[j + 1] = tempmovieid;

                            double tempavg = Avg[j];
                            Avg[j] = Avg[j + 1];
                            Avg[j + 1] = tempavg;

                            int tempcount = Count[j];
                            Count[j] = Count[j + 1];
                            Count[j + 1] = tempcount;


                        }
                }

                for (int i = 0; i < Count.Count(); i++)
                {
                    CountPercentage[i] = ((double)(Count[i]) / (double)(100000)) * (double)(100);
                }

                Console.Write("{0,-20}", "MovieID");
                Console.Write("{0,-20}", "Avg Rating");
                Console.Write("{0,-20}", "No of times Rated ");
                Console.WriteLine("\n");

                for (int i = 0; i < 5; i++)
                {
                    Avg[i] = Math.Round(Avg[i], 2);
                    Console.Write("{0,-20}", MoviesID[i]);
                    Console.Write("{0,-20}", Avg[i]);
                    Console.Write("{0,-20}", Count[i]);
                    Console.WriteLine("\n");
                }
            }
        }

    }
}
    

