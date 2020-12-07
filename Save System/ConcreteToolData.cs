
[System.Serializable]
public class ConcreteToolData : ToolData
{
    public int NumberOfUpgrades;

    public ConcreteToolData(ConcreteTool tool) : base(tool)
    {
        NumberOfUpgrades = tool.GetNumberOfUpgrades();
    }

    public ConcreteTool Convert(Entity entity, Entity[] externalEntities)
    {
        ConcreteTool tool = null;
        switch (Id)
        {
            //
            // Tools for player
            //
            case Tool.ToolID.SCRIPT:
                tool = new ScriptTool(Level, entity);
                break;
            case Tool.ToolID.INFRASTRUCTURE_UPGRADE:
                tool = new InfrastructureUpgradeTool(Level, entity);
                break;
            case Tool.ToolID.TOOL_UPGRADE:
                tool = new ToolUpgradeTool(Level, entity);
                break;
            case Tool.ToolID.HARDWORKER:
                tool = new HardworkerTool(Level, entity);
                break;
            case Tool.ToolID.NEGOTIATOR:
                tool = new NegotiatorTool(Level, entity);
                break;
            case Tool.ToolID.ONLINE_TUTORIALS:
                tool = new OnlineTutorialsTool(Level, entity);
                break;
            case Tool.ToolID.OUTSOURCING:
                tool = new OutsourcingTool(Level, entity);
                break;
            case Tool.ToolID.OVERTIME:
                tool = new OvertimeTool(Level, entity);
                break;
            case Tool.ToolID.PAIR_PROGRAMMER:
                tool = new PairProgrammerTool(Level, entity);
                break;
            case Tool.ToolID.QUICK_LEARNER:
                tool = new QuickLearnerTool(Level, entity);
                break;
            case Tool.ToolID.STARTUP_KING:
                tool = new StartupKingTool(Level, entity);
                break;
            case Tool.ToolID.EXPERT:
                tool = new ExpertTool(Level, entity);
                break;
            //
            // Upgrades for player
            //
            case Tool.ToolID.ENGINEERING_PRACTICE:
                tool = new EngineeringPractice(Level, entity);
                break;
            case Tool.ToolID.DESIGN_CLASSES:
                tool = new DesignClasses(Level, entity);
                break;
            case Tool.ToolID.SOUND_STUDIO:
                tool = new SoundStudio(Level, entity);
                break;
            case Tool.ToolID.MINER:
                tool = new Miner(Level, entity);
                break;
            case Tool.ToolID.ARCHITECT:
                tool = new Architect(Level, entity, externalEntities);
                break;
            case Tool.ToolID.CARPENTER:
                tool = new Carpenter(Level, entity, externalEntities);
                break;
            case Tool.ToolID.AUDIOLOGIST:
                tool = new Audiologist(Level, entity, externalEntities);
                break;
            //
            // Upgrades for game studio
            //
            case Tool.ToolID.SOFTWARE_ENGINEER:
                tool = new SoftwareEngineerEmployee(Level, entity, externalEntities);
                break;
            case Tool.ToolID.DESIGNER:
                tool = new DesignerEmployee(Level, entity, externalEntities);
                break;
            case Tool.ToolID.SOUND_DESIGNER:
                tool = new SoundDesignerEmployee(Level, entity, externalEntities);
                break;
        }
        if (tool != null)
        {
            tool.SetUpgrades(NumberOfUpgrades);
        }
        return tool;
    }
}
