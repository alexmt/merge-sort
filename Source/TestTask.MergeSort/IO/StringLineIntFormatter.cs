using System;
using System.IO;

namespace TestTask.MergeSort.IO
{
    /// <summary>
    /// Presents ability to read/write integer into specifed stream using text format.
    /// </summary>
    public class StringLineIntFormatter : IStreamFormatter<int, StreamReader, StreamWriter>
    {
        /// <summary>
        /// Creates the reader for specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public StreamReader CreateReader(Stream stream)
        {
            return new StreamReader(stream);
        }

        /// <summary>
        /// Parses the next element from reader.
        /// </summary>
        /// <param name="reader">The stream reader.</param>
        /// <param name="element">The new element.</param>
        /// <returns><c>True</c> if element has been parsed, otherwise <c>false</c>.</returns>
        public bool ParseNext(StreamReader reader, out int element)
        {
            if (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                if(!int.TryParse(line, out element))
                {
                    string message = string.Format("Cannot convert '{0}' into integer.", line);
                    throw new ApplicationException(message);
                }
                return true;
            }
            element = 0;
            return false;
        }

        /// <summary>
        /// Creates the writer for specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public StreamWriter CreateWriter(Stream stream)
        {
            return new StreamWriter(stream);
        }

        public void Write(StreamWriter writer, int element)
        {
            writer.WriteLine(element);
        }
    }
}