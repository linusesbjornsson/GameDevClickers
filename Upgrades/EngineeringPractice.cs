using System;
using System.Collections.Generic;
using System.Numerics;

public class EngineeringPractice : ConcreteTool
{
    public EngineeringPractice(int level, Entity player) : base("Engineering Practice", level, player)
    {
    }

    public EngineeringPractice(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Engineering Practice", level, player, nextUpgrades, upgrades, numberOfUpgrades)
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
        return FormatDescription("Increases effectiveness of Technology by <TOOL_VALUE>%");
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
        EngineeringPractice tool = new EngineeringPractice(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.ENGINEERING_PRACTICE;
    }

    protected override void SetupUpgrades()
    {
        SetupUpgrade(new Technician(1, _entity, this));
        SetupUpgrade(new Technician(2, _entity, this));
        SetupUpgrade(new Technician(3, _entity, this));
        SetupUpgrade(new Technician(4, _entity, this));
        SetupUpgrade(new Technician(5, _entity, this));
        SetupUpgrade(new Technician(6, _entity, this));
        SetupUpgrade(new Technician(7, _entity, this));
        SetupUpgrade(new Technician(8, _entity, this));
        SetupUpgrade(new Technician(9, _entity, this));
        SetupUpgrade(new Technician(10, _entity, this));
    }
}
