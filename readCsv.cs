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

namespace MovieRecommender
{
    public class readCsv
    {
        //string readCsvData(string path, string delimiter, bool header, int collum, int field)
        //{
        //    using (TextFieldParser csvParser = new TextFieldParser(path))
        //    {
        //        csvParser.SetDelimiters(new string[] { delimiter });

        //        // Skip the row with the column names
        //        if (header) csvParser.ReadLine();

        //        string results;

        //        while (!csvParser.EndOfData)
        //        {
        //            // Read current line fields, pointer moves to the next line.
        //            string[] fields = csvParser.ReadFields();
        //            results = fields[collum];
        //        }

        //        csvParser.Close();
        //        return results;
        //    }
        //}

        public static string readLastCsvData(string path, string delimiter, bool header, int collum)
        {
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { $"{delimiter}" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                if (header) csvParser.ReadLine();
                string results = "";

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();
                    //movieId,movieName,releaseDate,
                    //string movieId = fields[0];
                    results = fields[collum];

                }

                csvParser.Close();
                Console.WriteLine("Results: " + results);
                return results;
            }


        }

        public static string readCurrentUserID(string name)
        {
            using (TextFieldParser csvParser = new TextFieldParser(@"C:\Users\Cedric\source\repos\MovieRecommender\Data\user-list.csv"))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { $"," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();
                string results = "";

                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    results = fields[1];
                    if (results == name)
                    {
                        results = fields[0];
                        csvParser.Close();
                        Console.WriteLine("Results: " + results);
                        return results;
                    }
                }


                csvParser.Close();
                results = "error";
                Console.WriteLine("Results: " + results);
                return results;
            }
        }
    }
}
