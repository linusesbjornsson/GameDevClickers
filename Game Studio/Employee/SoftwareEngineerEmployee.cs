using System;
using System.Collections.Generic;
using System.Numerics;

public class SoftwareEngineerEmployee : ExternalConcreteTool
{
    public SoftwareEngineerEmployee(int level, Entity studio, Entity[] externalEntities) : base("Software Engineer", level, studio, externalEntities)
    {
    }

    public SoftwareEngineerEmployee(int level, Entity studio, Entity[] externalEntities, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Software Engineer", level, studio, externalEntities, nextUpgrades, upgrades, numberOfUpgrades)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            BigInteger cost = new BigInteger(100);
            _cost = cost * Level * new BigRational(Math.Pow(Level + 1, 0.75)).WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Turns coffee into code. Adds <TOOL_VALUE> technology progress per second to your active game project.");
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            BigRational newModifier = new BigRational((double)1) * new BigRational((double)Level) * new BigRational(Math.Pow(Level, 0.20));
            Player player = _externalEntities[0] as Player;
            Tool architect = player.Tools.GetTool(ToolID.ARCHITECT);
            if (architect != null)
            {
                newModifier += player.Technology * architect.GetModifier();
            }
            _modifier = newModifier;
        }
        return _modifier;
    }

    public override object Clone()
    {
        SoftwareEngineerEmployee tool = new SoftwareEngineerEmployee(Level, _entity, _externalEntities, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.SOFTWARE_ENGINEER;
    }

    protected override void SetupUpgrades()
    {
    }
}