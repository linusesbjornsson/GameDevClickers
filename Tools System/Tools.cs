
using System.Collections.Generic;
using static Tool;

public class Tools
{
    private Dictionary<ToolID, ConcreteTool> activeTools = new Dictionary<ToolID, ConcreteTool>();

    public ConcreteTool GetTool(ToolID id)
    {
        activeTools.TryGetValue(id, out ConcreteTool foundTool);
        return foundTool;
    }

    public ToolUpgrade GetToolUpgrade(ToolID id, ToolID upgradeId)
    {
        ConcreteTool tool = GetTool(id);
        if (tool == null)
        {
            return null;
        }
        return tool.GetUpgrade(upgradeId);
    }

    public List<ConcreteTool> GetAllTools()
    {
        List<ConcreteTool> allTools = new List<ConcreteTool>();
        foreach (KeyValuePair<ToolID, ConcreteTool> tool in activeTools)
        {
            allTools.Add(tool.Value);
        }
        return allTools;
    }

    public void AddTool(ConcreteTool tool)
    {
        AddTool(tool.GetID(), tool);
    }

    public void AddTool(ToolID id, ConcreteTool tool)
    {
        if (GetTool(id) != null)
        {
            activeTools[id] = tool;
        }
        else
        {
            activeTools.Add(id, tool);
        }
    }

    public void Initialize(ToolsData tools, Entity entity, Entity[] externalEntities)
    {
        Tools newTools = tools.Convert(entity, externalEntities);
        activeTools = newTools.activeTools;
    }
}
