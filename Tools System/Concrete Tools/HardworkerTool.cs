using System;
using System.Collections.Generic;
using System.Numerics;

public class HardworkerTool : ConcreteTool
{
    public HardworkerTool(int level, Entity player) : base("Hardworker", level, player)
    {
    }

    public HardworkerTool(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Hardworker", level, player, nextUpgrades, upgrades, numberOfUpgrades)
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
        return FormatDescription("Add <TOOL_VALUE> progress per second to the fourth active job");
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            Tool promotion = _entity.Tools.GetToolUpgrade(ToolID.HARDWORKER, ToolID.PROMOTION);
            BigRational newModifier = new BigRational(2) * new BigRational(Level) * new BigRational(Math.Pow(Level, 0.20));
            if (promotion != null)
            {
                newModifier *= promotion.GetModifier();
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
        return upgradeTool.GetModifier() >= new BigRational(4);
    }

    public override object Clone()
    {
        HardworkerTool tool = new HardworkerTool(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.HARDWORKER;
    }

    protected override void SetupUpgrades()
    {
        SetupUpgrade(new PromotionUpgrade(1, _entity, this));
        SetupUpgrade(new PromotionUpgrade(2, _entity, this));
        SetupUpgrade(new PromotionUpgrade(3, _entity, this));
        SetupUpgrade(new PromotionUpgrade(4, _entity, this));
        SetupUpgrade(new PromotionUpgrade(5, _entity, this));
        SetupUpgrade(new PromotionUpgrade(6, _entity, this));
        SetupUpgrade(new PromotionUpgrade(7, _entity, this));
        SetupUpgrade(new PromotionUpgrade(8, _entity, this));
        SetupUpgrade(new PromotionUpgrade(9, _entity, this));
        SetupUpgrade(new PromotionUpgrade(10, _entity, this));
    }
}
