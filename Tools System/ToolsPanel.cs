using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolsPanel : MonoBehaviour
{
    public Player player;
    protected List<ConcreteTool> _availableTools = new List<ConcreteTool>();
    public Button buyOne;
    public Button buyTen;
    public Button buyTwentyFive;
    public Button buyHundred;

    protected virtual void AddAvailableTool(ConcreteTool tool)
    {
        if (player.Tools.GetTool(tool.GetID()) != null)
        {
            ConcreteTool playerTool = player.Tools.GetTool(tool.GetID());
            if (tool.Level > 0)
            {
                tool.Level = playerTool.Level;
                tool.SetUpgrades(playerTool.GetNumberOfUpgrades());
                _availableTools.Add(tool);
            }
        }
        else
        {
            _availableTools.Add(tool);
        }
    }

    void Start()
    {
        OnStart();
        player.OnMoneyChanged += OnMoneyChanged;
        player.OnToolPurchase += OnToolPurchase;
        buyOne.onClick.AddListener(() => ToolBuyOptionHandler.Instance().SetOption(1));
        buyTen.onClick.AddListener(() => ToolBuyOptionHandler.Instance().SetOption(10));
        buyTwentyFive.onClick.AddListener(() => ToolBuyOptionHandler.Instance().SetOption(25));
        buyHundred.onClick.AddListener(() => ToolBuyOptionHandler.Instance().SetOption(100));
        ToolBuyOptionHandler.Instance().OnOptionChanged += OnOptionChanged;
        OptionChanged();
        CheckEligibility();
    }

    protected virtual void OnStart()
    {
        AddAvailableTool(new ToolUpgradeTool(1, player));
        AddAvailableTool(new InfrastructureUpgradeTool(1, player));
        AddAvailableTool(new ScriptTool(1, player));
        AddAvailableTool(new OnlineTutorialsTool(1, player));
        AddAvailableTool(new OutsourcingTool(1, player));
        AddAvailableTool(new HardworkerTool(1, player));
        AddAvailableTool(new PairProgrammerTool(1, player));
        AddAvailableTool(new StartupKingTool(1, player));
        AddAvailableTool(new OvertimeTool(1, player));
        AddAvailableTool(new ExpertTool(1, player));
        AddAvailableTool(new QuickLearnerTool(1, player));
        AddAvailableTool(new NegotiatorTool(1, player));
    }

    protected virtual void OptionChanged()
    {
        switch (ToolBuyOptionHandler.Instance().GetOption())
        {
            case 10:
                buyTen.interactable = false;
                buyOne.interactable = true;
                buyHundred.interactable = true;
                buyTwentyFive.interactable = true;
                break;
            case 25:
                buyTwentyFive.interactable = false;
                buyOne.interactable = true;
                buyHundred.interactable = true;
                buyTen.interactable = true;
                break;
            case 100:
                buyHundred.interactable = false;
                buyOne.interactable = true;
                buyTwentyFive.interactable = true;
                buyTwentyFive.interactable = true;
                break;
            default:
                buyOne.interactable = false;
                buyTen.interactable = true;
                buyHundred.interactable = true;
                buyTwentyFive.interactable = true;
                break;
        }
    }

    protected virtual void OnOptionChanged(object sender, EventArgs args)
    {
        OptionChanged();
    }

    protected void OnMoneyChanged(object sender, MoneyArgs args)
    {
        CheckEligibility();
    }

    protected void OnToolPurchase(object sender, ToolArgs args)
    {
        CheckEligibility();
    }

    protected virtual void CheckEligibility()
    {
        List<ConcreteTool> _newTools = new List<ConcreteTool>();
        foreach (ConcreteTool tool in _availableTools)
        {
            if (tool.IsUnlocked())
            {
                ToolUI.Create(transform, player, tool);
            }
            else
            {
                _newTools.Add(tool);
            }
        }
        _availableTools = _newTools;
    }
}
