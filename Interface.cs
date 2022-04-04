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
using Spectre.Console;

namespace MovieRecommender
{
    public class Interface
    {
        //Booting sequenze animation
        public static void LoadInterface(MLContext mLContext, ITransformer model)
        {
            AnsiConsole.Status()
            .Spinner(Spinner.Known.BetaWave)
            .Start("Starting", ctx =>
            {
                ctx.Spinner(Spinner.Known.BetaWave);
                ctx.SpinnerStyle(Style.Parse("green"));

                // Simulate some work
                AnsiConsole.MarkupLine("System Offline");
                Thread.Sleep(1000);

                // Update the status and spinner
                ctx.Status("Loading all Files");

                // Simulate some work
                AnsiConsole.MarkupLine("System Booted");
                Thread.Sleep(2000);


                ctx.Status("Preparing advanced AI");
                
                AnsiConsole.MarkupLine("All Files Loaded");
                Thread.Sleep(2000);

                ctx.Status("Initialising Interface");

                AnsiConsole.MarkupLine("AI Ready");
                Thread.Sleep(2000);

                AnsiConsole.MarkupLine("System Initialised");
            });
            InterfaceInit(mLContext, model);
        }

        //Main Interface the user interacts with. Contains all the possible options you can choose
        public static void InterfaceInit(MLContext mlContext, ITransformer model)
        {
            AnsiConsole.Write(new Rule("[orange1]Login/Create Instance[/]").LeftAligned());
            

            var inp = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("How do you want to [green]procede[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(new[] {
            "Login", "Create Account", "Exit the Program",
                    }));

            if (inp == "Login")
                userLoginManagement.userLogin( mlContext,  model);
            else if (inp == "Create Account")
                userLoginManagement.userCreate(mlContext,  model);
            else if (inp == "Exit the Program")
                Environment.Exit(0);
        }


        public static void LoginSequence(MLContext mlContext, ITransformer model)
        {
            //User selects login to a account or create a account
            AnsiConsole.Write(new Rule("[orange1]Login/Create Instance[/]").LeftAligned());

            var inp = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("How do you want to [green]procede[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(new[] {
            "Login", "Create Account", "Exit the Program"
                    }));

            if (inp == "Login")
                userLoginManagement.userLogin(mlContext, model);
            else if (inp == "Create Account")
                userLoginManagement.userCreate(mlContext, model);
            else if (inp == "Exit the Program")
                Environment.Exit(0);

        }

        public static void mainInterface(user user, MLContext mlContext, ITransformer model)
        {
            AnsiConsole.Write(new Rule("[orange1]Main Interface[/]").LeftAligned());

            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What do you want to [green]do[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(new[] {
                        "Rate a Movie", "Get your Top Ten Movie Recommendations", "See all your rated Movies", 
                        "Logout", "Exit the Program", "Delete your Account",
                    }));

            if (action == "Get your Top Ten Movie Recommendations")
                topTen.outTopTen(user, mlContext, model);
            else if (action == "Rate a Movie")
                recommend.inpRecommend(user, mlContext, model);
            else if (action == "See all your rated Movies")
                rated.getRatedMovies(user, mlContext, model);
            else if (action == "Exit the Program")
                Environment.Exit(0);
            else if (action == "Logout")
            {
                AnsiConsole.Clear();
                InterfaceInit(mlContext, model);
            }
            else if (action == "Delete your Account")
                userDeleteManagement.userDelete(user, mlContext, model);
            else
                mainInterface(user, mlContext, model);
        }
    }
}
