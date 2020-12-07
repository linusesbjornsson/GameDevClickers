using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : ProgressBarBase
{
    public TextMeshProUGUI _experience;
    public TextMeshProUGUI _currentLevel;
    public TextMeshProUGUI _nextLevel;
    public Entity entity;
    protected IEntity _entity;
    private BarEntry currentEntry;
    private Queue<BarEntry> queue;

    public bool EnableFloatingText = true;

    private void Awake()
    {
        OnAwake();
        _entity = entity;
    }

    protected void OnAwake()
    {
        slider = gameObject.GetComponent<Slider>();
        currentEntry = null;
        queue = new Queue<BarEntry>();
    }

    protected void OnStart()
    {
        UpdateExperienceFields();
        UpdateLevelFields();
        if (_entity != null)
        {
            _entity.OnExperienceGained += OnExperienceGained;
            _entity.OnLevelGained += OnLevelGained;
        }
        slider.value = GetProgress(new BigRational(_entity.ExperiencePoints), new BigRational(_entity.ExperienceToLevel));
    }

    // Start is called before the first frame update
    void Start()
    {
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEntry != null)
        {
            if (currentEntry.Type == BarEntry.QueueType.EXPERIENCE)
            {
                if (slider.value < currentEntry.Progress)
                {
                    slider.value += GetFillSpeed() * Time.deltaTime;
                }
                else
                {
                    currentEntry = null;
                }
            }
            else
            {
                Vector3 newPosition = new Vector3(transform.position.x - 2f, transform.position.y + 0.5f);
                if (EnableFloatingText)
                {
                    NumberPopupAnimation.Create(newPosition, "LEVEL UP!", 10);
                }
                slider.value = 0;
                UpdateLevelFields(currentEntry.Level);
                currentEntry = null;
            }
        }
        else
        {
            if (queue.Count > 0)
            {
                currentEntry = queue.Dequeue();
            }
        }
    }

    void UpdateExperienceFields()
    {
        _experience.text = new NumberFormatter(_entity.ExperiencePoints).Format() + "/" + new NumberFormatter(_entity.ExperienceToLevel).Format();
    }

    void UpdateLevelFields()
    {
        UpdateLevelFields(_entity.Level);
    }

    void UpdateLevelFields(System.Numerics.BigInteger level)
    {
        _currentLevel.text = level.ToString();
        _nextLevel.text = (level + 1).ToString();
    }

    void OnExperienceGained(object sender, ExperienceArgs args)
    {
        Vector3 newPosition = new Vector3(transform.position.x + 1.75f, transform.position.y + 0.5f);
        if (EnableFloatingText)
        {
            NumberPopupAnimation.Create(newPosition, "+" + (args.NewExperience - args.OldExperience).ToString(), 6, Color.green);
        }
        queue.Enqueue(new BarEntry(GetProgress(new BigRational(_entity.ExperiencePoints), new BigRational(_entity.ExperienceToLevel)), BarEntry.QueueType.EXPERIENCE));
        UpdateExperienceFields();
    }

    void OnLevelGained(object sender, LevelArgs args)
    {
        queue.Enqueue(new BarEntry(args.NewLevel, BarEntry.QueueType.LEVEL));
    }

    private class BarEntry
    {
        public float Progress { get; set; }
        public System.Numerics.BigInteger Level { get; set; }
        public QueueType Type { get; set; }

        public enum QueueType
        {
            LEVEL,
            EXPERIENCE
        }

        public BarEntry(System.Numerics.BigInteger value, QueueType type)
        {
            Progress = 0f;
            Level = value;
            Type = type;
        }

        public BarEntry(float value, QueueType type)
        {
            Progress = value;
            Level = System.Numerics.BigInteger.Zero;
            Type = type;
        }
    }
}
