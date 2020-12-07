using System.Numerics;
using TMPro;
using UnityEngine;

public class UpgradeTooltip : MonoBehaviour
{
    public TextMeshProUGUI upgradeName;
    public TextMeshProUGUI upgradeDescription;
    public TextMeshProUGUI upgradeRequirement;
    public TextMeshProUGUI upgradeCost;

    private object _sender = null;

    public void Show(object sender, string name, string description, string requirement, BigInteger cost)
    {
        _sender = sender;
        gameObject.SetActive(true);
        UpdateTooltip(sender, name, description, requirement, cost);
    }

    public void UpdateTooltip(object sender, string name, string description, string requirement, BigInteger cost)
    {
        if (_sender != null && sender == _sender)
        {
            upgradeName.text = name;
            upgradeDescription.text = description;
            if (requirement.Length > 0)
            {
                upgradeRequirement.gameObject.SetActive(true);
                upgradeRequirement.text = requirement;
            }
            else
            {
                upgradeRequirement.gameObject.SetActive(false);
            }
            if (cost > BigInteger.Zero)
            {
                upgradeCost.transform.parent.gameObject.SetActive(true);
                upgradeCost.text = new NumberFormatter(cost).Format();
            }
            else
            {
                upgradeCost.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
