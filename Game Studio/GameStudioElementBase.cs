
using System;
using System.Numerics;

public abstract class GameStudioElementBase : IGameStudioElement, IEntity
{
    protected GameStudio _studio;
    protected ulong _level;
    protected BigInteger _experiencePoints;
    protected BigInteger _experienceToLevel;

    protected const int EXPERIENCE_MODIFIER = 100;

    public GameStudioElementBase(GameStudio studio)
    {
        _studio = studio;
        _level = 1;
        _experiencePoints = 0;
        _experienceToLevel = GetExperienceToLevel();
    }

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
        BigRational experienceReward = new BigRational(EXPERIENCE_MODIFIER);
        experienceReward *= new BigRational(Level);
        experienceReward *= new BigRational(Math.Pow(Level, 0.5f));
        return experienceReward.WholePart;
    }

    public abstract string GetName();
    public abstract float GetMultiplier();
    public abstract BigRational GetCost();

    public abstract BigInteger GetExperienceReward();
    public abstract BigRational GetMoneyReward();
    public abstract BigRational GetAwardReward();
    public abstract GameProject.ProjectElementType GetProjectType();

    // Events
    public event EventHandler<ExperienceArgs> OnExperienceGained;
    public event EventHandler<LevelArgs> OnLevelGained;
}


