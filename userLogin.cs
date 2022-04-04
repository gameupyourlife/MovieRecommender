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
    public class userLoginManagement
    {
        public static void userLogin(MLContext mlContext, ITransformer model)
        {
            AnsiConsole.Write(new Rule("[orange1]Login[/]").LeftAligned());

            string name = AnsiConsole.Ask<string>("What's your [green]name[/]?");


            var pPromt = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]password[/]?")
                    .PromptStyle("red")
                    .Secret());
            string password = Convert.ToString( pPromt[0] );

            // Creates the user; makes sure the user exists
            user lUser = new user(name, password,  mlContext,  model);
            var user = lUser;
            AnsiConsole.Clear();
            Interface.mainInterface(user,  mlContext,  model);
        }

        public static void userCreate(MLContext mlContext, ITransformer model)
        {
            AnsiConsole.Write(new Rule("[orange1]Create Account[/]").LeftAligned());

            string name = AnsiConsole.Ask<string>("What's your [green]name[/]?");

            string password = setNewPassword();
            

            // Is something for the future
            // hashPassword.encryptPassword(password);

            int id = getFreeUserId();
            var timestamp = DateTime.Now;
            var csv = new StringBuilder();

            //Write new user in user-list.csv
            var newLine = string.Format("{0},{1},{2},{3}", id, name, password, timestamp);
            csv.AppendLine(newLine);
            File.AppendAllText(@"Data\user-list.csv", csv.ToString());
            
            // Login process
            user lUser = new user(name, password, mlContext, model);
            var user = lUser;
            AnsiConsole.Clear();

            Interface.mainInterface(user, mlContext, model);
        }

        private static string setNewPassword()
        {
            var pPromt = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]password[/]?")
                    .PromptStyle("red")
                    .Secret());
            string password = Convert.ToString(pPromt[0]);

            if (password == "")
                setNewPassword();

            return password;
        }

        //public static void userDelete(user user, MLContext mlContext, ITransformer model)
        //{
        //    AnsiConsole.WriteLine("Are your sure to delete your account?");
        //    AnsiConsole.WriteLine("To procede pleas write - confirm -");
        //    string inp = Console.ReadLine().ToUpper();
        //    if (inp == "CONFIRM")
        //    {
        //        deleteUserInUser(user);
        //        deleteUserInMovies(user); //Has to be created
        //        Interface.InterfaceInit(mlContext, model);

        //    }
        //    else
        //    {
        //        AnsiConsole.WriteLine("Process of deletion abordet");
        //        Interface.mainInterface(user, mlContext, model);
        //    }

        //}

        //static void deleteUserInUser(user user)
        //{
        //    using (TextFieldParser csvParser = new TextFieldParser(@"Data\user-list.csv"))
        //    {
        //        csvParser.CommentTokens = new string[] { "#" };
        //        csvParser.SetDelimiters(new string[] { $"," });
        //        csvParser.HasFieldsEnclosedInQuotes = true;

        //        // Skip the row with the column names
                
        //        string temp;

        //        var csv = new StringBuilder();
        //        csv.AppendLine(csvParser.ReadLine());

        //        while (!csvParser.EndOfData)
        //        {
        //            string[] fields = csvParser.ReadFields();
        //            //temp = UserId
        //            temp = fields[0];
        //            if (Convert.ToInt32(temp) == user.userID)
        //            {
        //                //Delete User
        //                continue;
        //            }
        //            else
        //            {
        //                var newLine = string.Format("{0},{1},{2},{3}", fields[0], fields[1], fields[2], fields[3]);
        //                csv.AppendLine(newLine);
        //            }   
        //        }
        //        File.WriteAllText(@"Data\user-list.csv", csv.ToString());
        //        csvParser.Close();
        //    }
        //    return;
        //}

        //static void deleteUserInMovies(user user)
        //{
        //    using (TextFieldParser csvParser = new TextFieldParser(@"Data\user-movie-rating.csv"))
        //    {
        //        csvParser.CommentTokens = new string[] { "#" };
        //        csvParser.SetDelimiters(new string[] { $"," });
        //        csvParser.HasFieldsEnclosedInQuotes = true;

        //        // Skip the row with the column names
        //        string temp;

        //        var csv = new StringBuilder();
        //        csv.AppendLine(csvParser.ReadLine());


        //        while (!csvParser.EndOfData)
        //        {
        //            string[] fields = csvParser.ReadFields();
        //            //temp = UserId
        //            temp = fields[0];
        //            if (Convert.ToInt32(temp) == user.userID)
        //            {
        //                //Delete User
        //                continue;
        //            }
        //            else
        //            {
        //                var newLine = string.Format("{0},{1},{2},{3}", fields[0], fields[1], fields[2], fields[3]);
        //                csv.AppendLine(newLine);
        //            }
        //        }
        //        File.WriteAllText(@"Data\user-movie-rating.csv", csv.ToString());
        //        csvParser.Close();
        //    }
        //    return;
        //}

        // Looks for last user Id and increases its value by one
        // Maybe add a search algorythm for the lowest free id so that if a user in the middle of the list is deleted the id is given to an new user
        static int getFreeUserId()
        {
            int userId = Convert.ToInt32(readCsv.readLastCsvData(@"Data\user-list.csv", ",", true, 0));
            if(userId !>= 0) userId = 0;
            else
            {
                userId += 1;
            }
            return userId;
        }
    }

    // Stores the users login data and enables the functionality
    public class user
    {
        public int userID { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }

        public user(string name,string password, MLContext mlContext, ITransformer model)
        {
            userName = name;
            userPassword = password;

            // Checks if the user exists
            try
            {
                // Catches the user id of the user which wants to log in
                userID = Convert.ToInt32(readCsv.readCurrentUserID(name));
            }
            catch (Exception)
            {
                AnsiConsole.WriteLine("Error - No User found");
                Interface.LoginSequence( mlContext,  model);
            }


        }
    }
}
