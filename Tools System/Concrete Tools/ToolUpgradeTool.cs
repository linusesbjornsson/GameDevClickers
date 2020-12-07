using System;
using System.Collections.Generic;
using System.Numerics;

public class ToolUpgradeTool : ConcreteTool
{
    public ToolUpgradeTool(int level, Entity player) : base("Tool Upgrade", level, player)
    {
    }

    public ToolUpgradeTool(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Tool Upgrade", level, player, nextUpgrades, upgrades, numberOfUpgrades)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            BigInteger cost = new BigInteger(10);
            _cost = (Level - 1) * 250 * new BigRational(Math.Pow(Level, 0.75)).WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Increases the number of job slots to <TOOL_VALUE>");
    }

    public override ToolID GetID()
    {
        return ToolID.TOOL_UPGRADE;
    }

    public override object Clone()
    {
        ToolUpgradeTool tool = new ToolUpgradeTool(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override int MaxLevel()
    {
        return 5;
    }

    public override BigRational GetModifier()
    {
        return new BigRational((double) 1) * new BigRational((double) Level);
    }

    protected override void SetupUpgrades()
    {
        SetupUpgrade(new EvolutionUpgrade(1, _entity, this));
        SetupUpgrade(new EvolutionUpgrade(2, _entity, this));
        SetupUpgrade(new EvolutionUpgrade(3, _entity, this));
        SetupUpgrade(new EvolutionUpgrade(4, _entity, this));
        SetupUpgrade(new EvolutionUpgrade(5, _entity, this));
    }

    protected override uint[] LevelRange()
    {
        return new uint[] { 5 };
    }
}
