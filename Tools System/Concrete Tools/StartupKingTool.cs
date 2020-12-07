using System;
using System.Collections.Generic;
using System.Numerics;

public class StartupKingTool : ConcreteTool
{
    public StartupKingTool(int level, Entity player) : base("Startup King", level, player)
    {
    }

    public StartupKingTool(int level, Entity player, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base("Startup King", level, player, nextUpgrades, upgrades, numberOfUpgrades)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            _cost = Level * 500;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Increases chance to generate a legendary job by <TOOL_VALUE>%");
    }

    public override ToolID GetID()
    {
        return ToolID.STARTUP_KING;
    }

    public override bool IsUnlocked()
    {
        Tool pairCoder = _entity.Tools.GetTool(ToolID.PAIR_PROGRAMMER);
        return pairCoder != null;
    }

    public override object Clone()
    {
        StartupKingTool tool = new StartupKingTool(Level, _entity, _nextUpgrades, _upgrades, _numberOfUpgrades);
        tool.CloneUpgrades(this);
        return tool;
    }

    public override int MaxLevel()
    {
        return 400;
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            _modifier = new BigRational(1) * new BigRational(Math.Pow(Level, 0.5f));
        }
        return _modifier;
    }

    protected override void SetupUpgrades()
    {
    }
}
