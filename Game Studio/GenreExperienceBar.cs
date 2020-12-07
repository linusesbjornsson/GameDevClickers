
public class GenreExperienceBar : ExperienceBar
{
    public GameStudio studio;
    public GameStudioGenre.Genre genre;

    public void Start()
    {
        _entity = studio.GetGenre(genre);
        base.OnStart();
    }
}
