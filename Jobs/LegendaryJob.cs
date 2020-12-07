using System;
using UnityEngine;

public class LegendaryJob : Job
{
    protected const int LEGENDARY_JOB_MAX_PROGRESS = 25;
    protected const int LEGENDARY_JOB_EXP_MODIFIER = 25;
    protected const int LEGENDARY_JOB_MONEY_MODIFIER = 20;

    public LegendaryJob(Player player, string name, ulong level, FocusArea[] focus) : base(player, name, level, focus)
    {
    }

    public override System.Numerics.BigInteger GetExperienceReward()
    {
        BigRational experienceReward = new BigRational(LEGENDARY_JOB_EXP_MODIFIER);
        experienceReward = BigRational.Multiply(experienceReward, new BigRational(_level));
        experienceReward *= new BigRational(Math.Pow(_level, 1.25));
        return experienceReward.WholePart;
    }
    public override BigRational GetMoneyReward()
    {
        BigRational moneyReward = new BigRational(LEGENDARY_JOB_MONEY_MODIFIER);
        moneyReward = BigRational.Multiply(moneyReward, new BigRational(_level));
        moneyReward *= new BigRational(Math.Pow(_level, 1));
        return moneyReward;
    }

    public override BigRational GetMaxProgress()
    {
        if (_maxProgress == BigRational.Zero)
        {
            BigRational maxProgress = new BigRational(LEGENDARY_JOB_MAX_PROGRESS);
            maxProgress = BigRational.Multiply(maxProgress, new BigRational(_level));
            maxProgress *= BigRational.Pow(new BigRational(_level + 2), 1);
            _maxProgress = maxProgress;
        }
        return _maxProgress;
    }
}
