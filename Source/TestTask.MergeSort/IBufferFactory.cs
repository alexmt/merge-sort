namespace TestTask.MergeSort
{
    public interface IBufferFactory<T>
    {
        /// <summary>
        /// Creates the new buffer.
        /// </summary>
        /// <returns></returns>
        IBuffer<T> CreateBuffer();
    }
}