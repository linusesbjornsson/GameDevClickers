
[System.Serializable]
public class GameStudioData
{
    public EntityData EntityData;
    public GameStudioGenreData ActionData;
    public GameStudioGenreData AdventureData;
    public GameStudioGenreData HorrorData;
    public GameStudioGenreData RpgData;
    public bool IsActivated;

    public GameStudioData(GameStudio studio)
    {
        EntityData = new EntityData(studio);
        IsActivated = studio.IsActivated();
        ActionData = new GameStudioGenreData(studio.GetGenre(GameStudioGenre.Genre.ACTION));
        AdventureData = new GameStudioGenreData(studio.GetGenre(GameStudioGenre.Genre.ADVENTURE));
        HorrorData = new GameStudioGenreData(studio.GetGenre(GameStudioGenre.Genre.HORROR));
        RpgData = new GameStudioGenreData(studio.GetGenre(GameStudioGenre.Genre.RPG));
    }
}
