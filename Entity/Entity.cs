using System;
using System.Numerics;
using UnityEngine;

public class Entity : MonoBehaviour, IEntity
{
    protected ulong _level;
    protected BigInteger _experiencePoints;
    protected BigInteger _experienceToLevel;

    protected const int EXPERIENCE_MODIFIER = 100;

    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        Tools = new Tools();
        _level = 1;
        _experiencePoints = 0;
        _experienceToLevel = GetExperienceToLevel();
    }

    public Tools Tools { get; protected set; }

    public ulong Level
    {
        get
        {
            return _level;
        }
        set
        {
            if (value >= 1)
            {
                ulong oldLevel = _level;
                _level = value;
                _experienceToLevel = GetExperienceToLevel();
                OnLevelGained?.Invoke(this, new LevelArgs(_level - 1, _level));
            }
        }
    }
    public BigInteger ExperiencePoints
    {
        get
        {
            return _experiencePoints;
        }
        set
        {
            BigInteger oldExp = _experiencePoints;
            if (value >= _experienceToLevel)
            {
                BigInteger rest = value - _experienceToLevel;
                _experiencePoints = _experienceToLevel;
                OnExperienceGained?.Invoke(this, new ExperienceArgs(oldExp, _experiencePoints));
                Level++;
                if (rest >= 0)
                {
                    _experiencePoints = 0;
                    ExperiencePoints = rest;
                }
                else
                {
                    _experiencePoints = 0;
                }
            }
            else
            {
                _experiencePoints = value;
                OnExperienceGained?.Invoke(this, new ExperienceArgs(oldExp, _experiencePoints));
            }
        }
    }

    public BigInteger ExperienceToLevel
    {
        get
        {
            return _experienceToLevel;
        }
        set
        {
            _experienceToLevel = value;
        }
    }

    protected virtual BigInteger GetExperienceToLevel()
    {
        BigInteger cost = new BigInteger(EXPERIENCE_MODIFIER);
        return cost * Level * new BigRational(Math.Pow(Level + 1, 0.75)).WholePart;
    }

    protected void Initialize(EntityData data, Entity[] externalEntities)
    {
        _level = data.Level;
        _experiencePoints = BigInteger.Parse(data.ExperiencePoints);
        _experienceToLevel = BigInteger.Parse(data.ExperienceToLevel);
        Tools.Initialize(data.Tools, this, externalEntities);
    }

    // Events
    public event EventHandler<ExperienceArgs> OnExperienceGained;
    public event EventHandler<LevelArgs> OnLevelGained;
}
