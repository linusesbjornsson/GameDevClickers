using System.Collections.Generic;

public abstract class ConcreteTool : Tool
{
    protected Queue<ToolUpgrade> _nextUpgrades = new Queue<ToolUpgrade>();
    protected Dictionary<ToolID, ToolUpgrade> _upgrades = new Dictionary<ToolID, ToolUpgrade>();
    protected int _numberOfUpgrades = 0;
    public ConcreteTool(string name, int level, Entity entity) : base(name, level, entity)
    {
        SetupUpgrades();
    }

    public ConcreteTool(string name, int level, Entity entity, Queue<ToolUpgrade> nextUpgrades, Dictionary<ToolID, ToolUpgrade> upgrades, int numberOfUpgrades) : base(name, level, entity)
    {
        _nextUpgrades = nextUpgrades;
        _upgrades = upgrades;
        _numberOfUpgrades = numberOfUpgrades;
    }

    protected abstract void SetupUpgrades();

    protected void CloneUpgrades(ConcreteTool tool)
    {
        Queue<ToolUpgrade> _newQueue = new Queue<ToolUpgrade>();
        foreach (ToolUpgrade upgrade in _nextUpgrades)
        {
            ToolUpgrade newUpgrade = (ToolUpgrade)upgrade.Clone();
            newUpgrade.SetConnectedTool(this);
            _newQueue.Enqueue(newUpgrade);
        }
        tool._nextUpgrades = _newQueue;
        Dictionary<ToolID, ToolUpgrade> newUpgrades = new Dictionary<ToolID, ToolUpgrade>();
        foreach (ToolID key in _upgrades.Keys)
        {
            ToolUpgrade newUpgrade = (ToolUpgrade)_upgrades[key].Clone();
            newUpgrade.SetConnectedTool(this);
            newUpgrades.Add(key, newUpgrade);
        }
        tool._upgrades = newUpgrades;
        tool._numberOfUpgrades = _numberOfUpgrades;
    }

    protected void SetupUpgrade(ToolUpgrade upgrade)
    {
        _nextUpgrades.Enqueue(upgrade);
    }

    public void AddNextUpgrade()
    {
        ToolUpgrade upgrade = _nextUpgrades.Dequeue();
        if (_upgrades.ContainsKey(upgrade.GetID()))
        {
            _upgrades[upgrade.GetID()] = upgrade;
        }
        else
        {
            _upgrades.Add(upgrade.GetID(), upgrade);
        }
        _numberOfUpgrades++;
    }

    public ToolUpgrade GetUpgrade(ToolID id)
    {
        _upgrades.TryGetValue(id, out ToolUpgrade foundUpgrade);
        return foundUpgrade;
    }

    public ConcreteTool NextLevel()
    {
        if (IsMaxLevel())
        {
            return null;
        }
        ConcreteTool tool = (ConcreteTool) Clone();
        tool.Level++;
        return tool;
    }

    public ToolUpgrade PurchaseUpgrade()
    {
        ToolUpgrade upgrade = NextUpgrade();
        Invalidate();
        return upgrade;
    }

    public ToolUpgrade NextUpgrade()
    {
        if (_nextUpgrades.Count == 0)
        {
            return null;
        }
        ToolUpgrade upgrade = _nextUpgrades.Peek();
        return upgrade;
    }

    public uint NextUpgradeLevel()
    {
        if (NextUpgrade() == null)
        {
            return 0;
        }
        if (_numberOfUpgrades >= LevelRange().Length)
        {
            return 0;
        }
        uint range = LevelRange()[_numberOfUpgrades];
        return range;
    }

    public int GetNumberOfUpgrades()
    {
        return _numberOfUpgrades;
    }

    public void SetUpgrades(int upgrades)
    {
        for (int i = 0; i < upgrades; i++)
        {
            AddNextUpgrade();
        }
    }

    public virtual string Requirement()
    {
        return "Level " + NextUpgradeLevel();
    }

    public bool IsUpgradeUnlocked()
    {
        return Level >= NextUpgradeLevel();
    }

    protected virtual uint[] LevelRange()
    {
        return new uint[] { 5, 10, 25, 50, 75, 100, 125, 150, 175, 200 };
    }
}