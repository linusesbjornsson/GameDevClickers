using System;
using System.Numerics;

public class MoneyArgs : EventArgs
{
    public BigRational OldMoney { get; private set; }
    public BigRational NewMoney { get; private set; }
    public MoneyArgs(BigRational oldMoney, BigRational newMoney)
    {
        OldMoney = oldMoney;
        NewMoney = newMoney;
    }
}
