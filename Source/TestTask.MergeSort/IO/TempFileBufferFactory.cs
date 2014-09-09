using System;
using System.IO;

namespace TestTask.MergeSort.IO
{
    /// <summary>
    /// Creates new buffer over temporal file in specified directory.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    /// <typeparam name="TReader">The type of the reader.</typeparam>
    /// <typeparam name="TWriter">The type of the writer.</typeparam>
    public class TempFileBufferFactory<TElement, TReader, TWriter> : IBufferFactory<TElement> 
        where TReader : IDisposable
        where TWriter : class, IDisposable
    {
        private readonly string directoryPath;
        private readonly IStreamFormatter<TElement, TReader, TWriter> streamFormatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TempFileBufferFactory&lt;TElement, TReader, TWriter&gt;"/> class.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="streamFormatter">The stream formatter.</param>
        public TempFileBufferFactory(string directoryPath, 
            IStreamFormatter<TElement, TReader, TWriter> streamFormatter)
        {
            this.directoryPath = directoryPath;
            this.streamFormatter = streamFormatter;
        }

        /// <summary>
        /// Creates the new buffer.
        /// </summary>
        /// <returns></returns>
        public IBuffer<TElement> CreateBuffer()
        {
            string tempFilePath = Path.Combine(directoryPath, Path.GetRandomFileName());
            return new TempFileBuffer<TElement, TReader, TWriter>(tempFilePath, streamFormatter);
        }
    }
}