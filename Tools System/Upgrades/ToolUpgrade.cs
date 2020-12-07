
public abstract class ToolUpgrade : Tool
{
    protected Tool _connectedTool;

    public ToolUpgrade(string name, int level, Entity player, Tool connectedTool) : base(name, level, player)
    {
        _connectedTool = connectedTool;
    }

    public void SetConnectedTool(Tool tool)
    {
        _connectedTool = tool;
    }

    public override string GetName()
    {
        return base.GetName() + " " + Roman.To(Level);
    }
}
