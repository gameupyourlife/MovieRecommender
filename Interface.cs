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

namespace MovieRecommender
{
    public class Interface
    {
        public static void InterfaceInit(MLContext mlContext, ITransformer model)
        {
            Console.WriteLine("===== Initialising Boot Sequence =====");
            Console.WriteLine("Hello welcome to your Movie Recommender!");
            Console.WriteLine("We are going to start by creating or logging in to your account.");
            Console.WriteLine("Pleas wirte login or create to procede.");
            var inp = Console.ReadLine();
            if (inp == "login" || inp == "l")
                userLoginManagement.userLogin( mlContext,  model);
            else if (inp == "crate" || inp == "c")
                userLoginManagement.userCreate(mlContext,  model);
            else
                LoginSequence(mlContext, model);            
            }

        public static void LoginSequence(MLContext mlContext, ITransformer model)
        {
            //User selects login to a account or create a account
            Console.WriteLine("Pleas wirte login or create to procede.");
            var inp = Console.ReadLine();
            if (inp == "login" || inp == "l")
                userLoginManagement.userLogin(mlContext, model);
            else if (inp == "crate" || inp == "c")
                userLoginManagement.userCreate(mlContext, model);
            else
                LoginSequence(mlContext, model);


        }

        public static void mainInterface(user user, MLContext mlContext, ITransformer model)
        {
            //Allows the user to interact with the Program
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You are now loged in congrats! \n You can etheir return your top 10 recommended movies \n rate a new movie \n or see your ratings. \n Write Top or Recommend or Ratings.");
            Console.ForegroundColor = ConsoleColor.White;
            string action = Console.ReadLine().ToUpper();
            if (action == "TOP" || action == "T")
                topTen.outTopTen(user, mlContext, model);
            else if (action == "RECOMMEND" || action == "RE")
                recommend.inpRecommend(user, mlContext, model);
            else if (action == "RATINGS" || action == "RA")
                rated.getRatedMovies(user, mlContext, model);
            else if (action == "EXIT" || action == "E")
                Environment.Exit(0);
            else if (action == "LOGOUT" || action == "L")
                InterfaceInit(mlContext, model);
            else if (action == "DELETE" || action == "D")
                userLoginManagement.userDelete(user, mlContext, model);
            else
                mainInterface(user, mlContext, model);
        }
    }
}
