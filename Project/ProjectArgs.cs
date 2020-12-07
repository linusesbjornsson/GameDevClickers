using System;

public class ProjectArgs : EventArgs
{
    public GameProject Project { get; private set; }
    public float Multiplier { get; private set; }
    public ProjectArgs(GameProject project, float multiplier)
    {
        Project = project;
        Multiplier = multiplier;
    }
}
