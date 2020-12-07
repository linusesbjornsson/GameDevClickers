using System;
using System.Numerics;

public abstract class Tool : ICloneable
{
    protected string _name;
    protected int _level;
    protected BigRational _modifier = BigRational.Zero;
    protected BigInteger _cost = BigInteger.Zero;
    public int Level
    {
        get
        {
            return _level;
        }
        set
        {
            Invalidate();
            _level = value;
        }
    }
    protected Entity _entity;

    public Tool(string name, int level, Entity entity)
    {
        _name = name;
        _level = level;
        _entity = entity;
    }

    public virtual void Invalidate()
    {
        _modifier = BigRational.Zero;
        _cost = BigInteger.Zero;
    }

    public virtual string GetName()
    {
        return _name;
    }
    public abstract string GetDescription();
    public abstract BigRational GetModifier();
    public abstract BigInteger GetCost();
    public abstract ToolID GetID();
    public virtual bool IsUnlocked()
    {
        return true;
    }
    protected virtual string FormatDescription(string description)
    {
        return description.Replace("<TOOL_VALUE>", ToolTipModifier());
    }
    public virtual string ToolTipModifier()
    {
        return GetModifier().Format();
    }

    public abstract object Clone();

    public bool IsMaxLevel()
    {
        return MaxLevel() != -1 && Level >= MaxLevel() || MaxLevel() == 0;
    }
    // -1: No max level 0: Only one level
    public virtual int MaxLevel()
    {
        return -1;
    }

    public enum ToolID
    {
        INFRASTRUCTURE_UPGRADE,
        SCRIPT,
        ONLINE_TUTORIALS,
        OUTSOURCING,
        HARDWORKER,
        PAIR_PROGRAMMER,
        MINER,
        STARTUP_KING,
        OVERTIME,
        IMPROVED_AUTOMATION,
        TUTORING,
        TASK_DELEGATION,
        PROMOTION,
        GENIUS,
        AUTOMATED_TUTORING,
        SOFTWARE_ENGINEER,
        DESIGNER,
        SOUND_DESIGNER,
        EXPERT,
        QUICK_LEARNER,
        NEGOTIATOR,
        TOOL_UPGRADE,
        ENGINEERING_PRACTICE,
        DESIGN_CLASSES,
        SOUND_STUDIO,
        EVOLUTION,
        TECHNICIAN,
        CRAFTSMAN,
        PRODUCER,
        KNOW_IT_ALL,
        ARCHITECT,
        CARPENTER,
        AUDIOLOGIST
    }
}
