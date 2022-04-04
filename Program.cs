using Microsoft.ML;
using Microsoft.ML.Trainers;
using System;
using System.IO;
using System.Text;
using CsvHelper;
using Microsoft.VisualBasic.FileIO;
using MovieRecommender;
using System.Runtime.InteropServices;
using Spectre.Console;

Console.Title = "Movie Recommender";



// Main Structure
//Start ML Sequence
MLContext mlContext = new MLContext();

//Load the Training Data in a IDataView
(IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);




//Erstellen und Trainieren des MLM
ITransformer model = BuildAndTrainModel(mlContext, trainingDataView);

//Überprüfen des MLM anhand von Überprüfungsdaten
EvaluateModel(mlContext, testDataView, model);

    //Testen des MLM anhand von Test Eingabedaten zur Vorhersage der Bewertung
    // UseModelForSinglePrediction(mlContext, model);

    //Abspecihern des MLM
    SaveModel(mlContext, trainingDataView.Schema, model);


//createRandomData();
AnsiConsole.Clear();
readCsvData();

//Initialising the Interface the user interacts with
Interface.LoadInterface(mlContext, model);
//Interface.InterfaceInit(mlContext, model);


(IDataView training, IDataView test) LoadData(MLContext mlContext)
{
    var trainingDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-train.csv");
    var testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-test.csv");

    IDataView trainingDataView = mlContext.Data.LoadFromTextFile<MovieRating>(trainingDataPath, hasHeader: true, separatorChar: ',');
    IDataView testDataView = mlContext.Data.LoadFromTextFile<MovieRating>(testDataPath, hasHeader: true, separatorChar: ',');

    return (trainingDataView, testDataView);
}



ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
{
    IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: "userId")
    .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "movieIdEncoded", inputColumnName: "movieId"));

    var options = new MatrixFactorizationTrainer.Options
    {
        MatrixColumnIndexColumnName = "userIdEncoded",
        MatrixRowIndexColumnName = "movieIdEncoded",
        LabelColumnName = "Label",
        NumberOfIterations = 100,
        ApproximationRank = 100
    };

    var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));
    AnsiConsole.WriteLine("=============== Training the model ===============");
    ITransformer model = trainerEstimator.Fit(trainingDataView);

    return model;
}



void EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
{
    AnsiConsole.WriteLine("=============== Evaluating the model ===============");
    var prediction = model.Transform(testDataView);

    var metrics = mlContext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");

    AnsiConsole.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
    AnsiConsole.WriteLine("RSquared: " + metrics.RSquared.ToString());
}



void UseModelForSinglePrediction(MLContext mlContext, ITransformer model)
{
    AnsiConsole.WriteLine("=============== Making a prediction ===============");
    var predictionEngine = mlContext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(model);
    var recomendedMovies = new List<float>();
    var recomendedRating = new List<float>();
    var userId = 6;

    for (int i = 0; i < 10000; i++)
    {
        var testInput = new MovieRating { userId = userId, movieId = i };


        var movieRatingPrediction = predictionEngine.Predict(testInput);

        if (Math.Round(movieRatingPrediction.Score, 1) > 3.5)
        {
            AnsiConsole.WriteLine("Movie " + testInput.movieId + " is recommended for user " + testInput.userId);
            recomendedMovies.Add(testInput.movieId);
            recomendedRating.Add(movieRatingPrediction.Score);
        }
        else
        {
            AnsiConsole.WriteLine("-----Movie " + testInput.movieId + " is not recommended for user " + testInput.userId);
        }
    }

    for (int i = 0; i < recomendedMovies.Count; i++)
    {
        for (int x = 0; x < recomendedRating.Count-1; x++)
        {
            if (recomendedRating[index: x] <= recomendedRating[index: x+1])
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

    for (int i = 0; i < 10; i++)
    {
        AnsiConsole.WriteLine("Movie " + recomendedMovies[index: i] + " is with a rating of " + recomendedRating[index: i] + " inside the Top 10 of User" + userId);
    }
}


void SaveModel(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
{
    var modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "MovieRecommenderModel.zip");

    AnsiConsole.WriteLine("=============== Saving the model to a file ===============");
    mlContext.Model.Save(model, trainingDataViewSchema, modelPath);
}

//Uses a String Builder to create and write random data in csv
void createRandomData()
{
    Random rnd = new Random();
    var csv = new StringBuilder();

    for (int i = 0; i < 10000; i++)
    {
        //AnsiConsole.WriteLine("userId:");
        var userId = $"{rnd.Next(10)}";
        //AnsiConsole.WriteLine("movieId:");
        var movieId = $"{rnd.Next(10000)}";
        //AnsiConsole.WriteLine("Rating:");
        var rating = $"{rnd.Next(1,5)}";
        var timestamp = DateTime.Now;

        //To avoid nulls while manaual entry
        //while (userId == "")
        //{
        //    AnsiConsole.WriteLine("userId:");
        //    userId = Console.ReadLine();
        //}
        //while (movieId == "")
        //{
        //    AnsiConsole.WriteLine("movieId:");
        //    movieId = Console.ReadLine();
        //}
        //while (rating == "")
        //{
        //    AnsiConsole.WriteLine("rating:");
        //    rating = Console.ReadLine();
        //}
        var newLine = string.Format("{0},{1},{2},{3}", userId, movieId, rating, timestamp);
        csv.AppendLine(newLine);
    }

    AnsiConsole.WriteLine("=============== Finished Creating Random Data ===============");
    File.AppendAllText(@"Data\recommendation-ratings-user.csv", csv.ToString());
}

//Reads a csv via a textparser. I don't think this function is used but it is a example
void readCsvData()
{
    var path = @"Data\movie-list.csv"; // Habeeb, "Dubai Media City, Dubai"
    using (TextFieldParser csvParser = new TextFieldParser(path))
    {
        csvParser.CommentTokens = new string[] { "#" };
        csvParser.SetDelimiters(new string[] { ";" });
        csvParser.HasFieldsEnclosedInQuotes = true;

        // Skip the row with the column names
        csvParser.ReadLine();
        var movieId = new List<string>();
        var movieName = new List<string>();
        var releaseDate = new List<string>();

        while (!csvParser.EndOfData)
        {
            // Read current line fields, pointer moves to the next line.
            string[] fields = csvParser.ReadFields();
            //movieId,movieName,releaseDate,
            //string movieId = fields[0];
            //Adds the current field into the list
            movieId.Add(fields[0]);
            movieName.Add(fields[1]);
            releaseDate.Add(fields[2]);
           
        }
        //for (int x = 0; x < movieId.Count; x++)
        //{
        //    AnsiConsole.WriteLine(movieId[x]);
        //    AnsiConsole.WriteLine(movieName[x]);
        //    AnsiConsole.WriteLine(releaseDate[x]);
        //}
        

        csvParser.Close();
    }
}

