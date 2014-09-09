using System;
using System.Collections.Generic;
using System.Threading;

namespace TestTask.MergeSort.Threading
{
    /// <summary>
    /// Provides ability to process generic tasks using multiple threads.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TaskProcessor<T> : IDisposable
    {
        #region Private Fields

        private readonly Stack<T> pendingTasks = new Stack<T>();
        private readonly Func<T, T, T> taskExecutor;
        private readonly int maxProcessingCount;
        private readonly ManualResetEvent taskCompletedEvent;
        private int processingCount;

        #endregion

        #region Constructor

        public TaskProcessor(Func<T, T, T> taskExecutor, int maxProcessingCount)
        {
            this.taskExecutor = taskExecutor;
            this.maxProcessingCount = maxProcessingCount;
            taskCompletedEvent = new ManualResetEvent(false);
        }

        #endregion

        #region Private Methods

        private void Process(T first, T second)
        {
            T item = taskExecutor(first, second);
            T next;
            while (TryGetPending(out next))
            {
                item = taskExecutor(item, next);
            }
            lock (pendingTasks)
            {
                processingCount--;
                AddTask(item);
            }
        }

        private bool TryGetPending(out T item)
        {
            lock (pendingTasks)
            {
                if(pendingTasks.Count > 0)
                {
                    item = pendingTasks.Pop();
                    return true;
                }
                item = default(T);
                return false;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the new pending task.
        /// </summary>
        /// <param name="item">The pending task.</param>
        public void AddTask(T item)
        {
            bool isCompleted = pendingTasks.Count == 0 && processingCount == 0;
            pendingTasks.Push(item);
            if (isCompleted)
            {
                taskCompletedEvent.Set();
            }
            else
            {
                taskCompletedEvent.Reset();
            }
        }

        /// <summary>
        /// Executes pending tasks.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws if no pending tasks have been specieid.</exception>
        /// <returns>Tasks execution result.</returns>
        public T ExecuteTasks()
        {
            if (pendingTasks.Count == 0)
            {
                throw new InvalidOperationException("No tasks were specified.");
            }
            lock (pendingTasks)
            {
                while (pendingTasks.Count > 1 && processingCount <= maxProcessingCount)
                {
                    processingCount++;
                    T first = pendingTasks.Pop();
                    T second = pendingTasks.Pop();
                    ThreadPool.QueueUserWorkItem(input => Process(first, second));
                }
            }
            taskCompletedEvent.WaitOne();
            return pendingTasks.Pop();
        }

        public void Dispose()
        {
            if(taskCompletedEvent != null)
            {
                ((IDisposable)taskCompletedEvent).Dispose();
            }
        }

        #endregion
    }
}
