
using System;
using System.Numerics;

public interface IEntity
{
    ulong Level
    {
        get;
        set;
    }
    BigInteger ExperiencePoints
    {
        get;
        set;
    }
    BigInteger ExperienceToLevel
    {
        get;
        set;
    }

    // Events
    event EventHandler<ExperienceArgs> OnExperienceGained;
    event EventHandler<LevelArgs> OnLevelGained;
}