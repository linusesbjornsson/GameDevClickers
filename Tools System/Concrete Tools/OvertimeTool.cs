using System;
using System.Collections.Generic;
using System.Numerics;

public class OvertimeTool : ConcreteTool
{
    public OvertimeTool(int level, Entity player) : base("Overtime", level, player)
    {
    }

    public OvertimeTool(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Overtime", level, player, nextUpgrades, upgrades, numberOfUpgrades)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            BigInteger cost = new BigInteger(10);
            _cost = cost * Level * new BigRational(Math.Pow(Level + 1, 0.75)).WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Every 60 seconds, there is a <TOOL_VALUE>% chance to instantly complete an active job");
    }

    public override int MaxLevel()
    {
        return 100;
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            _modifier = new BigRational(1) * new BigRational(Math.Pow(Level, 0.35f));
        }
        return _modifier;
    }

    public override bool IsUnlocked()
    {
        Tool pairCoder = _entity.Tools.GetTool(ToolID.PAIR_PROGRAMMER);
        return pairCoder != null;
    }

    public override object Clone()
    {
        OvertimeTool tool = new OvertimeTool(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.OVERTIME;
    }

    protected override void SetupUpgrades()
    {
    }
}
