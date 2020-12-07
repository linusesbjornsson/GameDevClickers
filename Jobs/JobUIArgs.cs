public class JobUIArgs : JobArgs
{
    public int Index { get; private set; }
    public JobUIArgs(Job job, int index) : base(job)
    {
        Index = index;
    }
}
