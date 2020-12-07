using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Job;

public class JobUI : ProgressBarBase
{
    public Button addTechnologyButton;
    public Button addDesignBtn;
    public Button addAudioBtn;
    public TextMeshProUGUI experienceReward;
    public TextMeshProUGUI moneyReward;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI nameText;

    protected float _currentEntry;
    protected Queue<float> _queue;

    protected ObjectPooler _pooler;
    protected GameObject _activeObject;
    protected Player _player;
    protected Job _job;
    protected bool _isFinished = false;

    protected const float FILL_SPEED = 200f;

    public static JobUI Create(ObjectPooler pooler, Transform parent, Player player, Vector3 position, Job job)
    {
        GameObject jobObject = pooler.InitializeFromPool("job");
        jobObject.transform.SetParent(parent, false);
        jobObject.transform.localPosition = position;
        JobUI jobUi = jobObject.transform.Find("Job Data").GetComponent<JobUI>();
        jobUi.Setup(pooler, jobObject, player, job);
        return jobUi;
    }

    public void Setup(ObjectPooler pooler, GameObject activeObject, Player player, Job job)
    {
        _currentEntry = -1;
        _queue = new Queue<float>();
        _pooler = pooler;
        _job = job;
        slider.value = 0;
        _activeObject = activeObject;
        ResetAnimation(true);
        _player = player;
        _player.OnStatChanged += OnPlayerStatChange;
        _job.OnJobFinished += OnJobDataFinished;
        _job.OnJobProgressIncreased += OnJobProgressIncreased;
        InitializeJob();
    }

    public override float GetFillSpeed()
    {
        return FILL_SPEED;
    }

    private void OnPlayerStatChange(object sender, StatArgs args)
    {
        InitializeModifiers();
    }

    private void OnJobDataFinished(object sender, JobArgs args)
    {
        _isFinished = true;
    }

    private void InitializeModifiers()
    {
        BigRational technologyModifier = _job.GetFocusModifier(FocusArea.FocusPoint.TECHNOLOGY);
        BigRational designModifier = _job.GetFocusModifier(FocusArea.FocusPoint.DESIGN);
        BigRational audioModifier = _job.GetFocusModifier(FocusArea.FocusPoint.AUDIO);
        addTechnologyButton.onClick.RemoveAllListeners();
        addTechnologyButton.onClick.AddListener(() => IncreaseProgress(FocusArea.FocusPoint.TECHNOLOGY, technologyModifier));
        FormatModifierText(addTechnologyButton.GetComponentInChildren<TextMeshProUGUI>(), technologyModifier);
        addDesignBtn.onClick.RemoveAllListeners();
        addDesignBtn.onClick.AddListener(() => IncreaseProgress(FocusArea.FocusPoint.DESIGN, designModifier));
        FormatModifierText(addDesignBtn.GetComponentInChildren<TextMeshProUGUI>(), designModifier);
        addAudioBtn.onClick.RemoveAllListeners();
        addAudioBtn.onClick.AddListener(() => IncreaseProgress(FocusArea.FocusPoint.AUDIO, audioModifier));
        FormatModifierText(addAudioBtn.GetComponentInChildren<TextMeshProUGUI>(), audioModifier);
    }

    private void InitializeJob()
    {
        InitializeModifiers();
        experienceReward.text = new NumberFormatter(_job.GetExperienceReward()).Format();
        slider.value = GetProgress(_job.GetProgress(), _job.GetMaxProgress());
        moneyReward.text = _job.GetMoneyReward().Format();
        progressText.text = _job.GetProgress().Format() + "/" + _job.GetMaxProgress().Format();
        levelText.text = "Level " + _job.GetLevel().ToString();
        nameText.text = _job.GetName();
    }

    private void FormatModifierText(TextMeshProUGUI text, BigRational value)
    {
        string valueText = value.Format();
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

    protected virtual string GetPrefab()
    {
        return "job";
    }

    protected void UpdateJob()
    {
        progressText.text = _job.GetProgress().Format() + "/" + _job.GetMaxProgress().Format();
        if (slider.value >= 100)
        {
            OnJobFinished?.Invoke(this, new JobArgs(_job));
            _currentEntry = -1;
            _player.OnStatChanged -= OnPlayerStatChange;
            _job.OnJobFinished -= OnJobDataFinished;
            _job.OnJobProgressIncreased -= OnJobProgressIncreased;
            _job = null;
            _player = null;
            _isFinished = false;
            _activeObject.transform.localScale = new Vector3(1.8f, 1.8f, 1f);
            ResetAnimation(false);
            _pooler.ReturnToPool(GetPrefab(), _activeObject);
        }
        if (_currentEntry != -1)
        {
            if (slider.value < _currentEntry)
            {
                slider.value += GetFillSpeed() * Time.deltaTime;
            }
            else
            {
                _currentEntry = -1;
            }
        }
        else
        {
            if (_queue.Count > 0)
            {
                _currentEntry = _queue.Dequeue();
            }
        }
    }

    void Update()
    {
        UpdateJob();
    }

    protected void ResetAnimation(bool state)
    {
        Animator animator = _activeObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("show", state);
        }
    }

    public void IncreaseProgress(FocusArea.FocusPoint focus, BigRational value)
    {
        if (!_isFinished)
        {
            if (value > BigRational.Zero && _job.GetProgress() < _job.GetMaxProgress())
            {
                Vector3 newPosition = new Vector3(transform.position.x + 3f, transform.position.y);
                switch (focus)
                {
                    case FocusArea.FocusPoint.TECHNOLOGY:
                        NumberPopupAnimation.Create(newPosition, "+" + value.Format(), 6, Color.red);
                        break;
                    case FocusArea.FocusPoint.DESIGN:
                        NumberPopupAnimation.Create(newPosition, "+" + value.Format(), 6, Color.green);
                        break;
                    case FocusArea.FocusPoint.AUDIO:
                        NumberPopupAnimation.Create(newPosition, "+" + value.Format(), 6, Color.yellow);
                        break;
                    default:
                        break;
                }
                _job.IncreaseProgress(value, false);
                BigRational maxProgress = _job.GetMaxProgress();
                if (_job.GetProgress() > maxProgress)
                {
                    _job.SetProgress(maxProgress, false);
                }
            }
        }
    }

    private void OnJobProgressIncreased(object sender, JobProgressArgs args)
    {
        if (args.Idle)
        {
            slider.value += GetProgress(args.Progress, _job.GetMaxProgress());
        }
        else
        {
            _queue.Enqueue(GetProgress(_job.GetProgress(), _job.GetMaxProgress()));
        }
    }

    // Events
    public event EventHandler<JobArgs> OnJobFinished;
}
