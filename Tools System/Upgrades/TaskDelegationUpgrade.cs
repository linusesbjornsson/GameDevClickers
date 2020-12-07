using System;
using System.Numerics;

public class TaskDelegationUpgrade : ToolUpgrade
{
    public TaskDelegationUpgrade(int level, Entity player, Tool connectedTool) : base("Task Delegation", level, player, connectedTool)
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
        return FormatDescription("Increases effectiveness of Outsourcing by <TOOL_VALUE>%");
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
        return ToolID.TASK_DELEGATION;
    }

    public override int MaxLevel()
    {
        return 10;
    }

    public override object Clone()
    {
        return new TaskDelegationUpgrade(Level, _entity, _connectedTool);
    }
}
