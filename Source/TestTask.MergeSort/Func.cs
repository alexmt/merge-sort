namespace TestTask.MergeSort
{
    public delegate TResult Func<TResult>();
    public delegate TResult Func<TFirst, TResult>(TFirst first);
    public delegate TResult Func<TFirst, TSecond, TResult>(TFirst first, TSecond second);
}
