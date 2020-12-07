using System;
using System.Collections.Generic;
using System.Numerics;

public class PairProgrammerTool : ConcreteTool
{
    public PairProgrammerTool(int level, Entity player) : base("Pair Programmer", level, player)
    {
    }

    public PairProgrammerTool(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Pair Programmer", level, player, nextUpgrades, upgrades, numberOfUpgrades)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            BigInteger cost = new BigInteger(10);
            _cost = cost * Level * new BigRational(Math.Pow(Level + 1, 0.75)).WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Add <TOOL_VALUE> progress per second to the fifth active job");
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            Tool genius = _entity.Tools.GetToolUpgrade(ToolID.PAIR_PROGRAMMER, ToolID.GENIUS);
            BigRational newModifier = new BigRational(2) * new BigRational(Level) * new BigRational(Math.Pow(Level, 0.20));
            if (genius != null)
            {
                newModifier *= genius.GetModifier();
            }
            ToolUpgrade evolution = _entity.Tools.GetTool(ToolID.TOOL_UPGRADE).GetUpgrade(ToolID.EVOLUTION);
            if (evolution != null)
            {
                newModifier *= new BigRational(1f) + evolution.GetModifier();
            }
            _modifier = newModifier;
        }
        return _modifier;
    }

    public override bool IsUnlocked()
    {
        Tool upgradeTool = _entity.Tools.GetTool(ToolID.TOOL_UPGRADE);
        return upgradeTool.GetModifier() >= new BigRational(5);
    }

    public override object Clone()
    {
        PairProgrammerTool tool = new PairProgrammerTool(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.PAIR_PROGRAMMER;
    }

    protected override void SetupUpgrades()
    {
        SetupUpgrade(new GeniusUpgrade(1, _entity, this));
        SetupUpgrade(new GeniusUpgrade(2, _entity, this));
        SetupUpgrade(new GeniusUpgrade(3, _entity, this));
        SetupUpgrade(new GeniusUpgrade(4, _entity, this));
        SetupUpgrade(new GeniusUpgrade(5, _entity, this));
        SetupUpgrade(new GeniusUpgrade(6, _entity, this));
        SetupUpgrade(new GeniusUpgrade(7, _entity, this));
        SetupUpgrade(new GeniusUpgrade(8, _entity, this));
        SetupUpgrade(new GeniusUpgrade(9, _entity, this));
        SetupUpgrade(new GeniusUpgrade(10, _entity, this));
    }
}
