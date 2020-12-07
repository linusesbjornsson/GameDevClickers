
using System.Numerics;

public class GameStudioFeature : GameStudioElementBase
{
    private string _name;
    private float _multiplier;
    private int _timeDeduction;

    public GameStudioFeature(GameStudio studio, string name, float multiplier, int timeDeduction) : base(studio)
    {
        _name = name;
        _multiplier = multiplier;
        _timeDeduction = timeDeduction;
    }

    public override BigRational GetAwardReward()
    {
        return BigRational.Zero;
    }

    public override BigRational GetCost()
    {
        return BigRational.Zero;
    }

    public override BigInteger GetExperienceReward()
    {
        return BigInteger.Zero;
    }

    public override BigRational GetMoneyReward()
    {
        return BigRational.Zero;
    }

    public override float GetMultiplier()
    {
        return _multiplier;
    }
    
    public int GetTimeDeduction()
    {
        return _timeDeduction;
    }

    public override string GetName()
    {
        return _name;
    }

    public override GameProject.ProjectElementType GetProjectType()
    {
        return GameProject.ProjectElementType.FEATURE;
    }
}
