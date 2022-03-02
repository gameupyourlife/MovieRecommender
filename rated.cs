using Microsoft.ML;
using Microsoft.ML.Trainers;
using System;
using System.IO;
using System.Text;
using CsvHelper;
using Microsoft.VisualBasic.FileIO;
using MovieRecommender;


namespace MovieRecommender
{
    public class rated
    {
        public static void getRatedMovies (user user, MLContext mlContext, ITransformer model)
        {
            var results = new List<int>();
            var ratings = new List<int>();


            using (TextFieldParser csvParser = new TextFieldParser(@"C:\Users\Cedric\source\repos\MovieRecommender\Data\user-movie-rating.csv"))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { $"," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();
                string temp;

                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    //Result = UserId
                    temp = fields[0];
                    if (Convert.ToInt32(temp) == user.userID)
                    {
                        //Result = MovieId
                        results.Add(Convert.ToInt32(fields[1]));
                        ratings.Add(Convert.ToInt32(fields[2]));
                        
                    }
                }

                csvParser.Close();
            }

            using (TextFieldParser csvParser = new TextFieldParser(@"C:\Users\Cedric\source\repos\MovieRecommender\Data\movie-list.csv"))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { $";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();
                string temp;
                for (int i = 0; i < results.Count; i++)
                {
                    int target = results[i];

                    while (!csvParser.EndOfData)
                    {
                        string[] fields = csvParser.ReadFields();
                        //Result = UserId
                        temp = fields[0];
                        if (Convert.ToInt32(temp) == target)
                        {
                            Console.WriteLine($"{fields[1]} with a rating of {ratings[i]}");
                            break;

                        }
                    }
                }

                csvParser.Close();
            }

            Interface.mainInterface(user, mlContext, model);
        }
    }
}
