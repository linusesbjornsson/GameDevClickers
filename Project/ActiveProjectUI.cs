using System;
using UnityEngine;

public class ActiveProjectUI : MonoBehaviour
{
    public Player player;
    public GameStudio studio;

    public void Setup(GameProject project)
    {
        GameProjectUI gameProject = GameProjectUI.Create(transform, player, studio, project);
        gameProject.OnProjectFinished += ProjectFinished;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ProjectFinished(object sender, ProjectArgs args)
    {
        GameProject project = args.Project;
        studio.ExperiencePoints += (new BigRational(project.GetExperienceReward()) * new BigRational(args.Multiplier)).WholePart;
        studio.GetGenre(project.Genre.GetGenre()).ExperiencePoints += (new BigRational(project.GetGenreExperienceReward()) * new BigRational(args.Multiplier)).WholePart;
        player.Awards += project.GetAwardReward() * new BigRational(args.Multiplier);
        OnProjectFinished?.Invoke(sender, args);
    }

    public EventHandler<ProjectArgs> OnProjectFinished;
}
