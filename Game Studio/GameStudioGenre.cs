using System;
using System.Numerics;
using static Job;

public class GameStudioGenre : GameStudioElementBase
{
    private Genre _genre;
    private FocusArea[] _focus;

    private const int BASE_COST = 7500;
    private const int BASE_EXP = 25;

    public GameStudioGenre(GameStudio studio, Genre genre, FocusArea[] focus) : base(studio)
    {
        _genre = genre;
        _focus = focus;
    }

    public override BigRational GetAwardReward()
    {
        return BigRational.Zero;
    }

    public override BigRational GetCost()
    {
        return new BigRational(BASE_COST) * new BigRational(_level) * new BigRational(Math.Pow(_level, 0.5));
    }

    public override BigInteger GetExperienceReward()
    {
        return (new BigRational(BASE_EXP) * new BigRational(_level) * new BigRational(Math.Pow(_level, 0.5))).WholePart;
    }

    public FocusArea[] GetFocusArea()
    {
        return _focus;
    }

    public Genre GetGenre()
    {
        return _genre;
    }

    public override BigRational GetMoneyReward()
    {
        return BigRational.Zero;
    }

    public override float GetMultiplier()
    {
        return 0f;
    }

    public override string GetName()
    {
        return _genre.ToString().Substring(0, 1) + _genre.ToString().Substring(1).ToString().ToLower();
    }

    public override GameProject.ProjectElementType GetProjectType()
    {
        return GameProject.ProjectElementType.GENRE;
    }

    public void Initialize(GameStudioGenreData data)
    {
        Level = data.Level;
        ExperiencePoints = BigInteger.Parse(data.ExperiencePoints);
        ExperienceToLevel = BigInteger.Parse(data.ExperienceToLevel);
    }

    public enum Genre
    {
        RPG,
        ACTION,
        ADVENTURE,
        HORROR
    }
}
