using System;

public class JobArgs : EventArgs
{
    public Job Job { get; private set; }
    public JobArgs(Job job)
    {
        Job = job;
    }
}
