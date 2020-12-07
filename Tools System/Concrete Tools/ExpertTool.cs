using System;
using System.Collections.Generic;
using System.Numerics;

public class ExpertTool : ConcreteTool
{
    public ExpertTool(int level, Entity player) : base("Expert", level, player)
    {
    }

    public ExpertTool(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Expert", level, player, nextUpgrades, upgrades, numberOfUpgrades)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            BigInteger cost = new BigInteger(65);
            _cost = cost * Level * new BigRational(Math.Pow(Level + 1, 0.75)).WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Add <TOOL_VALUE> progress per second to all active jobs");
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            _modifier = new BigRational(2) * new BigRational(Level) * new BigRational(Math.Pow(Level, 0.2));
            Tool knowItAll = _entity.Tools.GetToolUpgrade(ToolID.EXPERT, ToolID.KNOW_IT_ALL);
            if (knowItAll != null)
            {
                _modifier *= knowItAll.GetModifier() + new BigRational(1f);
            }
            ToolUpgrade evolution = _entity.Tools.GetTool(ToolID.TOOL_UPGRADE).GetUpgrade(ToolID.EVOLUTION);
            if (evolution != null)
            {
                _modifier *= new BigRational(1f) + evolution.GetModifier();
            }
        }
        return _modifier;
    }

    public override bool IsUnlocked()
    {
        Tool upgradeTool = _entity.Tools.GetTool(ToolID.TOOL_UPGRADE);
        return upgradeTool.GetModifier() >= new BigRational(4);
    }

    public override object Clone()
    {
        ExpertTool tool = new ExpertTool(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.EXPERT;
    }

    protected override void SetupUpgrades()
    {
        SetupUpgrade(new KnowItAllUpgrade(1, _entity, this));
        SetupUpgrade(new KnowItAllUpgrade(2, _entity, this));
        SetupUpgrade(new KnowItAllUpgrade(3, _entity, this));
        SetupUpgrade(new KnowItAllUpgrade(4, _entity, this));
        SetupUpgrade(new KnowItAllUpgrade(5, _entity, this));
        SetupUpgrade(new KnowItAllUpgrade(6, _entity, this));
        SetupUpgrade(new KnowItAllUpgrade(7, _entity, this));
        SetupUpgrade(new KnowItAllUpgrade(8, _entity, this));
        SetupUpgrade(new KnowItAllUpgrade(9, _entity, this));
        SetupUpgrade(new KnowItAllUpgrade(10, _entity, this));
    }
}
