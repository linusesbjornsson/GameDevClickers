
[System.Serializable]
public class EntityData
{
    public ulong Level;
    public string ExperiencePoints;
    public string ExperienceToLevel;
    public ToolsData Tools;

    public EntityData(Entity entity)
    {
        Tools = new ToolsData(entity.Tools);
        Level = entity.Level;
        ExperiencePoints = entity.ExperiencePoints.ToString();
        ExperienceToLevel = entity.ExperienceToLevel.ToString();
    }
}