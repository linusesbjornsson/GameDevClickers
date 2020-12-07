using System;
using System.Collections.Generic;
using System.Numerics;

public class QuickLearnerTool : ConcreteTool
{
    public QuickLearnerTool(int level, Entity player) : base("Quick Learner", level, player)
    {
    }

    public QuickLearnerTool(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Quick Learner", level, player, nextUpgrades, upgrades, numberOfUpgrades)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            BigInteger cost = new BigInteger(40);
            _cost = cost * Level * new BigRational(Math.Pow(Level + 1, 0.6)).WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Increase experience gained by new jobs by <TOOL_VALUE>%");
    }

    public override string ToolTipModifier()
    {
        return (GetModifier() * new BigRational(100)).Format();
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            BigRational newModifier = new BigRational(5) * new BigRational(Math.Pow(Level, 0.85));
            _modifier = newModifier / new BigRational(100);
        }
        return _modifier;
    }

    public override bool IsUnlocked()
    {
        return _entity.Level >= 2;
    }

    public override object Clone()
    {
        QuickLearnerTool tool = new QuickLearnerTool(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.QUICK_LEARNER;
    }

    protected override void SetupUpgrades()
    {
    }
}
