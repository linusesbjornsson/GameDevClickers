using System;
using System.Numerics;

public class ImprovedAutomationUpgrade : ToolUpgrade
{
    public ImprovedAutomationUpgrade(int level, Entity player, Tool connectedTool) : base("Improved Automation", level, player, connectedTool)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            BigRational newModifier = new BigRational(300) * new BigRational(Level) * new BigRational(Math.Pow(Level, 2));
            _cost = newModifier.WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Increases effectiveness of Script by <TOOL_VALUE>%");
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
        return ToolID.IMPROVED_AUTOMATION;
    }

    public override int MaxLevel()
    {
        return 10;
    }

    public override object Clone()
    {
        return new ImprovedAutomationUpgrade(Level, _entity, _connectedTool);
    }
}
