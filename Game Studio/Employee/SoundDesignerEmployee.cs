using System;
using System.Collections.Generic;
using System.Numerics;

public class SoundDesignerEmployee : ExternalConcreteTool
{
    public SoundDesignerEmployee(int level, Entity studio, Entity[] externalEntities) : base("Sound Designer", level, studio, externalEntities)
    {
    }

    public SoundDesignerEmployee(int level, Entity studio, Entity[] externalEntities, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Sound Designer", level, studio, externalEntities, nextUpgrades, upgrades, numberOfUpgrades)
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
        return FormatDescription("Sees the world in decibels. Adds <TOOL_VALUE> audio progress per second to your active game project.");
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            BigRational newModifier = new BigRational((double)1) * new BigRational((double)Level) * new BigRational(Math.Pow(Level, 0.20));
            Player player = _externalEntities[0] as Player;
            Tool audiologist = player.Tools.GetTool(ToolID.AUDIOLOGIST);
            if (audiologist != null)
            {
                newModifier += player.Audio * audiologist.GetModifier();
            }
            _modifier = newModifier;
        }
        return _modifier;
    }

    public override object Clone()
    {
        SoundDesignerEmployee tool = new SoundDesignerEmployee(Level, _entity, _externalEntities, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.SOUND_DESIGNER;
    }

    protected override void SetupUpgrades()
    {
    }
}