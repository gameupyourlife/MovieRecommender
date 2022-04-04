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
using Spectre.Console;


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
            AnsiConsole.Write(new Rule("[orange1]Recommend Movie[/]").LeftAligned());

            string movieName = AnsiConsole.Ask<string>("What's the [green]name[/] of the movie?");
            var movieId = getMovieId(movieName.ToUpper());
            if (movieId == "ERROR")
            {
                var inp = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Movie not found. What do you want to [green]do[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(new[] {
                        "Rate another movie or retry", "Do something else"
                    }));

                if (inp == "Rate another movie or retry")
                    inpRecommend(user, mlContext, model);
                else
                    Interface.mainInterface(user, mlContext, model);
            }


            
            var movieRating = AnsiConsole.Prompt(
                new TextPrompt<int>("How do you [green]rate[/] the movie?")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That's not a valid rating[/]")
                    .Validate(rating =>
                    {
                        return rating switch
                        {
                            < 0 => ValidationResult.Error("[red]Rating has to be at least 0[/]"),
                            > 5 => ValidationResult.Error("[red]Rating hast to be at max 5[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));

            var newLine = string.Format("{0},{1},{2},{3}", userId, movieId, movieRating, DateTime.Now);
            csv.AppendLine(newLine);
            

            File.AppendAllText(@"Data\user-movie-rating.csv", csv.ToString());

            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    // Define tasks
                    var task1 = ctx.AddTask("[green]Saving Entry[/]");
                    var task2 = ctx.AddTask("[green]Recalculating MLM[/]");

                    while (!ctx.IsFinished)
                    {
                        task1.Increment(3);
                        task2.Increment(1.5);
                        Thread.Sleep(50);
                    }
                });

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
                AnsiConsole.WriteLine("Pleas write a number");
                rating = Console.ReadLine();
                return tryRating(rating);
            }
        }

        public static string getMovieId (string movieName)
        {
            using (TextFieldParser csvParser = new TextFieldParser(@"Data\movie-list.csv"))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { $";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                
                string results = "";

                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    results = fields[1];
                    if (results.ToUpper() == movieName)
                    {
                        results = fields[0];
                        csvParser.Close();
                        return results;
                    }
                }


                csvParser.Close();
                results = "ERROR";
                return results;
            }
        }
    }
}
