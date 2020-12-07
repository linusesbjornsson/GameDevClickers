using System.Collections.Generic;
using UnityEngine;

public class EmployeePanel : MonoBehaviour
{
    public Player player;
    public GameStudio studio;

    void Start()
    {
        AddEmployee(new SoftwareEngineerEmployee(1, studio, new Entity[] { player }));
        AddEmployee(new DesignerEmployee(1, studio, new Entity[] { player }));
        AddEmployee(new SoundDesignerEmployee(1, studio, new Entity[] { player }));
    }

    private void AddEmployee(ExternalConcreteTool tool)
    {
        if (studio.Tools.GetTool(tool.GetID()) != null)
        {
            ConcreteTool studioTool = studio.Tools.GetTool(tool.GetID());
            if (tool.Level > 0)
            {
                tool.Level = studioTool.Level;
                tool.SetUpgrades(studioTool.GetNumberOfUpgrades());
            }
        }
        EmployeeUI.Create(transform, studio, player, tool);
    }
}