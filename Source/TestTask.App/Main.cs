using System;
using NConsoler;
using System.IO;
using TestTask.MergeSort;
using TestTask.MergeSort.IO;

namespace TestTask.App
{
	class MainClass
	{
		static void Main(string[] args)
        {
            Consolery.Run(typeof (MainClass), args);
        }

        [Action("Generate input file")]
        public static void GenerateInputFile(
            [Required(Description = "Result file path")]
            string filePath, 
            [Required(Description = "Numbers count")]
            int numberCount,
            [Optional(false, Description = "If set to true, then random file will be generated, overwise sequential file.")]
            bool random)
        {
            StringLineIntFormatter formatter = new StringLineIntFormatter();
            Random rand = new Random();
            using (StreamWriter writer = formatter.CreateWriter(File.Create(filePath)))
            {
                while (numberCount-- > 0)
                {
                    formatter.Write(writer, random ? rand.Next(numberCount) : numberCount+1);
                }
            }
        }

        [Action("Sort input file")]
        public static void SortInputFile(
            [Required(Description = "Source file path")]
            string sourceFilePath,
            [Required(Description = "Output file path")]
            string outputFilePath,
            [Optional(4, Description = "Processing thread count")] 
            int processingThreadCount,
            [Optional(1024*10, Description = "Buffer size kilobytes")] 
            int bufferSize)
        {
            using (Stream inputStream = File.OpenRead(sourceFilePath))
            {
                using (Stream resultStream = File.Open(outputFilePath, FileMode.Create))
                {
                    string tempFilesDirectory = Path.GetTempPath();
                    BinaryIntFormatter tempFileFormatter = new BinaryIntFormatter();
                    TempFileBufferFactory<int, BinaryReader, BinaryWriter> bufferFactory = new TempFileBufferFactory<int, BinaryReader, BinaryWriter>(tempFilesDirectory, tempFileFormatter);
                    using (IBuffer<int> result = new StreamBuffer<int, StreamReader, StreamWriter>(resultStream, new StringLineIntFormatter()))
                    {
                        StreamEnumerator<int, StreamReader> input = new StreamEnumerator<int, StreamReader>(inputStream, new StringLineIntFormatter());
                        int chunkSize = bufferSize*1024/sizeof (int);
                        DateTime start = DateTime.Now;
                        Console.Write("Start sorting... ");
                        Sort.MergeSort(chunkSize, processingThreadCount, input, result, bufferFactory);
                        Console.WriteLine("Done. Sorting is complited for {0}.", DateTime.Now - start);
                    }
                }
            }
        }
	}
}
