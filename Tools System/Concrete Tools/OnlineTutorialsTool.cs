using System;
using System.Collections.Generic;
using System.Numerics;

public class OnlineTutorialsTool : ConcreteTool
{
    public OnlineTutorialsTool(int level, Entity player) : base("Online Tutorials", level, player)
    {
    }

    public OnlineTutorialsTool(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Online Tutorials", level, player, nextUpgrades, upgrades, numberOfUpgrades)
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
        return FormatDescription("Add <TOOL_VALUE> progress per second to the second active job");
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            Tool tutoring = _entity.Tools.GetToolUpgrade(ToolID.ONLINE_TUTORIALS, ToolID.TUTORING);
            BigRational newModifier = new BigRational(2) * new BigRational(Level) * new BigRational(Math.Pow(Level, 0.20));
            if (tutoring != null)
            {
                newModifier *= tutoring.GetModifier();
            }
            Tool automatedTutoring = _entity.Tools.GetToolUpgrade(ToolID.SCRIPT, ToolID.AUTOMATED_TUTORING);
            if (automatedTutoring != null)
            {
                Tool script = _entity.Tools.GetTool(ToolID.SCRIPT);
                newModifier += script.GetModifier() * automatedTutoring.GetModifier();
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
        return upgradeTool.GetModifier() >= new BigRational(2);
    }

    public override object Clone()
    {
        OnlineTutorialsTool tool = new OnlineTutorialsTool(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.ONLINE_TUTORIALS;
    }

    protected override void SetupUpgrades()
    {
        SetupUpgrade(new TutoringUpgrade(1, _entity, this));
        SetupUpgrade(new TutoringUpgrade(2, _entity, this));
        SetupUpgrade(new TutoringUpgrade(3, _entity, this));
        SetupUpgrade(new TutoringUpgrade(4, _entity, this));
        SetupUpgrade(new TutoringUpgrade(5, _entity, this));
        SetupUpgrade(new TutoringUpgrade(6, _entity, this));
        SetupUpgrade(new TutoringUpgrade(7, _entity, this));
        SetupUpgrade(new TutoringUpgrade(8, _entity, this));
        SetupUpgrade(new TutoringUpgrade(9, _entity, this));
        SetupUpgrade(new TutoringUpgrade(10, _entity, this));
    }
}
