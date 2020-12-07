using System;
using System.Numerics;

public class AutomatedTutoringUpgrade : ToolUpgrade
{
    public AutomatedTutoringUpgrade(int level, Entity player, Tool connectedTool) : base("Automated Tutoring", level, player, connectedTool)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            _cost = Level * 1000;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Online Tutorials gain <TOOL_VALUE>% of Scripts progress");
    }

    public override string ToolTipModifier()
    {
        return BigRational.Multiply(GetModifier(), new BigRational(100)).Format();
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            _modifier = new BigRational(0.05f) * new BigRational(Level);
        }
        return _modifier;
    }

    public override ToolID GetID()
    {
        return ToolID.AUTOMATED_TUTORING;
    }

    public override int MaxLevel()
    {
        return 0;
    }

    public override object Clone()
    {
        return new AutomatedTutoringUpgrade(Level, _entity, _connectedTool);
    }

    public override string GetName()
    {
        return _name;
    }
}
