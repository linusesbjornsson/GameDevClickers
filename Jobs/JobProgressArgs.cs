using System;

public class JobProgressArgs : EventArgs
{
    public BigRational Progress { get; private set; }
    public bool Idle { get; private set; }
    public JobProgressArgs(BigRational progress, bool idle)
    {
        Progress = progress;
        Idle = idle;
    }
}
