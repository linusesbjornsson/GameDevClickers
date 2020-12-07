
using System;
using System.Numerics;

public class GameStudioType : GameStudioElementBase
{
    private string _name;
    private float _multiplier;
    private int _defaultProgress;

    private const int BASE_COST = 15000;
    private const int BASE_EXP = 100;

    public GameStudioType(GameStudio studio, string name, float multiplier) : base(studio)
    {
        _name = name;
        _multiplier = multiplier;
    }

    public override BigRational GetAwardReward()
    {
        if (_studio.Level >= 25)
        {
            return new BigRational(2);
        }
        return BigRational.Zero;
    }

    public override BigRational GetCost()
    {
        return new BigRational(BASE_COST) * new BigRational(Level) * new BigRational(Math.Pow(_studio.Level, 0.5)) * new BigRational(_multiplier);
    }

    public override BigInteger GetExperienceReward()
    {
        return (new BigRational(BASE_EXP) * new BigRational(Level) * new BigRational(Math.Pow(_studio.Level, 0.5)) * new BigRational(_multiplier)).WholePart;
    }

    public override BigRational GetMoneyReward()
    {
        return BigRational.Zero;
    }

    public override float GetMultiplier()
    {
        return _multiplier;
    }

    public override string GetName()
    {
        return _name;
    }

    public override GameProject.ProjectElementType GetProjectType()
    {
        return GameProject.ProjectElementType.TYPE;
    }

    public BigRational GetMaxProgress()
    {
        return new BigRational(_defaultProgress);
    }
}
