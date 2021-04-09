using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Spark.Sql;
using Microsoft.VisualBasic.FileIO;

namespace SudokuCombinatorialEvolutionSolver
{
    internal static class Program
    {

        static void Main(string[] args)
        {
            
            string filepath = "/Users/sarahvasquez/Desktop/Sudoku/SudokuCombinatorialEvolutionSolver/sudoku.csv";

            var temps1 = new System.Diagnostics.Stopwatch();

            var noyau = "1";
            var noeud = "1";

            temps1.Start();
            runSpark(filepath, noyau, noeud, 10);
            temps1.Stop();

            Console.WriteLine($"Temps d'exécution pour " + noyau + " noyau et " + noeud + " noeud: " + temps1.ElapsedMilliseconds + " ms");

        }


        static void runSpark(string file_path, string cores, string nodes, int nrows)
        {

            // Create Spark session 
            SparkSession spark =
                SparkSession
                    .Builder()
                    .AppName("Resolution de " + nrows + " sudokus par évolution combinatoire de " + cores + " noyau(x) et " + nodes + " noeud(s)")
                    .Config("spark.executor.cores", cores)
                    .Config("spark.executor.instances", nodes)
                    .GetOrCreate();

            // Create initial DataFrame
            DataFrame dataFrame = spark
                .Read()
                .Option("header", true)
                .Option("inferSchema", true)
                .Schema("quizzes string, solutions string")
                .Csv(file_path);

            DataFrame dataFrame2 = dataFrame.Limit(nrows);

            spark.Udf().Register<string, string>(
                "SukoduUDF",
                (sudoku) => sudokusolution(sudoku));

            dataFrame2.CreateOrReplaceTempView("Resolved");
            DataFrame sqlDf = spark.Sql("SELECT quizzes, SukoduUDF(quizzes) as Resolution from Resolved");
            sqlDf.Show();

            spark.Stop();
            Console.WriteLine("\n\n\n\n\n\n");
            Console.WriteLine("Test");
            Console.WriteLine("\n\n\n\n\n\n");
        }

        static string sudokusolution(string grid)
        {
            

            int[,] sudoku = new int[9, 9];
            
            for (int i = 0; i < 9; i++)
            {
                    
                for (int index = 0; index < 9; index++)
                {
                    sudoku[i, index] = Convert.ToInt32(grid[index]);
                }
            }


            Console.WriteLine("Begin solving Sudoku using combinatorial evolution");
            Console.WriteLine("The Sudoku is:");
            Console.WriteLine(sudoku.ToString());

            const int numOrganisms = 200;
            const int maxEpochs = 5000;
            const int maxRestarts = 40;
            Console.WriteLine($"Setting numOrganisms: {numOrganisms}");
            Console.WriteLine($"Setting maxEpochs: {maxEpochs}");
            Console.WriteLine($"Setting maxRestarts: {maxRestarts}");

            var solver = new SudokuSolver();
            var solvedSudoku = solver.Solve(sudoku, numOrganisms, maxEpochs, maxRestarts);

            Console.WriteLine("Best solution found:");
            Console.WriteLine(solvedSudoku.ToString());
            Console.WriteLine(solvedSudoku.Error == 0 ? "Success" : "Did not find optimal solution");
            Console.WriteLine("End Sudoku using combinatorial evolution");

            return Convert.ToString(solvedSudoku);
        }

        



    }
}