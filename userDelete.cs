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
    public class userDeleteManagement
    {
        public static void userDelete(user user, MLContext mlContext, ITransformer model)
        {
            AnsiConsole.Write(new Rule("[orange1]Delete Account[/]").LeftAligned());
            if (confirm())
            {
                AnsiConsole.Write(
                    new FigletText("Deleted")
                        .LeftAligned()
                        .Color(Color.Red));
                deleteUserInUser(user);
                deleteUserInMovies(user);
                Thread.Sleep(2000);
                AnsiConsole.Clear();
                Interface.InterfaceInit(mlContext, model);

            }
            else
            {
                AnsiConsole.Write(
                    new FigletText("Aborted")
                        .LeftAligned()
                        .Color(Color.Green));
                Interface.mainInterface(user, mlContext, model);
            }

        }

        static bool confirm()
        {
            if (AnsiConsole.Confirm("Are you sure you want to [red]DELETE[/] your Account?"))
            {
                AnsiConsole.MarkupLine("Account will be deleted...");
                return true;
            }

            return false;
        }

        static void deleteUserInUser(user user)
        {
            using (TextFieldParser csvParser = new TextFieldParser(@"Data\user-list.csv"))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { $"," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names

                string temp;

                var csv = new StringBuilder();
                csv.AppendLine(csvParser.ReadLine());

                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    //temp = UserId
                    temp = fields[0];
                    if (Convert.ToInt32(temp) == user.userID)
                    {
                        //Delete User
                        continue;
                    }
                    else
                    {
                        var newLine = string.Format("{0},{1},{2},{3}", fields[0], fields[1], fields[2], fields[3]);
                        csv.AppendLine(newLine);
                    }
                }
                File.WriteAllText(@"Data\user-list.csv", csv.ToString());
                csvParser.Close();
            }
            return;
        }

        static void deleteUserInMovies(user user)
        {
            using (TextFieldParser csvParser = new TextFieldParser(@"Data\user-movie-rating.csv"))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { $"," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                string temp;

                var csv = new StringBuilder();
                csv.AppendLine(csvParser.ReadLine());


                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    //temp = UserId
                    temp = fields[0];
                    if (Convert.ToInt32(temp) == user.userID)
                    {
                        //Delete User
                        continue;
                    }
                    else
                    {
                        var newLine = string.Format("{0},{1},{2},{3}", fields[0], fields[1], fields[2], fields[3]);
                        csv.AppendLine(newLine);
                    }
                }
                File.WriteAllText(@"Data\user-movie-rating.csv", csv.ToString());
                csvParser.Close();
            }
            return;
        }
    }
}
