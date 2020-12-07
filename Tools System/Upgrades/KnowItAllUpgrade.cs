using System;
using System.Numerics;

public class KnowItAllUpgrade : ToolUpgrade
{
    public KnowItAllUpgrade(int level, Entity player, Tool connectedTool) : base("Know-It-All", level, player, connectedTool)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            _cost = Level * 5000 * new BigRational(Math.Pow(Level + 1, 0.5)).WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Increases effectiveness of Expert by <TOOL_VALUE>%");
    }

    public override string ToolTipModifier()
    {
        return (GetModifier() * new BigRational(100)).Format();
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            _modifier = new BigRational(0.15 + (0.15 * (Level - 1)));
        }
        return _modifier;
    }

    public override ToolID GetID()
    {
        return ToolID.KNOW_IT_ALL;
    }

    public override int MaxLevel()
    {
        return 10;
    }

    public override object Clone()
    {
        return new KnowItAllUpgrade(Level, _entity, _connectedTool);
    }
}
