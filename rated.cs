using Microsoft.ML;
using Microsoft.ML.Trainers;
using System;
using System.IO;
using System.Text;
using CsvHelper;
using Microsoft.VisualBasic.FileIO;
using MovieRecommender;
using Spectre.Console;




namespace MovieRecommender
{
    public class rated
    {
        //Lists all the movies a user has rated up until this point
        public static void getRatedMovies (user user, MLContext mlContext, ITransformer model)
        {
            var results = new List<int>();
            var ratings = new List<int>();
            var name = new List<string>();



            using (TextFieldParser csvParser = new TextFieldParser(@"Data\user-movie-rating.csv"))
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
                    if (Convert.ToInt32(temp) == Convert.ToInt32( user.userID))
                    {
                        //Result = MovieId

                        //results.Append(Convert.ToInt32(fields[1]));
                        //ratings.Append(Convert.ToInt32(fields[2]));

                        results.Add(Convert.ToInt32(fields[1]));
                        ratings.Add(Convert.ToInt32(fields[2]));


                    }
                }

                csvParser.Close();
            }


            
                
                int target;
                for (int i = 0; i < results.Count; i++)
                {
                    using (TextFieldParser csvParser = new TextFieldParser(@"Data\movie-list.csv"))
                    {
                        csvParser.CommentTokens = new string[] { "#" };
                        csvParser.SetDelimiters(new string[] { $";" });
                        csvParser.HasFieldsEnclosedInQuotes = false;

                        // Skip the row with the column names
                        csvParser.ReadLine();
                        target = results[i];
                        string temp;
                        while (!csvParser.EndOfData)
                        {
                            string[] fields = csvParser.ReadFields();
                            //Result = UserId
                            temp = fields[0];
                            if (Convert.ToInt32(temp) == target)
                                name.Add(fields[1]);


                            
                 
                        }
                        csvParser.Close();
                    }
                }

            AnsiConsole.Write(new Rule("[orange1]Rated Movies[/]").LeftAligned());


            // Create a table
            var table = new Table();

            AnsiConsole.Live(table)
                .Start(ctx =>
                {
                    // Add some columns
                    table.AddColumn(new TableColumn("Movie").Centered());
                    table.AddColumn(new TableColumn("Rating").Centered());
                    ctx.Refresh();
                    Thread.Sleep(1000);

                    // Add some rows
                    for (int i = 0; i < results.Count; i++)
                    {
                        table.AddRow($"{name[i]}", $"{ratings[i]}");
                        ctx.Refresh();
                        Thread.Sleep(100);
                    }
                });
            // Render the table to the console
            //AnsiConsole.Write(table);

            Interface.mainInterface(user, mlContext, model);
        }
    }
}
