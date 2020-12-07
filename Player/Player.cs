using System;
using System.Numerics;
using UnityEngine;
using static Job;

public class Player : Entity
{

    private BigRational _money = BigRational.Zero;
    private BigRational _technology = BigRational.Zero;
    private BigRational _design = BigRational.Zero;
    private BigRational _audio = BigRational.Zero;
    private BigRational _awards = BigRational.Zero;
    private BigRational _pendingAwards = BigRational.Zero;
    private PlayerManager _manager;

    public BigRational Money
    {
        get
        {
            return _money;
        }
        set
        {
            BigRational oldMoney = _money;
            if (oldMoney != null && (oldMoney == BigRational.Zero && oldMoney > value) || value < BigRational.Zero)
            {
                _money = BigRational.Zero;
                OnMoneyChanged?.Invoke(this, new MoneyArgs(oldMoney, _money));
            }
            else
            {
                _money = value;
                if (oldMoney != null)
                {
                    OnMoneyChanged?.Invoke(this, new MoneyArgs(oldMoney, _money));
                }
                else
                {
                    OnMoneyChanged?.Invoke(this, new MoneyArgs(BigRational.Zero, _money));
                }
            }
        }
    }
    public BigRational Technology
    {
        get
        {
            Tool engineeringPractice = Tools.GetTool(Tool.ToolID.ENGINEERING_PRACTICE);
            BigRational result = BigRational.Multiply(_technology, BigRational.Add(BigRational.Multiply(_awards, new BigRational(0.1f)), new BigRational(1)));
            if (engineeringPractice != null)
            {
                result *= new BigRational(1) + engineeringPractice.GetModifier();
            }
            return result;
        }
        set
        {
            BigRational oldValue = _technology;
            _technology = value;
            OnStatChanged?.Invoke(this, new StatArgs(oldValue, _technology));
        }
    }
    public BigRational Design
    {
        get
        {
            Tool designClasses = Tools.GetTool(Tool.ToolID.DESIGN_CLASSES);
            BigRational result = BigRational.Multiply(_design, BigRational.Add(BigRational.Multiply(_awards, new BigRational(0.1f)), new BigRational(1)));
            if (designClasses != null)
            {
                result *= new BigRational(1) + designClasses.GetModifier();
            }
            return result;
        }
        set
        {
            BigRational oldValue = _design;
            _design = value;
            OnStatChanged?.Invoke(this, new StatArgs(oldValue, _design));
        }
    }
    public BigRational Audio
    {
        get
        {
            Tool soundStudio = Tools.GetTool(Tool.ToolID.SOUND_STUDIO);
            BigRational result = BigRational.Multiply(_audio, BigRational.Add(BigRational.Multiply(_awards, new BigRational(0.1f)), new BigRational(1)));
            if (soundStudio != null)
            {
                result *= new BigRational(1) + soundStudio.GetModifier();
            }
            return result;
        }
        set
        {
            BigRational oldValue = _audio;
            _audio = value;
            OnStatChanged?.Invoke(this, new StatArgs(oldValue, _audio));
        }
    }
    public BigRational Awards
    {
        get
        {
            return _awards;
        }
        set
        {
            BigRational oldAwards = _awards;
            _awards = value;
            OnAwardsChanged?.Invoke(this, new AwardsArgs(oldAwards, _awards));
        }
    }
    public BigRational PendingAwards
    {
        get
        {
            return _pendingAwards;
        }
        set
        {
            _pendingAwards = value;
        }
    }

    public BigRational GetUnmodifiedTechnology()
    {
        return _technology;
    }

    public BigRational GetUnmodifiedDesign()
    {
        return _design;
    }

    public BigRational GetUnmodifiedAudio()
    {
        return _audio;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        Tools.AddTool(Tool.ToolID.INFRASTRUCTURE_UPGRADE, new InfrastructureUpgradeTool(1, this));
        Tools.AddTool(Tool.ToolID.TOOL_UPGRADE, new ToolUpgradeTool(1, this));
        _technology = new BigRational(1);
        _design = new BigRational(1);
        _audio = new BigRational(1);
        _awards = new BigRational(0);
        _pendingAwards = new BigRational(0);
        _money = new BigRational(0);
        _manager = new PlayerManager(this);
    }

    void Start()
    {
        OnLevelGained += OnPlayerLevelGained;
    }

    private void Update()
    {
        _manager.Update(Time.deltaTime);
    }

    void OnPlayerLevelGained(object sender, LevelArgs args)
    {
        Technology = BigRational.Add(_technology, new BigRational(1));
        Design = BigRational.Add(_design, new BigRational(1));
        Audio = BigRational.Add(_audio, new BigRational(1));
    }

    public void ToolPurchase(object sender, ToolArgs tool)
    {
        Tools.AddTool(tool.Tool.GetID(), tool.Tool);
        OnToolPurchase?.Invoke(this, tool);
        switch (tool.Tool.GetID())
        {
            case Tool.ToolID.ENGINEERING_PRACTICE:
            case Tool.ToolID.DESIGN_CLASSES:
            case Tool.ToolID.SOUND_STUDIO:
                // Perhaps fix so that specific tool sends correct updated stats?
                OnStatChanged?.Invoke(this, new StatArgs(BigRational.Zero, BigRational.Zero));
                break;
        }
    }

    public void UpgradePurchase(object sender, ToolArgs tool)
    {
        ConcreteTool playerTool = Tools.GetTool(tool.Tool.GetID());
        if (playerTool != null)
        {
            playerTool.AddNextUpgrade();
            playerTool.Invalidate();
            OnUpgradePurchase?.Invoke(this, tool);
        }
    }

    public BigRational GetFocusModifier(FocusArea area)
    {
        ConcreteTool _tool = Tools.GetTool(Tool.ToolID.TOOL_UPGRADE);
        ToolUpgrade evolution = _tool.GetUpgrade(Tool.ToolID.EVOLUTION);
        BigRational result = new BigRational(1f);
        if (evolution != null)
        {
            result += evolution.GetModifier();
        }
        return result;
    }

    public void Initialize(PlayerData data, GameStudio studio)
    {
        _audio = BigRational.Parse(data.Audio);
        _awards = BigRational.Parse(data.Awards);
        _pendingAwards = BigRational.Parse(data.PendingAwards);
        _technology = BigRational.Parse(data.Technology);
        _money = BigRational.Parse(data.Money);
        _design = BigRational.Parse(data.Design);
        Initialize(data.EntityData, new Entity[] { studio });
    }

    // Events
    public event EventHandler<MoneyArgs> OnMoneyChanged;
    public event EventHandler<AwardsArgs> OnAwardsChanged;
    public event EventHandler<StatArgs> OnStatChanged;
    public event EventHandler<ToolArgs> OnToolPurchase;
    public event EventHandler<ToolArgs> OnUpgradePurchase;
}
