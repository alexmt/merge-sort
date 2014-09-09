using System.IO;

namespace TestTask.MergeSort.IO
{
    public interface IStreamReader<TElement, TReader>
    {
        /// <summary>
        /// Creates the reader for specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        TReader CreateReader(Stream stream);

        /// <summary>
        /// Parses the next element from reader.
        /// </summary>
        /// <param name="reader">The stream reader.</param>
        /// <param name="element">The new element.</param>
        /// <returns><c>True</c> if element has been parsed, otherwise <c>false</c>.</returns>
        bool ParseNext(TReader reader, out TElement element);
    }

    public interface IStreamFormatter<TElement, TReader, TWriter> : IStreamReader<TElement, TReader>
    {
        /// <summary>
        /// Creates the writer for specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        TWriter CreateWriter(Stream stream);

        void Write(TWriter writer, TElement element);
    }
}
