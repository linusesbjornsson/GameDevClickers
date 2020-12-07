using System;
using System.Collections.Generic;
using System.Numerics;

public class ProjectManagementTechnology : ConcreteTool
{
    public ProjectManagementTechnology(int level, Entity player) : base("Project Management: Technology", level, player)
    {
    }

    public ProjectManagementTechnology(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Project Management: Technology", level, player, nextUpgrades, upgrades, numberOfUpgrades)
    {
    }

    public override BigInteger GetCost()
    {
        BigInteger cost = new BigInteger(10);
        return cost * Level * new BigRational(Math.Pow(Level + 1, 0.75)).WholePart;
    }

    public override string GetDescription()
    {
        return FormatDescription("Add <TOOL_VALUE> progress per second to the second active job");
    }

    public override BigRational GetModifier()
    {
        Tool tutoring = _entity.Tools.GetToolUpgrade(ToolID.ONLINE_TUTORIALS, ToolID.TUTORING);
        BigRational newModifier = new BigRational(1) * new BigRational(Level) * new BigRational(Math.Pow(Level, 0.20));
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
        return newModifier;
    }

    public override bool IsUnlocked()
    {
        return false;
    }

    public override object Clone()
    {
        ProjectManagementTechnology tool = new ProjectManagementTechnology(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
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
