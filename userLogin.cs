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
    public class userLoginManagement
    {
        public static void userLogin(MLContext mlContext, ITransformer model)
        {
            Console.WriteLine("Name:");
            string name = Console.ReadLine();

            Console.WriteLine("Password:");
            string password = Console.ReadLine();
            user lUser = new user(name, password,  mlContext,  model);
            var user = lUser;
            Interface.mainInterface(user,  mlContext,  model);
        }

        public static void userCreate(MLContext mlContext, ITransformer model)
        {
            Console.Write("User Name:");
            string name = Console.ReadLine();
            while (name == "")
            {
                name = Console.ReadLine();
            }

            Console.Write("Password:");
            string password = Console.ReadLine();
            while (password == "")
            {
                password = Console.ReadLine();
            }

            int id = getFreeUserId();

            var timestamp = DateTime.Now;

            Random rnd = new Random();
            var csv = new StringBuilder();

            var newLine = string.Format("{0},{1},{2},{3}", id, name, password, timestamp);
            csv.AppendLine(newLine);
            File.AppendAllText(@"C:\Users\Cedric\source\repos\MovieRecommender\Data\user-list.csv", csv.ToString());
            
            user lUser = new user(name, password, mlContext, model);
            var user = lUser;

            Interface.mainInterface(user, mlContext, model);
        }

        public static void userDelete(user user, MLContext mlContext, ITransformer model)
        {
            Console.WriteLine("Are your sure to delete your account?");
            Console.WriteLine("To procede pleas write - confirm -");
            string inp = Console.ReadLine().ToUpper();
            if (inp == "CONFIRM")
            {
                deleteUserInUser(user);
                deleteUserInMovies(user); //Has to be created
                Interface.InterfaceInit(mlContext, model);

            }
            else
            {
                Console.WriteLine("Process of deletion abordet");
                Interface.mainInterface(user, mlContext, model);
            }

        }

        static void deleteUserInUser(user user)
        {
            using (TextFieldParser csvParser = new TextFieldParser(@"C:\Users\Cedric\source\repos\MovieRecommender\Data\user-list.csv"))
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
                File.WriteAllText(@"C:\Users\Cedric\source\repos\MovieRecommender\Data\user-list.csv", csv.ToString());
                csvParser.Close();
            }
            return;
        }

        static void deleteUserInMovies(user user)
        {
            using (TextFieldParser csvParser = new TextFieldParser(@"C:\Users\Cedric\source\repos\MovieRecommender\Data\user-movie-rating.csv"))
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
                File.WriteAllText(@"C:\Users\Cedric\source\repos\MovieRecommender\Data\user-movie-rating.csv", csv.ToString());
                csvParser.Close();
            }
            return;
        }

        static int getFreeUserId()
        {
            int userId = Convert.ToInt32(readCsv.readLastCsvData(@"C:\Users\Cedric\source\repos\MovieRecommender\Data\user-list.csv", ",", true, 0));
            if(userId !>= 0) userId = 0;
            else
            {
                userId += 1;
            }
            return userId;
        }
    }

    public class user
    {
        public int userID { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }

        public user(string name,string password, MLContext mlContext, ITransformer model)
        {
            userName = name;
            userPassword = password;

            try
            {
                userID = Convert.ToInt32(readCsv.readCurrentUserID(name));
            }
            catch (Exception)
            {
                Console.WriteLine("Error - No User found");
                Interface.LoginSequence( mlContext,  model);
            }


        }
    }
}
