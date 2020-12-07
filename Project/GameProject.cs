using System;
using System.Collections.Generic;
using System.Numerics;

public class GameProject : IRewardProvider
{
    private BigRational _awardReward;
    private BigRational _cost;
    private BigRational _maxProgress;
    private ulong _level;
    private float _featureMultiplier = 1f;
    private int _time;

    public GameStudioType Type { get; private set; }
    public GameStudioGenre Genre { get; private set; }
    public List<GameStudioFeature> Features { get; private set; }

    private readonly int PROJECT_DEFAULT_TIME = 100;
    private readonly int PROJECT_MAX_PROGRESS = 5000;

    public GameProject(ulong level, GameStudioType type, GameStudioGenre genre, List<GameStudioFeature> features)
    {
        _level = 1;
        Type = type;
        Genre = genre;
        Features = features;
        BigRational projectCost = new BigRational(0);
        BigRational projectAwards = new BigRational(0);
        int projectTime = PROJECT_DEFAULT_TIME;
        if (type != null)
        {
            projectCost += type.GetCost();
            projectAwards += type.GetAwardReward();
        }
        if (genre != null)
        {
            projectCost += genre.GetCost();
            projectAwards += genre.GetAwardReward();
        }
        foreach (GameStudioFeature feature in features)
        {
            projectTime -= feature.GetTimeDeduction();
            _featureMultiplier += feature.GetMultiplier() - 1f;
        }
        _awardReward = BigRational.Multiply(projectAwards, new BigRational(_featureMultiplier));
        _maxProgress = new BigRational(PROJECT_MAX_PROGRESS);
        _maxProgress = _maxProgress * new BigRational(_level) * new BigRational(Math.Pow(_level, 1.25));
        _time = projectTime;
        _cost = BigRational.Multiply(projectCost, new BigRational(_featureMultiplier));
    }

    public ulong GetLevel()
    {
        return _level;
    }

    public Job.FocusArea[] GetFocus()
    {
        return new Job.FocusArea[] { new Job.FocusArea(Job.FocusArea.FocusPoint.TECHNOLOGY, 1f), new Job.FocusArea(Job.FocusArea.FocusPoint.DESIGN, 1f), new Job.FocusArea(Job.FocusArea.FocusPoint.AUDIO, 1f) };
    }

    public BigRational GetMaxProgress(Job.FocusArea.FocusPoint focus)
    {
        return _maxProgress;
    }

    public BigInteger GetExperienceReward()
    {
        if (Type != null)
        {
            return BigRational.Multiply(new BigRational(Type.GetExperienceReward()), new BigRational(_featureMultiplier)).WholePart;
        }
        return BigInteger.Zero;
    }

    public BigInteger GetGenreExperienceReward()
    {
        if (Genre != null)
        {
            return BigRational.Multiply(new BigRational(Genre.GetExperienceReward()), new BigRational(_featureMultiplier)).WholePart;
        }
        return BigInteger.Zero;
    }

    public BigRational GetMoneyReward()
    {
        return BigRational.Zero;
    }

    public BigRational GetAwardReward()
    {
        return _awardReward;
    }

    public BigRational GetCost()
    {
        return _cost;
    }

    public int GetTime()
    {
        return _time;
    }

    public enum ProjectElementType
    {
        TYPE,
        GENRE,
        FEATURE
    }
}
