using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TestTask.MergeSort.IO
{
    /// <summary>
    /// Presents enumerator over specified stream using specified stream formatter.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    /// <typeparam name="TReader">The type of the reader.</typeparam>
    public class StreamEnumerator<TElement, TReader> : IEnumerator<TElement>
        where TReader : IDisposable
    {
        #region Private Fields

        private readonly TReader reader;
        private readonly IStreamReader<TElement, TReader> streamFormatter;
        private TElement current;
        private readonly Stream stream;
        private bool disposed;

        #endregion

        #region Constructor

        public StreamEnumerator(Stream stream, IStreamReader<TElement, TReader> streamFormatter)
        {
            this.stream = stream;
            this.streamFormatter = streamFormatter;
            reader = streamFormatter.CreateReader(stream);
        }

        #endregion

        #region ISource<TElement> implementation

        public TElement Current
        {
            get
            {
                return current;
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                reader.Dispose();
                disposed = true;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public bool MoveNext()
        {
            return streamFormatter.ParseNext(reader, out current);
        }

        public void Reset()
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        #endregion
    }
}