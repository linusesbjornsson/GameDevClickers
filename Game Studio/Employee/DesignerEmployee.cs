using System;
using System.Collections.Generic;
using System.Numerics;

public class DesignerEmployee : ExternalConcreteTool
{
    public DesignerEmployee(int level, Entity studio, Entity[] externalEntities) : base("Designer", level, studio, externalEntities)
    {
    }

    public DesignerEmployee(int level, Entity studio, Entity[] externalEntities, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Designer", level, studio, externalEntities, nextUpgrades, upgrades, numberOfUpgrades)
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
        return FormatDescription("The heart of a game. Adds <TOOL_VALUE> design progress per second to your active game project.");
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            BigRational newModifier = new BigRational((double)1) * new BigRational((double)Level) * new BigRational(Math.Pow(Level, 0.20));
            Player player = _externalEntities[0] as Player;
            Tool carpenter = player.Tools.GetTool(ToolID.CARPENTER);
            if (carpenter != null)
            {
                newModifier += player.Design * carpenter.GetModifier();
            }
            _modifier = newModifier;
        }
        return _modifier;
    }

    public override object Clone()
    {
        DesignerEmployee tool = new DesignerEmployee(Level, _entity, _externalEntities, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.DESIGNER;
    }

    protected override void SetupUpgrades()
    {
    }
}