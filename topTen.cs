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
    public class topTen
    {
        //Spits out the top ten recommended films for that user
        public static void outTopTen(user user, MLContext mlContext, ITransformer model)
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            var predictionEngine = mlContext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(model);
            var recomendedMovies = new List<float>();
            var recomendedRating = new List<float>();
            var userId = user.userID;

            for (int i = 0; i < 23856; i++)
            {
                var testInput = new MovieRating { userId = userId, movieId = i };

                var movieRatingPrediction = predictionEngine.Predict(testInput);

                if (Math.Round(movieRatingPrediction.Score, 1) > 3.5)
                {
                    recomendedMovies.Add(testInput.movieId);
                    recomendedRating.Add(movieRatingPrediction.Score);
                }
            }

            for (int i = 0; i < recomendedMovies.Count; i++)
            {
                for (int x = 0; x < recomendedRating.Count - 1; x++)
                {
                    if (recomendedRating[index: x] <= recomendedRating[index: x + 1])
                    {
                        float zs = recomendedRating[index: x];
                        recomendedRating[index: x] = recomendedRating[index: x + 1];
                        recomendedRating[index: x + 1] = zs;

                        zs = recomendedMovies[index: x];
                        recomendedMovies[index: x] = recomendedMovies[index: x + 1];
                        recomendedMovies[index: x + 1] = zs;
                    }
                }
            }

            if (recomendedMovies.Count > 0)
            {
                using (TextFieldParser csvParser = new TextFieldParser(@"C:\Users\Cedric\source\repos\MovieRecommender\Data\movie-list.csv"))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { $";" });
                    csvParser.HasFieldsEnclosedInQuotes = true;

                    // Skip the row with the column names
                    csvParser.ReadLine();
                    string temp;
                    for (int i = 0; i < 10; i++)
                    {
                        float target = recomendedMovies[i];
                        Convert.ToInt32(target);

                        while (!csvParser.EndOfData)
                        {
                            string[] fields = csvParser.ReadFields();
                            // Temp = movieId
                            temp = fields[0];
                            if (Convert.ToInt32(temp) == target)
                            {
                                Console.WriteLine("Movie " + fields[1] + " is with a rating of " + recomendedRating[index: i] + " inside the Top 10 of " + user.userName);
                                break;

                            }
                        }
                    }

                    csvParser.Close();
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Interface.mainInterface(user, mlContext, model);
        }
    }
}
