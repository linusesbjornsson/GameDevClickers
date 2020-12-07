[System.Serializable]
public class ToolData
{
    public int Level;
    public Tool.ToolID Id;

    public ToolData(Tool tool)
    {
        Level = tool.Level;
        Id = tool.GetID();
    }
}