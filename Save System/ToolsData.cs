using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToolsData
{
    [SerializeField]
    public List<ConcreteToolData> activeTools = new List<ConcreteToolData>();

    public ToolsData(Tools tools)
    {
        foreach (ConcreteTool tool in tools.GetAllTools())
        {
            activeTools.Add(new ConcreteToolData(tool));
        }
    }

    public Tools Convert(Entity entity, Entity[] externalEntities)
    {
        Tools tools = new Tools();
        foreach (ConcreteToolData tool in activeTools)
        {
            ConcreteTool convertedTool = tool.Convert(entity, externalEntities);
            if (convertedTool != null)
            {
                tools.AddTool(convertedTool);
            }
            else
            {
                Debug.LogError("Tool " + tool.Id.ToString() + " has no conversion implementatiom in ConcreteTool.Convert()");
            }
        }
        return tools;
    }
}
