using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using System;
using System.IO;
using CsvHelper;
using Microsoft.VisualBasic.FileIO;
using MovieRecommender;


namespace MovieRecommender
{
    public class recommend
    {
        //Allows the user to rate a new movie
        public static void inpRecommend(user user, MLContext mlContext, ITransformer model)
        {
            Random rnd = new Random();
            var csv = new StringBuilder();

            var userId = user.userID;
            Console.WriteLine("Movie Name:");
            string movieName = Console.ReadLine();
            var movieId = getMovieId(movieName);
            if (movieId == "ERROR")
            {
                Console.WriteLine("Movie is not in DB or was written false.");
                Console.WriteLine("Either try again by typing try or choose a other option.");
                string inp = Console.ReadLine();
                if (inp == "try" || inp == "t")
                    inpRecommend(user, mlContext, model);
                else
                    Interface.mainInterface(user, mlContext, model);
            }
            Console.WriteLine("Rating (1-5):");
            var rating = Console.ReadLine();
            while (rating == "")
            {
                Console.WriteLine("Try Again");
                rating = Console.ReadLine();
            }
            int movieRating = tryRating(rating);
            if (movieRating > 5)
                movieRating = 5;
            if (movieRating < 1)
                movieRating = 0;

            var newLine = string.Format("{0},{1},{2},{3}", userId, movieId, movieRating, DateTime.Now);
            csv.AppendLine(newLine);
            

            File.AppendAllText(@"C:\Users\Cedric\source\repos\MovieRecommender\Data\user-movie-rating.csv", csv.ToString());

            Console.WriteLine("Sucessfuly Rated");

            Interface.mainInterface(user, mlContext, model);
        }
  
        private static int tryRating(string rating)
        {
            try
            {
                int rate = Convert.ToInt32(rating);
                return rate;

            }
            catch (Exception)
            {
                Console.WriteLine("Error no number inputed. Retry rating");
                rating = Console.ReadLine();
                tryRating(rating);
            }
            return Convert.ToInt32(rating);
        }

        public static string getMovieId (string movieName)
        {
            using (TextFieldParser csvParser = new TextFieldParser(@"C:\Users\Cedric\source\repos\MovieRecommender\Data\movie-list.csv"))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { $";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                Console.WriteLine(csvParser.ReadLine());

                
                string results = "";

                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    //Console.WriteLine(fields);
                    results = fields[1];
                    if (results == movieName)
                    {
                        results = fields[0];
                        csvParser.Close();
                        //Console.WriteLine("Results: " + results);
                        return results;
                    }
                }


                csvParser.Close();
                results = "ERROR";
                Console.WriteLine("Results: " + results);
                return results;
            }
        }
    }
}
