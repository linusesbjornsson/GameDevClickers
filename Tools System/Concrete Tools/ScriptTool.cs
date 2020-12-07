using System;
using System.Collections.Generic;
using System.Numerics;

public class ScriptTool : ConcreteTool
{
    public ScriptTool(int level, Entity player) : base("Script", level, player)
    {
    }

    public ScriptTool(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Script", level, player, nextUpgrades, upgrades, numberOfUpgrades)
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
        return FormatDescription("Add <TOOL_VALUE> progress per second to the first active job");
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            ToolUpgrade automation = _entity.Tools.GetToolUpgrade(ToolID.SCRIPT, ToolID.IMPROVED_AUTOMATION);
            BigRational newModifier = new BigRational(2) * new BigRational(Level) * new BigRational(Math.Pow(Level, 0.20));
            if (automation != null)
            {
                newModifier *= automation.GetModifier();
            }
            ToolUpgrade evolution = _entity.Tools.GetTool(ToolID.TOOL_UPGRADE).GetUpgrade(ToolID.EVOLUTION);
            if (evolution != null)
            {
                newModifier *= new BigRational(1f) + evolution.GetModifier();
            }
            _modifier =  newModifier;
        }
        return _modifier;
    }

    public override object Clone()
    {
        ScriptTool tool = new ScriptTool(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.SCRIPT;
    }

    protected override void SetupUpgrades()
    {
        SetupUpgrade(new ImprovedAutomationUpgrade(1, _entity, this));
        SetupUpgrade(new ImprovedAutomationUpgrade(2, _entity, this));
        SetupUpgrade(new ImprovedAutomationUpgrade(3, _entity, this));
        SetupUpgrade(new AutomatedTutoringUpgrade(1, _entity, this));
        SetupUpgrade(new ImprovedAutomationUpgrade(4, _entity, this));
        SetupUpgrade(new ImprovedAutomationUpgrade(5, _entity, this));
        SetupUpgrade(new ImprovedAutomationUpgrade(6, _entity, this));
        SetupUpgrade(new ImprovedAutomationUpgrade(7, _entity, this));
        SetupUpgrade(new ImprovedAutomationUpgrade(8, _entity, this));
        SetupUpgrade(new ImprovedAutomationUpgrade(9, _entity, this));
        SetupUpgrade(new ImprovedAutomationUpgrade(10, _entity, this));
    }
}
