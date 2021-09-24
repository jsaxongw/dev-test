using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StringPerformanceTest
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        string[] Lines;

        public int NumberOfLines;

        [Params("Bacon", "pork", "prosciutto")]
        public string SearchValue;

        [Params("Files/Bacon10.txt", "Files/Bacon25.txt", "Files/Bacon50.txt")]
        public string FileToRead;

        [GlobalSetup]
        public void GlobalSetup()
        {
            string fileLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FileToRead);
            Lines = File.ReadAllLines(fileLocation);
            NumberOfLines = Lines.Count();
        }


        [Benchmark]
        public int CountOccurrences()
        {
            return IndexOfMethod();
        }

        private int RegexMethod()
        {
            throw new NotImplementedException();
        }

        private int IndexOfMethod()
        {
            int occurences = 0;

            foreach (var line in Lines)
            {
                int index = line.IndexOf(SearchValue);

                while (index != -1)
                {
                    occurences++;
                    index = line.IndexOf(SearchValue, index + SearchValue.Length);
                }
            }

            return occurences;
        }

    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmark>(new DebugInProcessConfig());
        }
    }
}
