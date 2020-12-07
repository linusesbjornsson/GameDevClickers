
[System.Serializable]
public class GameStudioGenreData
{
    public ulong Level;
    public string ExperiencePoints;
    public string ExperienceToLevel;

    public GameStudioGenreData(GameStudioGenre genre)
    {
        Level = genre.Level;
        ExperiencePoints = genre.ExperiencePoints.ToString();
        ExperienceToLevel = genre.ExperienceToLevel.ToString();
    }
}
