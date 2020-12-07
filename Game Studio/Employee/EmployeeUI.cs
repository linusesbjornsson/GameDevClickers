using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeUI : MonoBehaviour
{
    public TextMeshProUGUI employeeName;
    public TextMeshProUGUI employeeCount;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI buyText;

    private ConcreteTool _tool;
    private GameStudio _studio;
    private Player _player;
    private UpgradeTooltip _tooltip;

    private int option = 1;

    public static EmployeeUI Create(Transform parent, GameStudio studio, Player player, ConcreteTool tool)
    {
        Transform transform = Instantiate(GameAssets.Instance.pfEmployee);
        transform.SetParent(parent, false);
        EmployeeUI employeeUi = transform.GetComponent<EmployeeUI>();
        employeeUi.Setup(studio, player, tool);
        return employeeUi;
    }

    public void Setup(GameStudio studio, Player player, ConcreteTool tool)
    {
        _tooltip = transform.parent.parent.parent.GetComponentInChildren<UpgradeTooltip>(true);
        _studio = studio;
        _player = player;
        _tool = tool;
        UpdateUI();
        GetComponent<Button>().onClick.AddListener(() => ToolPurchase());
        buyText.gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        employeeName.text = _tool.GetName();
        Tool nextTool;
        if (_studio.Tools.GetTool(_tool.GetID()) == null)
        {
            employeeCount.text = "Employees: " + (_tool.Level - 1).ToString();
            nextTool = _tool;
        }
        else
        {
            employeeCount.text = "Employees: " + _tool.Level.ToString();
            nextTool = _tool.NextLevel();
        }
        cost.text = new NumberFormatter(nextTool.GetCost()).Format();
        if (_player.Money >= new BigRational(nextTool.GetCost()))
        {
            GetComponent<Button>().GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().GetComponent<Button>().interactable = false;
        }
        UpdateTooltip();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            option = 10;
        }
        else if (Input.GetKey(KeyCode.LeftAlt))
        {
            option = 25;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            option = 100;
        }
        else
        {
            option = 1;
        }
        if (option != 1)
        {
            buyText.gameObject.SetActive(true);
            buyText.text = "Buy x" + option.ToString();
        }
        else
        {
            buyText.gameObject.SetActive(false);
        }
        if (_player.Money >= new BigRational(GetToolCost(option)))
        {
            GetComponent<Button>().GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().GetComponent<Button>().interactable = false;
        }
        cost.text = new NumberFormatter(GetToolCost(option)).Format();

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
            clonedTool = (ConcreteTool)_tool.NextLevel().Clone();
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

    private void ToolPurchase()
    {
        ConcreteTool purchasedTool;
        if (_studio.Tools.GetTool(_tool.GetID()) == null)
        {
            purchasedTool = (ConcreteTool)_tool.Clone();
        }
        else
        {
            purchasedTool = (ConcreteTool)_tool.NextLevel().Clone();
        }
        for (int i = 0; i < option; i++)
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
        _tool = purchasedTool;
        _player.Money -= new BigRational(GetToolCost(option));
        _studio.Tools.AddTool(purchasedTool.GetID(), purchasedTool);
        UpdateUI();
    }

    public void UpdateTooltip()
    {
        _tooltip.UpdateTooltip(this, _tool.GetName(), FormatDescription(), "", _tool.GetCost());
    }

    private string FormatDescription()
    {
        string description = _tool.GetDescription();
        if (_studio.Tools.GetTool(_tool.GetID()) != null)
        {
            description += "\n\n" + "Next level: " + _tool.NextLevel().GetModifier().Format();
        }
        return description;
    }

    public void ShowTooltip()
    {
        _tooltip.Show(this, _tool.GetName(), FormatDescription(), "", _tool.GetCost());
    }

    public void HideTooltip()
    {
        _tooltip.Hide();
    }
}
