using System;
using System.Collections.Generic;
using System.Numerics;

public class NegotiatorTool : ConcreteTool
{
    public NegotiatorTool(int level, Entity player) : base("Negotiator", level, player)
    {
    }

    public NegotiatorTool(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Negotiator", level, player, nextUpgrades, upgrades, numberOfUpgrades)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            BigInteger cost = new BigInteger(40);
            _cost = cost * Level * new BigRational(Math.Pow(Level + 1, 0.85)).WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Increase money gained by new jobs by <TOOL_VALUE>%");
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
        return _entity.Level >= 4;
    }

    public override object Clone()
    {
        NegotiatorTool tool = new NegotiatorTool(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.NEGOTIATOR;
    }

    protected override void SetupUpgrades()
    {
    }
}
