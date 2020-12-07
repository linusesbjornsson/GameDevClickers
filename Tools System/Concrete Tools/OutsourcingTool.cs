using System;
using System.Collections.Generic;
using System.Numerics;

public class OutsourcingTool : ConcreteTool
{
    public OutsourcingTool(int level, Entity player) : base("Outsourcing", level, player)
    {
    }

    public OutsourcingTool(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Outsourcing", level, player, nextUpgrades, upgrades, numberOfUpgrades)
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
        return FormatDescription("Add <TOOL_VALUE> progress per second to the third active job");
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            Tool taskDelegation = _entity.Tools.GetToolUpgrade(ToolID.OUTSOURCING, ToolID.TASK_DELEGATION);
            BigRational newModifier = new BigRational(2) * new BigRational(Level) * new BigRational(Math.Pow(Level, 0.20));
            if (taskDelegation != null)
            {
                newModifier *= taskDelegation.GetModifier();
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
        return upgradeTool.GetModifier() >= new BigRational(3);
    }

    public override object Clone()
    {
        OutsourcingTool tool = new OutsourcingTool(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.OUTSOURCING;
    }

    protected override void SetupUpgrades()
    {
        SetupUpgrade(new TaskDelegationUpgrade(1, _entity, this));
        SetupUpgrade(new TaskDelegationUpgrade(2, _entity, this));
        SetupUpgrade(new TaskDelegationUpgrade(3, _entity, this));
        SetupUpgrade(new TaskDelegationUpgrade(4, _entity, this));
        SetupUpgrade(new TaskDelegationUpgrade(5, _entity, this));
        SetupUpgrade(new TaskDelegationUpgrade(6, _entity, this));
        SetupUpgrade(new TaskDelegationUpgrade(7, _entity, this));
        SetupUpgrade(new TaskDelegationUpgrade(8, _entity, this));
        SetupUpgrade(new TaskDelegationUpgrade(9, _entity, this));
        SetupUpgrade(new TaskDelegationUpgrade(10, _entity, this));
    }
}
