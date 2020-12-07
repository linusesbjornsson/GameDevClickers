using System;
using System.Numerics;

public class ExperienceArgs : EventArgs
{
    public BigInteger OldExperience { get; private set; }
    public BigInteger NewExperience { get; private set; }

    public ExperienceArgs(BigInteger oldExperience, BigInteger newExperience)
    {
        OldExperience = oldExperience;
        NewExperience = newExperience;
    }
}
