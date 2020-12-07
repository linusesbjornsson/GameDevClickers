using System.Collections.Generic;

public abstract class ExternalConcreteTool : ConcreteTool
{
    protected Entity[] _externalEntities;

    public ExternalConcreteTool(string name, int level, Entity entity, Entity[] externalEntities) : base(name, level, entity)
    {
        _externalEntities = externalEntities;
    }

    public ExternalConcreteTool(string name, int level, Entity entity, Entity[] externalEntities, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base(name, level, entity, nextUpgrades, upgrades, numberOfUpgrades)
    {
        _externalEntities = externalEntities;
    }
}
