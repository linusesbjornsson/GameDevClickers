using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveSystem;

public class OfflineProgressionSystem : MonoBehaviour
{
    public Player player;
    // UI
    public TextMeshProUGUI text;
    public TextMeshProUGUI timeLeft;
    public Button interactableButton;
    public GameObject completion;
    public GameObject calculating;
    public GameObject mainUI;
    public ProgressBarBase progressBar;

    private JobManager _manager;
    private float _seconds;
    private float _progressSeconds = 0f;
    private BigRational _startMoney = BigRational.Zero;
    private ulong _startLevel = 0;

    private static readonly int HOUR_IN_SECONDS = 3600;
    private static readonly int HALF_HOUR_IN_SECONDS = HOUR_IN_SECONDS / 2;
    private static readonly int THREE_HOURS_IN_SECONDS = HOUR_IN_SECONDS * 3;
    private static readonly int HALF_DAY_IN_SECONDS = HOUR_IN_SECONDS * 12;

    private static readonly float UPDATE_TICK_HALF_HOUR = 1f;
    private static readonly float UPDATE_TICK_HOUR = 5f;
    private static readonly float UPDATE_TICK_THREE_HOUR = 10f;
    private static readonly float UPDATE_TICK_TWELVE_HOUR = 50f;

    private void Start()
    {
        _manager = new JobManager(player, true);
        CheckOfflineProgress();
    }

    private void CheckOfflineProgress()
    {
        if (HasIdleTools())
        {
            SaveData data = Load();
            if (data != null)
            {
                long temp = Convert.ToInt64(data.Date);
                DateTime time = DateTime.FromBinary(temp);
                DateTime currentTime = DateTime.Now;
                TimeSpan diff = currentTime.Subtract(time);
                _seconds = Convert.ToInt64(diff.TotalSeconds);
                if (_seconds >= HALF_DAY_IN_SECONDS)
                {
                    _seconds = HALF_DAY_IN_SECONDS;
                }
                if (_seconds >= 60)
                {
                    _startMoney = player.Money;
                    _startLevel = player.Level;
                    _manager.Start();
                    timeLeft.text = GetRemainingSeconds(_seconds - _progressSeconds);
                }
                else
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }
        else
        {
            Close();
        }
    }

    public void Close()
    {
        mainUI.SetActive(true);
        gameObject.SetActive(false);
    }

    private string GetRemainingSeconds(float seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);
        string result = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                        t.Hours,
                        t.Minutes,
                        t.Seconds);
        return result;
    }

    private bool HasIdleTools()
    {
        return player.Tools.GetTool(Tool.ToolID.SCRIPT) != null
            || player.Tools.GetTool(Tool.ToolID.CRAFTSMAN) != null
            || player.Tools.GetTool(Tool.ToolID.PRODUCER) != null
            || player.Tools.GetTool(Tool.ToolID.TECHNICIAN) != null
            || player.Tools.GetTool(Tool.ToolID.ONLINE_TUTORIALS) != null
            || player.Tools.GetTool(Tool.ToolID.EXPERT) != null
            || player.Tools.GetTool(Tool.ToolID.HARDWORKER) != null
            || player.Tools.GetTool(Tool.ToolID.PAIR_PROGRAMMER) != null
            || player.Tools.GetTool(Tool.ToolID.OUTSOURCING) != null;
    }

    private void Update()
    {
        if (_progressSeconds < _seconds)
        {
            if (_seconds <= HALF_HOUR_IN_SECONDS)
            {
                _manager.Update(UPDATE_TICK_HALF_HOUR);
                _progressSeconds += UPDATE_TICK_HALF_HOUR;
            }
            else if (_seconds <= HOUR_IN_SECONDS)
            {
                _manager.Update(UPDATE_TICK_HOUR);
                _progressSeconds += UPDATE_TICK_HOUR;
            }
            else if (_seconds <= THREE_HOURS_IN_SECONDS)
            {
                _manager.Update(UPDATE_TICK_THREE_HOUR);
                _progressSeconds += UPDATE_TICK_THREE_HOUR;
            }
            else
            {
                _manager.Update(UPDATE_TICK_TWELVE_HOUR);
                _progressSeconds += UPDATE_TICK_TWELVE_HOUR;
            }
            timeLeft.text = GetRemainingSeconds(_seconds - _progressSeconds);
            progressBar.slider.value = progressBar.GetProgress(new BigRational(_progressSeconds), new BigRational(_seconds));
            if (_progressSeconds >= _seconds)
            {
                BigRational money = player.Money - _startMoney;
                ulong level = player.Level - _startLevel;
                if (money > BigRational.Zero || level > 0)
                {
                    completion.SetActive(true);
                    calculating.SetActive(false);
                    text.text = "You were offline for " + GetRemainingSeconds(_seconds) + " and gained " + money.Format() + " money and " + level + " levels";
                    interactableButton.interactable = true;
                }
                else
                {
                    Close();
                }
            }
        }
    }
}
