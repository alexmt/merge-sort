using System;
using System.Collections.Generic;

namespace TestTask.MergeSort
{
    public interface IBuffer<T> : IDisposable
    {
        /// <summary>
        /// Adds the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        void Add(T element);

        /// <summary>
        /// Returns iterator to collected elements.
        /// </summary>
        /// <returns></returns>
        IEnumerator<T> GetData();
    }
}