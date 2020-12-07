using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProjectElement : MonoBehaviour, IRewardProvider
{
    public TextMeshProUGUI projectName;
    public Transform expReward;
    public Transform awardReward;
    public Transform cost;
    public TextMeshProUGUI featureMultiplier;
    public Button selectButton;

    private bool _isSelected = false;
    private IGameStudioElement _element;

    public static ProjectElement Create(Transform parent, IGameStudioElement element)
    {
        Transform transform = Instantiate(GameAssets.Instance.pfProjectTemplate);
        transform.SetParent(parent, false);
        ProjectElement projectType = transform.GetComponent<ProjectElement>();
        projectType.Setup(element);
        return projectType;
    }

    public void Setup(IGameStudioElement element)
    {
        _element = element;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        switch (_element.GetProjectType())
        {
            case GameProject.ProjectElementType.FEATURE:
                projectName.text = _element.GetName();
                expReward.gameObject.SetActive(false);
                awardReward.gameObject.SetActive(false);
                cost.gameObject.SetActive(false);
                featureMultiplier.gameObject.SetActive(true);
                featureMultiplier.text = (_element.GetMultiplier()).ToString() + "X";
                break;
            default:
                projectName.text = _element.GetName();
                if (_element.GetProjectType() == GameProject.ProjectElementType.GENRE)
                {
                    expReward.GetComponentInChildren<TextMeshProUGUI>().color = new Color(255, 73, 0);
                }
                expReward.GetComponentInChildren<TextMeshProUGUI>().text = new NumberFormatter(_element.GetExperienceReward()).Format();
                if (_element.GetAwardReward() > BigRational.Zero)
                {
                    awardReward.gameObject.SetActive(true);
                    awardReward.GetComponentInChildren<TextMeshProUGUI>().text = _element.GetAwardReward().Format();
                }
                cost.GetComponentInChildren<TextMeshProUGUI>().text = _element.GetCost().Format();
                break;
        }
    }

    public System.Numerics.BigInteger GetExperienceReward()
    {
        return _element.GetExperienceReward();
    }

    public BigRational GetMoneyReward()
    {
        return BigRational.Zero;
    }

    public BigRational GetAwardReward()
    {
        return _element.GetAwardReward();
    }

    public float GetMultiplier()
    {
        return _element.GetMultiplier();
    }

    public BigRational GetCost()
    {
        return _element.GetCost();
    }

    public string GetName()
    {
        return _element.GetName();
    }
    
    public IGameStudioElement GetElement()
    {
        return _element;
    }

    public void AddClickListener(UnityAction handler)
    {
        selectButton.onClick.AddListener(handler);
    }

    public void SetSelected(bool selected)
    {
        _isSelected = selected;
        if (_element.GetProjectType() == GameProject.ProjectElementType.FEATURE)
        {
            TextMeshProUGUI buttonText = selectButton.GetComponentInChildren<TextMeshProUGUI>();
            if (selected)
            {
                buttonText.text = "Remove";
            }
            else
            {
                buttonText.text = "Select";
            }
        }
        else
        {
            selectButton.interactable = !_isSelected;
        }
    }

    public bool IsSelected()
    {
        return _isSelected;
    }
}
