using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Job;

public class GameProjectUI : ProgressBarBase
{
    public TextMeshProUGUI projectName;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI experienceReward;
    public TextMeshProUGUI genreExperienceReward;
    private BigRational _technologyProgress;
    private BigRational _designProgress;
    private BigRational _audioProgress;
    public Button addTechnologyButton;
    public Button addDesignButton;
    public Button addAudioButton;

    public Slider technologySlider;
    public Slider designSlider;
    public Slider audioSlider;
    public TextMeshProUGUI technologyText;
    public TextMeshProUGUI designText;
    public TextMeshProUGUI audioText;

    protected Queue<BarEntry> queue = new Queue<BarEntry>();
    protected BarEntry currentEntry = null;
    private GameProject _project;
    private Player _player;
    private GameStudio _studio;
    private float seconds = 0;

    public static GameProjectUI Create(Transform transform, Player player, GameStudio studio, GameProject project)
    {
        Transform gameProjectTransform = Instantiate(GameAssets.Instance.pfGameProject);
        gameProjectTransform.transform.SetParent(transform, false);
        GameProjectUI job = gameProjectTransform.GetComponent<GameProjectUI>();
        job.Setup(player, studio, project);
        return job;
    }

    public void Setup(Player player, GameStudio studio, GameProject project)
    {
        _player = player;
        _studio = studio;
        _project = project;
        InitializeModifiers();
        timeText.text = "Experience gained: 100%";
        levelText.text = "Level " + project.GetLevel();
        experienceReward.text = new NumberFormatter(_project.GetExperienceReward()).Format();
        genreExperienceReward.text = new NumberFormatter(_project.GetGenreExperienceReward()).Format();
        projectName.text = _project.Type.GetName() + "\n" + _project.Genre.GetName();
        technologyText.text = _technologyProgress.ToString() + "/" + _project.GetMaxProgress(FocusArea.FocusPoint.TECHNOLOGY);
        designText.text = _designProgress.ToString() + "/" + _project.GetMaxProgress(FocusArea.FocusPoint.DESIGN);
        audioText.text = _audioProgress.ToString() + "/" + _project.GetMaxProgress(FocusArea.FocusPoint.AUDIO);
    }

    private void Awake()
    {
        slider.value = 100;
        _technologyProgress = new BigRational(0);
        _designProgress = new BigRational(0);
        _audioProgress = new BigRational(0);
        seconds = 0;
    }

