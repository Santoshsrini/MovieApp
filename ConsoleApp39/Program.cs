using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Net.Http.Headers;
using System.Xml.Schema;

namespace ConsoleApp39
{
    class Program
    {
        static void Main(string[] args)
        {
            string filepath = @"D:\Psiog programs\ml-100k\u.genre";

            //int No_threads = 10000 / 15000 + 1;
            
            string home = "y";
            do
            {
                Console.Clear();

                Console.WriteLine("Welcome to MovieRatings App" + "\n");
                Console.WriteLine("1. Overall Top10 Ratings");
                Console.WriteLine("2. Top5 Ratings for Age Category");
                Console.WriteLine("3. Top5 Ratings for each Gender Category");


                Console.WriteLine("Which report do you want to see ?");
                int choice = int.Parse(Console.ReadLine());

                OverallMovieRatings overallMovieRatings = new OverallMovieRatings();



                switch (choice)
                {
                    case 1:
                        overallMovieRatings.Top10Ratings();
                        break;

                    case 2:
                        overallMovieRatings.AgeRatings();
                        break;

                    case 3:
                        overallMovieRatings.GenderRatings();
                        break;
                    

                    default:
                        Console.WriteLine("Enter value between 1 to 3");
                        break;

                }

                Console.WriteLine("Do you want to go back to home page (y/n)");
                home = Console.ReadLine();

            } while (home == "y");

            

            Console.ReadLine();
        }
    }
}
