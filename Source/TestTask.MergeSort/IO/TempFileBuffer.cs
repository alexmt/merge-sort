using System;
using System.IO;

namespace TestTask.MergeSort.IO
{
    /// <summary>
    /// Provides ability to add new elements into specied file.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <typeparam name="TReader">The type of the reader.</typeparam>
    /// <typeparam name="TWriter">The type of the writer.</typeparam>
    public class TempFileBuffer<T, TReader, TWriter> : StreamBuffer<T, TReader, TWriter>
        where TReader : IDisposable
        where TWriter : class, IDisposable
    {
        private readonly string filePath;
        private bool disposed;

        public TempFileBuffer(string filePath, IStreamFormatter<T, TReader, TWriter> streamFormatter) : 
            base(File.Create(filePath), streamFormatter)
        {
            this.filePath = filePath;
        }

        public override void Dispose()
        {
            try
            {
                base.Dispose();
            }
            finally
            {
                if (!disposed)
                {
                    File.Delete(filePath);
                    disposed = true;
                }
            }
        }
    }
}