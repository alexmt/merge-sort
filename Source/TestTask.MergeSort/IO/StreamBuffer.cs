using System;
using System.Collections.Generic;
using System.IO;

namespace TestTask.MergeSort.IO
{
    /// <summary>
    /// Provides ability to add new elements into specied stream.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <typeparam name="TReader">The type of the reader.</typeparam>
    /// <typeparam name="TWriter">The type of the writer.</typeparam>
    public class StreamBuffer<T, TReader, TWriter> : IBuffer<T>
        where TReader : IDisposable
        where TWriter : class, IDisposable
    {
        private readonly Stream stream;
        private readonly IStreamFormatter<T, TReader, TWriter> streamFormatter;
        private TWriter writer;
        private bool disposed;

        public StreamBuffer(Stream stream, IStreamFormatter<T, TReader, TWriter> streamFormatter)
        {
            this.streamFormatter = streamFormatter;
            this.stream = stream;
        }

        /// <summary>
        /// Adds the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        public void Add(T element)
        {
            streamFormatter.Write(writer ?? (writer = streamFormatter.CreateWriter(stream)), element);
        }

        /// <summary>
        /// Returns iterator to collected elements.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetData()
        {
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return new StreamEnumerator<T, TReader>(stream, streamFormatter);
        }

        public virtual void Dispose()
        {
            if (!disposed)
            {
                if (stream != null)
                {
                    stream.Flush();
                }
                if (writer != null)
                {
                    writer.Dispose();
                }
                disposed = true;
            }
        }
    }
}