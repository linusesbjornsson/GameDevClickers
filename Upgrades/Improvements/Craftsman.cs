using System;
using System.Collections.Generic;
using System.Numerics;

public class Craftsman : ToolUpgrade
{
    public Craftsman(int level, Entity player, Tool connectedTool) : base("Craftsman", level, player, connectedTool)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            BigInteger cost = new BigInteger(100);
            _cost = cost * Level * new BigRational(Math.Pow(Level + 1, 1.25)).WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Adds <TOOL_VALUE>% of your total Design to the progress of all active jobs");
    }
    public override string ToolTipModifier()
    {
        return BigRational.Multiply(GetModifier(), new BigRational(100)).Format();
    }

    public override int MaxLevel()
    {
        return 10;
    }

    public override BigRational GetModifier()
    {
        if (_modifier == BigRational.Zero)
        {
            _modifier = new BigRational(0.10f) * new BigRational(Math.Pow(Level, 0.75f));
        }
        return _modifier;
    }

    public override object Clone()
    {
        Craftsman tool = new Craftsman(Level, _entity, _connectedTool);
        return tool;
    }

    public override ToolID GetID()
    {
        return ToolID.CRAFTSMAN;
    }
}