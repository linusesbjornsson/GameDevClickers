using System;
using System.Numerics;

public class TutoringUpgrade : ToolUpgrade
{
    public TutoringUpgrade(int level, Entity player, Tool connectedTool) : base("Tutoring", level, player, connectedTool)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            _cost = Level * 300;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Increases effectiveness of Online Tutorials by <TOOL_VALUE>%");
    }

    public override string ToolTipModifier()
    {
        BigRational newModifier = BigRational.Subtract(GetModifier(), new BigRational(1));
        return BigRational.Multiply(newModifier, new BigRational(100)).Format();
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            _modifier = new BigRational(2 + (Level - 1));
        }
        return _modifier;
    }

    public override ToolID GetID()
    {
        return ToolID.TUTORING;
    }

    public override int MaxLevel()
    {
        return 10;
    }

    public override object Clone()
    {
        return new TutoringUpgrade(Level, _entity, _connectedTool);
    }
}
