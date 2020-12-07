using UnityEngine.UI;

public class UpgradesPanel : ToolsPanel
{
    public GameStudio studio;

    protected override void OnStart()
    {
        AddAvailableTool(new EngineeringPractice(1, player));
        AddAvailableTool(new DesignClasses(1, player));
        AddAvailableTool(new SoundStudio(1, player));
        AddAvailableTool(new Miner(1, player));
        AddAvailableTool(new Architect(1, player, new Entity[] { studio }));
        AddAvailableTool(new Carpenter(1, player, new Entity[] { studio }));
        AddAvailableTool(new Audiologist(1, player, new Entity[] { studio }));
    }
}
