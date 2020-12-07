using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectSetupUI : MonoBehaviour
{
    public ProjectSelectionUI projectType;
    public ProjectSelectionUI projectGenre;
    public ProjectFeaturesUI projectFeatures;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI money;
    public TextMeshProUGUI exp;
    public TextMeshProUGUI genreExp;
    public TextMeshProUGUI awards;
    public TextMeshProUGUI time;
    public Button buyButton;
    public Player player;
    public GameStudio studio;

    private ProjectElement _projectType = null;
    private ProjectElement _projectGenre = null;
    private List<ProjectElement> _projectFeatures;

    private void Awake()
    {
        _projectFeatures = new List<ProjectElement>();
        _projectType = null;
        _projectGenre = null;
    }

    void Start()
    {
        projectType.OnProjectSetupElementSelected += OnProjectTypeSelected;
        projectGenre.OnProjectSetupElementSelected += OnProjectGenreSelected;
        projectFeatures.OnProjectSetupElementSelected += OnProjectFeatureSelected;
        projectFeatures.OnProjectFeatureRemoved += OnProjectFeatureRemoved;
        buyButton.onClick.AddListener(StartProject);
        Setup();
    }

    private void StartProject()
    {
        if (_projectType != null && _projectGenre != null)
        {
            StartProject(new GameProject(1, (GameStudioType) _projectType.GetElement(), (GameStudioGenre) _projectGenre.GetElement(), ConvertFeatures()));
        }
    }

    private List<GameStudioFeature> ConvertFeatures()
    {
        List<GameStudioFeature> newFeatures = new List<GameStudioFeature>();
        foreach (ProjectElement feature in _projectFeatures)
        {
            newFeatures.Add((GameStudioFeature) feature.GetElement());
        }
        return newFeatures;
    }

    public void Setup()
    {
        if (_projectType != null)
        {
            _projectType.SetSelected(false);
            _projectType.UpdateUI();
        }
        if (_projectGenre != null)
        {
            _projectGenre.SetSelected(false);
            _projectType.UpdateUI();
        }
        foreach (ProjectElement feature in _projectFeatures)
        {
            feature.SetSelected(false);
            _projectType.UpdateUI();
        }
        _projectType = null;
        _projectGenre = null;
        _projectFeatures.Clear();
        UpdateUI();
    }

    void Update()
    {

    }

    private void OnProjectTypeSelected(object sender, ProjectSelectionArgs args)
    {
        _projectType = args.Element;
        UpdateUI();
    }
    private void OnProjectGenreSelected(object sender, ProjectSelectionArgs args)
    {
        _projectGenre = args.Element;
        UpdateUI();
    }
    private void OnProjectFeatureSelected(object sender, ProjectSelectionArgs args)
    {
        _projectFeatures.Add(args.Element);
        UpdateUI();
    }
    private void OnProjectFeatureRemoved(object sender, ProjectSelectionArgs args)
    {
        List<ProjectElement> newFeatures = new List<ProjectElement>();
        foreach (ProjectElement element in _projectFeatures)
        {
            if (!element.GetName().Equals(args.Element.GetName()))
            {
                newFeatures.Add(element);
            }
        }
        _projectFeatures = newFeatures;
        UpdateUI();
    }

    private void UpdateUI()
    {
        GameStudioType projectType = null;
        if (_projectType != null)
        {
            projectType = (GameStudioType) _projectType.GetElement();
        }        
        GameStudioGenre projectGenre = null;
        if (_projectGenre != null)
        {
            projectGenre = (GameStudioGenre) _projectGenre.GetElement();
        }
        GameProject project = new GameProject(1, projectType, projectGenre, ConvertFeatures());
        cost.text = project.GetCost().Format();
        money.text = project.GetMoneyReward().Format();
        exp.text = new NumberFormatter(project.GetExperienceReward()).Format();
        genreExp.text = new NumberFormatter(project.GetGenreExperienceReward()).Format();
        awards.text = new NumberFormatter(project.GetAwardReward()).Format();
        time.text = project.GetTime().ToString();
        if (player.Money >= project.GetCost() && _projectType != null && _projectGenre != null)
        {
            buyButton.interactable = true;
        }
        else
        {
            buyButton.interactable = false;
        }
    }

    public void StartProject(GameProject project)
    {
        OnProjectStarted?.Invoke(this, new ProjectArgs(project, 0f));
    }

    public EventHandler<ProjectArgs> OnProjectStarted;
}
