using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolUI : MonoBehaviour
{
    public TextMeshProUGUI toolName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI nextLevel;
    public TextMeshProUGUI level;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI buyText;
    public TextMeshProUGUI upgradeButtonText;
    public Button upgradeButton;

    private Button _backgroundBuy = null;

    private ConcreteTool _tool;
    private Player _player;
    private UpgradeTooltip _tooltip;
    private int _option = 1;

    public static ToolUI Create(Transform parent, Player player, ConcreteTool tool)
    {
        Transform transform = Instantiate(GameAssets.Instance.pfTool);
        transform.SetParent(parent, false);
        ToolUI projectType = transform.GetComponent<ToolUI>();
        projectType.Setup(player, tool);
        return projectType;
    }

    public void Setup(Player player, ConcreteTool tool)
    {
        _tooltip = transform.parent.parent.parent.GetComponentInChildren<UpgradeTooltip>(true);
        _player = player;
        _tool = tool;
        OnToolPurchase += _player.ToolPurchase;
        OnUpgradePurchase += _player.UpgradePurchase;
        _player.OnToolPurchase += OnPlayerToolPurchase;
        _player.OnUpgradePurchase += OnPlayerUpgradePurchase;
        _player.OnMoneyChanged += OnMoneyChanged;
        _backgroundBuy = transform.Find("Background").GetComponent<Button>();
        _backgroundBuy.onClick.AddListener(() => ToolPurchase());
        buyText.gameObject.SetActive(false);
        upgradeButton.onClick.AddListener(() => UpgradePurchase());
        upgradeButton.gameObject.SetActive(false);
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _option = 10;
        }
        else if (Input.GetKey(KeyCode.LeftAlt))
        {
            _option = 25;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            _option = 100;
        }
        else
        {
            if (ToolBuyOptionHandler.Instance().GetOption() > 1)
            {
                _option = ToolBuyOptionHandler.Instance().GetOption();
            }
            else
            {
                _option = 1;
            }
        }
        if (_option != 1)
        {
            buyText.gameObject.SetActive(true);
            buyText.text = "Buy x" + _option.ToString();
        }
        else
        {
            buyText.gameObject.SetActive(false);
        }
        if (!_tool.IsMaxLevel())
        {
            if (_player.Money >= new BigRational(GetToolCost(_option)))
            {
                _backgroundBuy.interactable = true;
            }
            else
            {
                _backgroundBuy.interactable = false;
            }
            cost.text = new NumberFormatter(GetToolCost(_option)).Format();
        }
        else
        {
            cost.text = "";
        }

    }

    private System.Numerics.BigInteger GetToolCost(int amount)
    {
        System.Numerics.BigInteger cost = new System.Numerics.BigInteger(0);
        ConcreteTool clonedTool;
        if (_player.Tools.GetTool(_tool.GetID()) == null)
        {
            clonedTool = (ConcreteTool)_tool.Clone();
        }
        else
        {
            clonedTool = _tool.NextLevel();
        }
        for (int i = 0; i < amount; i++)
        {
            cost += clonedTool.GetCost();
            clonedTool.Level++;
            if (clonedTool.IsMaxLevel())
            {
                break;
            }
        }
        return cost;
    }

    private void OnMoneyChanged(object sender, MoneyArgs args)
    {
        Tool nextTool;
        if (_player.Tools.GetTool(_tool.GetID()) == null)
        {
            nextTool = _tool;
        }
        else
        {
            nextTool = _tool.NextLevel();
        }
        if (!_tool.IsMaxLevel())
        {
            if (_player.Money >= new BigRational(nextTool.GetCost()))
            {
                _backgroundBuy.interactable = true;
            }
            else
            {
                _backgroundBuy.interactable = false;
            }
        }
        else
        {
            _backgroundBuy.interactable = false;
        }
        if (_tool.NextUpgrade() != null)
        {
            ToolUpgrade upgrade = _tool.NextUpgrade();
            if (_tool.IsUpgradeUnlocked() && _player.Money >= new BigRational(upgrade.GetCost()))
            {
                upgradeButton.interactable = true;
            }
            else
            {
                upgradeButton.interactable = false;
            }
        }
    }

    private void OnPlayerToolPurchase(object sender, ToolArgs args)
    {
        UpdateUI();
    }

    private void OnPlayerUpgradePurchase(object sender, ToolArgs args)
    {
        _tool.Invalidate();
        UpdateUI();
    }

    private void UpdateUI()
    {
        toolName.text = _tool.GetName();
        description.text = _tool.GetDescription();
        if (_tool.Level == -1)
        {
            level.gameObject.SetActive(false);
        }
        else
        {
            level.text = "Level " + _tool.Level.ToString();
        }
        Tool nextTool;
        if (_player.Tools.GetTool(_tool.GetID()) == null)
        {
            nextTool = _tool;
        }
        else
        {
            nextTool = _tool.NextLevel();
        }
        if (nextTool != null)
        {
            cost.text = new NumberFormatter(nextTool.GetCost()).Format();
            if (_player.Money >= new BigRational(nextTool.GetCost()))
            {
                _backgroundBuy.interactable = true;
            }
            else
            {
                _backgroundBuy.interactable = false;
            }
            if (_tool.NextLevel() != null && _player.Tools.GetTool(_tool.GetID()) != null)
            {
                nextLevel.gameObject.SetActive(true);
                nextLevel.text = "Next Level: " + _tool.NextLevel().ToolTipModifier();
            }
            else
            {
                nextLevel.gameObject.SetActive(false);
            }
        }
        else
        {
            nextLevel.gameObject.SetActive(false);
            _backgroundBuy.interactable = false;
        }
        if (_tool.NextUpgrade() != null)
        {
            ToolUpgrade upgrade = _tool.NextUpgrade();
            upgradeButton.gameObject.SetActive(true);
            upgradeButtonText.text = upgrade.GetName();
            if (_tool.IsUpgradeUnlocked() && _player.Money >= new BigRational(upgrade.GetCost()))
            {
                upgradeButton.interactable = true;
            }
            else
            {
                upgradeButton.interactable = false;
            }
        }
        else
        {
            upgradeButton.gameObject.SetActive(false);
        }
        ToolUpgrade nextUpgrade = _tool.NextUpgrade();
        if (nextUpgrade != null)
        {
            _tooltip.UpdateTooltip(this, nextUpgrade.GetName(), nextUpgrade.GetDescription(), _tool.Requirement(), nextUpgrade.GetCost());
        }
    }

    private void ToolPurchase()
    {
        ConcreteTool purchasedTool;
        if (_player.Tools.GetTool(_tool.GetID()) == null)
        {
            purchasedTool = (ConcreteTool)_tool.Clone();
        }
        else
        {
            purchasedTool = (ConcreteTool)_tool.NextLevel().Clone();
        }
        for (int i = 0; i < _option; i++)
        {
            if (purchasedTool.IsMaxLevel())
            {
                break;
            }
            else
            {
                if (i > 0)
                {
                    purchasedTool.Level++;
                }
            }
        }
        _player.Money -= new BigRational(GetToolCost(_option));
        _tool = purchasedTool;
        if (purchasedTool.IsMaxLevel() && purchasedTool.NextUpgrade() == null)
        {
            _player.OnToolPurchase -= OnPlayerToolPurchase;
            _player.OnUpgradePurchase -= OnPlayerUpgradePurchase;
            _player.OnMoneyChanged -= OnMoneyChanged;
            Destroy(gameObject);
        }
        OnToolPurchase?.Invoke(this, new ToolArgs(purchasedTool.Clone() as ConcreteTool));
    }

    private void UpgradePurchase()
    {
        ToolUpgrade purchasedUpgrade = _tool.PurchaseUpgrade();
        _player.Money -= new BigRational(purchasedUpgrade.GetCost());
        _tool.AddNextUpgrade();
        OnUpgradePurchase?.Invoke(this, new ToolArgs(_tool, purchasedUpgrade));
        if (_tool.NextUpgrade() == null)
        {
            HideUpgradeTooltip();
            if (_tool.IsMaxLevel())
            {
                _player.OnToolPurchase -= OnPlayerToolPurchase;
                _player.OnUpgradePurchase -= OnPlayerUpgradePurchase;
                _player.OnMoneyChanged -= OnMoneyChanged;
                Destroy(gameObject);
            }
        }
        else
        {
            UpdateUI();
        }
    }

    public void ShowUpgradeTooltip()
    {
        ToolUpgrade upgrade = _tool.NextUpgrade();
        if (upgrade != null)
        {
            _tooltip.Show(this, upgrade.GetName(), upgrade.GetDescription(), _tool.Requirement(), upgrade.GetCost());
        }
    }

    public void HideUpgradeTooltip()
    {
        _tooltip.Hide();
    }

    public EventHandler<ToolArgs> OnToolPurchase;
    public EventHandler<ToolArgs> OnUpgradePurchase;
}
