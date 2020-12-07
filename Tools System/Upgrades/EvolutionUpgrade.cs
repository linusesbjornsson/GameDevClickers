using System;
using System.Numerics;

public class EvolutionUpgrade : ToolUpgrade
{
    public EvolutionUpgrade(int level, Entity player, Tool connectedTool) : base("Evolution", level, player, connectedTool)
    {
    }

    public override BigInteger GetCost()
    {
        if (_cost == BigInteger.Zero)
        {
            _cost = Level * 5000 * new BigRational(Math.Pow(Level + 1, 5)).WholePart;
        }
        return _cost;
    }

    public override string GetDescription()
    {
        return FormatDescription("Increased effectiveness of all jobs by <TOOL_VALUE>%");
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
            _modifier = new BigRational(1f + (Level * 0.25f));
        }
        return _modifier;
    }

    public override ToolID GetID()
    {
        return ToolID.EVOLUTION;
    }

    public override int MaxLevel()
    {
        return 5;
    }

    public override object Clone()
    {
        return new EvolutionUpgrade(Level, _entity, _connectedTool);
    }
}
