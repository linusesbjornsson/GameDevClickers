using System;

public class StatArgs : EventArgs
{
    public BigRational OldStat { get; private set; }
    public BigRational NewStat { get; private set; }
    public StatArgs(BigRational oldStat, BigRational newStat)
    {
        OldStat = oldStat;
        NewStat = newStat;
    }
}
