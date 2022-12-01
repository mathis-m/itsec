using System;
using System.ComponentModel;
using System.IO;

namespace FrequencyAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = args.Length == 0 ? "C:\\src\\itsec\\lab4\\toDecrypt.txt" : args[0];

            var text = File.ReadAllText(path);
            //
            // var newText = "";
            // var index = 0;
            // while (index < text.Length)
            // {
            //     newText += text[index];
            //     index += 12;
            // }

            var frequencyAnalyzer = new FrequencyAnalyzer(text);
            frequencyAnalyzer.PrintResults();
            var csv = frequencyAnalyzer.GetCsvData();

            var outDir = Path.GetDirectoryName(path);
            var csvFilePath = Path.Combine(outDir ?? throw new InvalidOperationException(), "histogram.csv");

            File.WriteAllText(csvFilePath, csv);
            Console.ReadKey();
        }
    }
}
