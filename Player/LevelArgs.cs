using System;
using System.Numerics;

public class LevelArgs : EventArgs
{
    public BigInteger OldLevel { get; private set; }
    public BigInteger NewLevel { get; private set; }
    public LevelArgs(BigInteger oldLevel, BigInteger newLevel)
    {
        OldLevel = oldLevel;
        NewLevel = newLevel;
    }
}
