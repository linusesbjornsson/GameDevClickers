using System;
using System.Collections.Generic;
using System.Numerics;

public class Architect : ExternalConcreteTool
{
    public Architect(int level, Entity player, Entity[] externalEntities) : base("Architect", level, player, externalEntities)
    {
    }

    public Architect(int level, Entity player, Entity[] externalEntities, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Architect", level, player, externalEntities, nextUpgrades, upgrades, numberOfUpgrades)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            BigInteger cost = new BigInteger(100);
            _cost = cost * Level * new BigRational(Math.Pow(Level + 1, 1.25)).WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Increases effectiveness of Software Engineers by <TOOL_VALUE>% of Technology");
    }

    public override string ToolTipModifier()
    {
        return BigRational.Multiply(GetModifier(), new BigRational(100)).Format();
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            _modifier = new BigRational(0.02f) * new BigRational(Level);
        }
        return _modifier;
    }

    public override object Clone()
    {
        Architect tool = new Architect(Level, _entity, _externalEntities, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override bool IsUnlocked()
    {
        GameStudio studio = _externalEntities[0] as GameStudio;
        return studio.IsActivated();
    }

    public override ToolID GetID()
    {
        return ToolID.ARCHITECT;
    }

    protected override void SetupUpgrades()
    {
    }
}
