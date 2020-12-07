using System;
using System.Collections.Generic;
using System.Numerics;

public class DesignClasses : ConcreteTool
{
    public DesignClasses(int level, Entity player) : base("Design Classes", level, player)
    {
    }

    public DesignClasses(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Design Classes", level, player, nextUpgrades, upgrades, numberOfUpgrades)
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
        return FormatDescription("Increases effectiveness of Design by <TOOL_VALUE>%");
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
        DesignClasses tool = new DesignClasses(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.DESIGN_CLASSES;
    }

    protected override void SetupUpgrades()
    {
        SetupUpgrade(new Craftsman(1, _entity, this));
        SetupUpgrade(new Craftsman(2, _entity, this));
        SetupUpgrade(new Craftsman(3, _entity, this));
        SetupUpgrade(new Craftsman(4, _entity, this));
        SetupUpgrade(new Craftsman(5, _entity, this));
        SetupUpgrade(new Craftsman(6, _entity, this));
        SetupUpgrade(new Craftsman(7, _entity, this));
        SetupUpgrade(new Craftsman(8, _entity, this));
        SetupUpgrade(new Craftsman(9, _entity, this));
        SetupUpgrade(new Craftsman(10, _entity, this));
    }
}
