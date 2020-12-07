using System;
using System.Collections.Generic;
using System.Numerics;

public class Miner : ConcreteTool
{
    public Miner(int level, Entity player) : base("Miner", level, player)
    {
    }

    public Miner(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Miner", level, player, nextUpgrades, upgrades, numberOfUpgrades)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            BigInteger cost = new BigInteger(250);
            _cost = cost * Level * new BigRational(Math.Pow(Level + 1, 1)).WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Generate <TOOL_VALUE> money per second");
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            _modifier = new BigRational(1) * new BigRational(Level) * new BigRational(Math.Pow(Level, 0.75));
        }
        return _modifier;
    }

    public override int MaxLevel()
    {
        return 100;
    }

    public override object Clone()
    {
        Miner tool = new Miner(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.MINER;
    }

    protected override void SetupUpgrades()
    {
    }
}