    private void Update()
    {
        float expPercentage = ((technologySlider.value / 100) + (designSlider.value / 100) + (audioSlider.value / 100)) / 3 * 100;
        timeText.text = "Experience gained: " + Math.Round(expPercentage, 2) + "%";
        technologyText.text = _technologyProgress.Format() + "/" + _project.GetMaxProgress(FocusArea.FocusPoint.TECHNOLOGY).Format();
        designText.text = _designProgress.Format() + "/" + _project.GetMaxProgress(FocusArea.FocusPoint.DESIGN).Format();
        audioText.text = _audioProgress.Format() + "/" + _project.GetMaxProgress(FocusArea.FocusPoint.AUDIO).Format();
        IdleUpdateProject();
        if (currentEntry != null)
        {
            switch (currentEntry.Focus)
            {
                case FocusArea.FocusPoint.TECHNOLOGY:
                    if (technologySlider.value < currentEntry.Progress)
                    {
                        technologySlider.value += GetFillSpeed() * Time.deltaTime;
                    }
                    else
                    {
                        currentEntry = null;
                    }
                    break;
                case FocusArea.FocusPoint.DESIGN:
                    if (designSlider.value < currentEntry.Progress)
                    {
                        designSlider.value += GetFillSpeed() * Time.deltaTime;
                    }
                    else
                    {
                        currentEntry = null;
                    }
                    break;
                case FocusArea.FocusPoint.AUDIO:
                    if (audioSlider.value < currentEntry.Progress)
                    {
                        audioSlider.value += GetFillSpeed() * Time.deltaTime;
                    }
                    else
                    {
                        currentEntry = null;
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (queue.Count > 0)
            {
                currentEntry = queue.Dequeue();
            }
        }
        if (GameProjectFinished())
        {
            NumberPopupAnimation.Create(transform.position, "Project Completed", 12, Color.green);
            Destroy(gameObject);
            OnProjectFinished?.Invoke(this, new ProjectArgs(_project, ((technologySlider.value / 100) + (designSlider.value / 100) + (audioSlider.value / 100)) / 3));
        }
        else
        {
            seconds += Time.deltaTime;
            float progress = GetProgress(seconds, _project.GetTime());
            slider.value = 100 - progress;
            if (slider.value <= 0)
            {
                OnProjectFinished?.Invoke(this, new ProjectArgs(_project, ((technologySlider.value / 100) + (designSlider.value / 100) + (audioSlider.value / 100)) / 3));
                NumberPopupAnimation.Create(transform.position, "Project Failed", 12, Color.red);
                Destroy(gameObject);
            }
        }
    }

    private void IdleUpdateProject()
    {
        Tool softwareEngineer = _studio.Tools.GetTool(Tool.ToolID.SOFTWARE_ENGINEER);
        if (softwareEngineer != null)
        {
            IncreaseIdleProgress(FocusArea.FocusPoint.TECHNOLOGY, BigRational.Multiply(softwareEngineer.GetModifier(), new BigRational(Time.deltaTime)));
        }
        Tool designer = _studio.Tools.GetTool(Tool.ToolID.DESIGNER);
        if (designer != null)
        {
            IncreaseIdleProgress(FocusArea.FocusPoint.DESIGN, BigRational.Multiply(designer.GetModifier(), new BigRational(Time.deltaTime)));
        }
        Tool soundDesigner = _studio.Tools.GetTool(Tool.ToolID.SOUND_DESIGNER);
        if (soundDesigner != null)
        {
            IncreaseIdleProgress(FocusArea.FocusPoint.AUDIO, BigRational.Multiply(soundDesigner.GetModifier(), new BigRational(Time.deltaTime)));
        }
    }

    public void IncreaseIdleProgress(FocusArea.FocusPoint focus, BigRational value)
    {
        switch (focus)
        {
            case FocusArea.FocusPoint.TECHNOLOGY:
                _technologyProgress += value;
                if (_technologyProgress > _project.GetMaxProgress(FocusArea.FocusPoint.TECHNOLOGY))
                {
                    _technologyProgress = _project.GetMaxProgress(FocusArea.FocusPoint.TECHNOLOGY);
                }
                technologySlider.value += GetProgress(value, _project.GetMaxProgress(FocusArea.FocusPoint.TECHNOLOGY));
                break;
            case FocusArea.FocusPoint.DESIGN:
                _designProgress += value;
                if (_designProgress > _project.GetMaxProgress(FocusArea.FocusPoint.DESIGN))
                {
                    _designProgress = _project.GetMaxProgress(FocusArea.FocusPoint.DESIGN);
                }
                designSlider.value += GetProgress(value, _project.GetMaxProgress(FocusArea.FocusPoint.DESIGN));
                break;
            case FocusArea.FocusPoint.AUDIO:
                _audioProgress += value;
                if (_audioProgress > _project.GetMaxProgress(FocusArea.FocusPoint.AUDIO))
                {
                    _audioProgress = _project.GetMaxProgress(FocusArea.FocusPoint.AUDIO);
                }
                audioSlider.value += GetProgress(value, _project.GetMaxProgress(FocusArea.FocusPoint.AUDIO));
                break;
        }
    }

    private void InitializeModifiers()
    {
        System.Numerics.BigInteger programmingModifier = GetFocusModifier(FocusArea.FocusPoint.TECHNOLOGY);
        System.Numerics.BigInteger designModifier = GetFocusModifier(FocusArea.FocusPoint.DESIGN);
        System.Numerics.BigInteger audioModifier = GetFocusModifier(FocusArea.FocusPoint.AUDIO);
        addTechnologyButton.onClick.RemoveAllListeners();
        addTechnologyButton.onClick.AddListener(() => IncreaseProgress(FocusArea.FocusPoint.TECHNOLOGY, new BigRational(programmingModifier)));
        FormatModifierText(addTechnologyButton.GetComponentInChildren<TextMeshProUGUI>(), programmingModifier);
        addDesignButton.onClick.RemoveAllListeners();
        addDesignButton.onClick.AddListener(() => IncreaseProgress(FocusArea.FocusPoint.DESIGN, new BigRational(designModifier)));
        FormatModifierText(addDesignButton.GetComponentInChildren<TextMeshProUGUI>(), designModifier);
        addAudioButton.onClick.RemoveAllListeners();
        addAudioButton.onClick.AddListener(() => IncreaseProgress(FocusArea.FocusPoint.AUDIO, new BigRational(audioModifier)));
        FormatModifierText(addAudioButton.GetComponentInChildren<TextMeshProUGUI>(), audioModifier);
    }

    private void FormatModifierText(TextMeshProUGUI text, System.Numerics.BigInteger value)
    {
        IFormatter formatter = new NumberFormatter(value);
        string valueText = formatter.Format();
        text.text = "+" + valueText;
        if (valueText.Length > 3)
        {
            switch (valueText.Length)
            {
                case 3:
                    text.fontSize = 14;
                    break;
                case 4:
                    text.fontSize = 12;
                    break;
                default:
                    text.fontSize = 10;
                    break;
            }
        }
    }

    protected virtual System.Numerics.BigInteger FocusModifierValue(FocusArea area, BigRational value)
    {
        BigRational returnValue;
        returnValue = value * new BigRational(area.Magnitude);
        returnValue *= value * new BigRational(0.025f) * _player.GetFocusModifier(area) + new BigRational(1);
        return returnValue.WholePart;
    }

    private System.Numerics.BigInteger GetFocusModifier(FocusArea.FocusPoint area)
    {
        foreach (FocusArea focus in _project.GetFocus())
        {
            if (area == focus.Point)
            {
                return GetFocusModifier(focus);
            }
        }
        return 0;
    }

    private System.Numerics.BigInteger GetFocusModifier(FocusArea area)
    {
        System.Numerics.BigInteger value = 0;
        switch (area.Point)
        {
            case FocusArea.FocusPoint.TECHNOLOGY:
                value = FocusModifierValue(area, _player.Technology);
                break;
            case FocusArea.FocusPoint.DESIGN:
                value = FocusModifierValue(area, _player.Design);
                break;
            case FocusArea.FocusPoint.AUDIO:
                value = FocusModifierValue(area, _player.Audio);
                break;
            default:
                break;
        }
        return value;
    }

    public void IncreaseProgress(FocusArea.FocusPoint focus, BigRational value)
    {
        if (value > BigRational.Zero)
        {
            Vector3 newPosition = new Vector3(transform.position.x + 3f, transform.position.y);
            switch (focus)
            {
                case FocusArea.FocusPoint.TECHNOLOGY:
                    _technologyProgress += value;
                    if (_technologyProgress >= _project.GetMaxProgress(FocusArea.FocusPoint.TECHNOLOGY))
                    {
                        _technologyProgress = _project.GetMaxProgress(FocusArea.FocusPoint.TECHNOLOGY);
                    }
                    queue.Enqueue(new BarEntry(GetProgress(_technologyProgress, _project.GetMaxProgress(FocusArea.FocusPoint.TECHNOLOGY)), FocusArea.FocusPoint.TECHNOLOGY));
                    NumberPopupAnimation.Create(newPosition, "+" + value.Format(), 6, Color.red);
                    break;
                case FocusArea.FocusPoint.DESIGN:
                    _designProgress += value;
                    if (_designProgress >= _project.GetMaxProgress(FocusArea.FocusPoint.DESIGN))
                    {
                        _designProgress = _project.GetMaxProgress(FocusArea.FocusPoint.DESIGN);
                    }
                    queue.Enqueue(new BarEntry(GetProgress(_designProgress, _project.GetMaxProgress(FocusArea.FocusPoint.DESIGN)), FocusArea.FocusPoint.DESIGN));
                    NumberPopupAnimation.Create(newPosition, "+" + value.Format(), 6, Color.green);
                    break;
                case FocusArea.FocusPoint.AUDIO:
                    _audioProgress += value;
                    if (_audioProgress >= _project.GetMaxProgress(FocusArea.FocusPoint.AUDIO))
                    {
                        _audioProgress = _project.GetMaxProgress(FocusArea.FocusPoint.AUDIO);
                    }
                    queue.Enqueue(new BarEntry(GetProgress(_audioProgress, _project.GetMaxProgress(FocusArea.FocusPoint.AUDIO)), FocusArea.FocusPoint.AUDIO));
                    NumberPopupAnimation.Create(newPosition, "+" + value.Format(), 6, Color.yellow);
                    break;
                default:
                    break;
            }
        }
    }

    private bool GameProjectFinished()
    {
        return _technologyProgress >= _project.GetMaxProgress(FocusArea.FocusPoint.TECHNOLOGY)
            && _designProgress >= _project.GetMaxProgress(FocusArea.FocusPoint.DESIGN)
            && _audioProgress >= _project.GetMaxProgress(FocusArea.FocusPoint.AUDIO);
    }

    protected class BarEntry
    {
        public float Progress { get; set; }
        public FocusArea.FocusPoint Focus { get; set; }

        public BarEntry(float progress, FocusArea.FocusPoint focus)
        {
            Progress = progress;
            Focus = focus;
        }
    }

    public EventHandler<ProjectArgs> OnProjectFinished;
}
