using System;
using System.Collections.Generic;
using System.Numerics;

public class InfrastructureUpgradeTool : ConcreteTool
{
    public InfrastructureUpgradeTool(int level, Entity player) : base("Infrastructure Upgrade", level, player)
    {
    }

    public InfrastructureUpgradeTool(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Infrastructure Upgrade", level, player, nextUpgrades, upgrades, numberOfUpgrades)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            BigInteger cost = new BigInteger(15);
            _cost = cost * Level * new BigRational(Math.Pow(Level, 0.5)).WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Increases the level of new jobs");
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            _modifier = new BigRational(Level);
        }
        return _modifier;
    }

    public override object Clone()
    {
        InfrastructureUpgradeTool tool = new InfrastructureUpgradeTool(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.INFRASTRUCTURE_UPGRADE;
    }

    protected override void SetupUpgrades()
    {
    }
}
