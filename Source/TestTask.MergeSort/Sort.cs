using System;
using System.Collections.Generic;
using TestTask.MergeSort.Threading;

namespace TestTask.MergeSort
{
    public static class Sort
    {
        #region Private Methods

        private static void MergeData<T>(IEnumerator<T> firstData, IEnumerator<T> secondData, IBuffer<T> result)
            where T : IComparable<T>
        {
            bool isFirstEmpty = !firstData.MoveNext();
            bool isSecondEmpty = !secondData.MoveNext();
            do
            {
                T first = firstData.Current;
                T second = secondData.Current;
                if(first.CompareTo(second)<=0)
                {
                    result.Add(first);
                    isFirstEmpty = !firstData.MoveNext();
                }
                else
                {
                    result.Add(second);
                    isSecondEmpty = !secondData.MoveNext();
                }
            } while (!isFirstEmpty && !isSecondEmpty);
            if(!isFirstEmpty)
            {
                do
                {
                    result.Add(firstData.Current);
                } while (firstData.MoveNext());
            }
            if (!isSecondEmpty)
            {
                do
                {
                    result.Add(secondData.Current);
                } while (secondData.MoveNext());
            }
        }

        private static List<IBuffer<T>> SplitInput<T>(IEnumerator<T> input, int bufferSize, IBufferFactory<T> temporalBufferFactory)
            where T : IComparable<T>
        {
            List<IBuffer<T>> buffers = new List<IBuffer<T>>();
            List<T> elements = new List<T>();
            while (input.MoveNext())
            {
                if (elements.Count >= bufferSize)
                {
                    FillBuffer(elements, buffers, temporalBufferFactory);
                }
                elements.Add(input.Current);
            }
            if(elements.Count > 0)
            {
                FillBuffer(elements, buffers, temporalBufferFactory);
            }
            return buffers;
        }

        private static void FillBuffer<T>(List<T> elements, List<IBuffer<T>> buffers, IBufferFactory<T> temporalBufferFactory)
        {
            IBuffer<T> buffer = temporalBufferFactory.CreateBuffer();
            elements.Sort();
            foreach (T element in elements)
            {
                buffer.Add(element);
            }
            buffers.Add(buffer);
            elements.Clear();
        }

        private static IBuffer<T> MergeBuffers<T>(IBuffer<T> first, IBuffer<T> second, IBufferFactory<T> temporalBufferFactory)
            where T : IComparable<T>
        {
            IBuffer<T> result = temporalBufferFactory.CreateBuffer();
            MergeData(first.GetData(), second.GetData(), result);
            first.Dispose();
            second.Dispose();
            return result;
        }

        #endregion

        /// <summary>
        /// Sorts input enumerator using merge sort algorithm and stores result into specified result buffer.
        /// </summary>
        /// <typeparam name="T">Input iterator's element type.</typeparam>
        /// <param name="bufferSize">Number of elements stored in temporal files.</param>
        /// <param name="threadCount">The amount of threads used for temporal file merge.</param>
        /// <param name="input">The input iteratpr.</param>
        /// <param name="result">The result buffer.</param>
        /// <param name="temporalBufferFactory">The temporal buffer factory.</param>
        public static void MergeSort<T>(int bufferSize, int threadCount, IEnumerator<T> input, IBuffer<T> result, IBufferFactory<T> temporalBufferFactory)
            where T : IComparable<T>
        {
            List<IBuffer<T>> buffers = SplitInput(input, bufferSize, temporalBufferFactory);
            if (buffers.Count > 0)
            {
                Func<IBuffer<T>, IBuffer<T>, IBuffer<T>> taskExecutor = (first,second)=>MergeBuffers(first,second, temporalBufferFactory);
                TaskProcessor<IBuffer<T>> taskProcessor = new TaskProcessor<IBuffer<T>>(taskExecutor, threadCount);
                foreach (IBuffer<T> buffer in buffers)
                {
                    taskProcessor.AddTask(buffer);
                }
                IEnumerator<T> resultData = taskProcessor.ExecuteTasks().GetData();
                while (resultData.MoveNext())
                {
                    result.Add(resultData.Current);
                }
            }
        }
    }
}