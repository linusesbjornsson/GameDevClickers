using System;

public class Job : IRewardProvider
{
    protected Player _player;
    protected string _name;
    protected ulong _level;
    protected FocusArea[] _focus;
    protected BigRational _progress = BigRational.Zero;
    protected BigRational _maxProgress = BigRational.Zero;

    protected const int JOB_EXP_MODIFIER = 10;
    protected const int JOB_MONEY_MODIFIER = 5;
    protected const int JOB_MAX_PROGRESS = 10;

    public Job(Player player, string name, ulong level, FocusArea[] focus)
    {
        _player = player;
        _name = name;
        _level = level;
        _focus = focus;
    }

    public virtual string GetName()
    {
        return _name;
    }

    public ulong GetLevel()
    {
        return _level;
    }

    public virtual System.Numerics.BigInteger GetExperienceReward()
    {
        Tool quickLearner = _player.Tools.GetTool(Tool.ToolID.QUICK_LEARNER);
        BigRational experienceReward = new BigRational(JOB_EXP_MODIFIER);
        experienceReward = BigRational.Multiply(experienceReward, new BigRational(_level));
        experienceReward *= new BigRational(Math.Pow(_level, 0.5));
        if (quickLearner != null)
        {
            experienceReward *= new BigRational(1) + quickLearner.GetModifier();
        }
        return experienceReward.WholePart;
    }

    public virtual BigRational GetMoneyReward()
    {
        Tool negotiator = _player.Tools.GetTool(Tool.ToolID.NEGOTIATOR);
        BigRational moneyReward = new BigRational(JOB_MONEY_MODIFIER);
        moneyReward = BigRational.Multiply(moneyReward, new BigRational(_level));
        moneyReward *= new BigRational(Math.Pow(_level, 0.75));
        if (negotiator != null)
        {
            moneyReward *= new BigRational(1) + negotiator.GetModifier();
        }
        return moneyReward;
    }

    public virtual BigRational GetAwardReward()
    {
        return BigRational.Zero;
    }

    public virtual BigRational GetMaxProgress()
    {
        if (_maxProgress == BigRational.Zero)
        {
            BigRational maxProgress = new BigRational(JOB_MAX_PROGRESS);
            _maxProgress = maxProgress * new BigRational(_level) * new BigRational(Math.Pow(_level, 0.75));
        }
        return _maxProgress;
    }

    public virtual BigRational GetProgress()
    {
        return _progress;
    }

    protected virtual BigRational FocusModifierValue(FocusArea area, BigRational value)
    {
        BigRational returnValue;
        returnValue = value * new BigRational(area.Magnitude);
        returnValue *= value * new BigRational(0.05f) * _player.GetFocusModifier(area);
        returnValue += new BigRational(1);
        return returnValue;
    }

    public BigRational GetFocusModifier(FocusArea.FocusPoint area)
    {
        foreach (FocusArea focus in _focus)
        {
            if (area == focus.Point)
            {
                return GetFocusModifier(focus);
            }
        }
        return BigRational.Zero;
    }

    public BigRational GetFocusModifier(FocusArea area)
    {
        BigRational value = BigRational.Zero;
        switch (area.Point)
        {
            case FocusArea.FocusPoint.TECHNOLOGY:
                value = FocusModifierValue(area, _player.Technology);
                break;
            case FocusArea.FocusPoint.DESIGN:
                value = FocusModifierValue(area, _player.Design);
                break;
            case FocusArea.FocusPoint.AUDIO:
                value = FocusModifierValue(area, _player.Audio);
                break;
            default:
                break;
        }
        return value;
    }

    public bool IsFinished()
    {
        return _progress >= GetMaxProgress();
    }

    public void IncreaseProgress(BigRational progress, bool idle)
    {
        _progress += progress;
        OnJobProgressIncreased?.Invoke(this, new JobProgressArgs(progress, idle));
        if (IsFinished())
        {
            OnJobFinished?.Invoke(this, new JobArgs(this));
        }
    }

    public void SetProgress(BigRational progress, bool idle)
    {
        _progress = progress;
        OnJobProgressIncreased?.Invoke(this, new JobProgressArgs(progress, idle));
        if (IsFinished())
        {
            OnJobFinished?.Invoke(this, new JobArgs(this));
        }
    }

    // Events
    public event EventHandler<JobArgs> OnJobFinished;
    public event EventHandler<JobProgressArgs> OnJobProgressIncreased;
    public class FocusArea
    {
        public FocusPoint Point { get; private set; }
        public float Magnitude { get; private set; }

        public FocusArea(FocusPoint point, float magnitude)
        {
            Point = point;
            Magnitude = magnitude;
        }

        public enum FocusPoint
        {
            TECHNOLOGY,
            DESIGN,
            AUDIO
        }
    }
}
