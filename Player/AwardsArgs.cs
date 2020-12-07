using System;

public class AwardsArgs : EventArgs
{
    public BigRational OldAwards { get; private set; }
    public BigRational NewAwards { get; private set; }
    public AwardsArgs(BigRational oldAwards, BigRational newAwards)
    {
        OldAwards = oldAwards;
        NewAwards = newAwards;
    }
}
