using System;
using System.Collections.Generic;

public class JobManager
{
    private Player _player;
    private uint slots = 1;
    private List<ActiveJobData> _activeJobs = new List<ActiveJobData>();

    private static float LEGENDARY_JOB_CHANCE_PERCENTAGE = 1f;

    // OvertimeTool
    private float _seconds;
    private bool _autoGenerate;
    private readonly float MAX_SECONDS = 60f;

    public JobManager(Player player, bool autoGenerate = false)
    {
        _player = player;
        _autoGenerate = autoGenerate;
    }

    public void Start()
    {
        UpdateSlots();
    }

    public void Update(float time)
    {
        UpdateSlots();
        IdleUpdateJobs(time);
        // OvertimeTool
        Tool overtime = _player.Tools.GetTool(Tool.ToolID.OVERTIME);
        if (overtime != null)
        {
            if (_seconds > MAX_SECONDS)
            {
                bool instantComplete = UnityEngine.Random.Range(0f, 100f) <= (float) overtime.GetModifier();
                if (instantComplete)
                {
                    int index = UnityEngine.Random.Range(0, _activeJobs.Count - 1);
                    Job activeJob = _activeJobs[index].ActiveJob;
                    activeJob.SetProgress(activeJob.GetMaxProgress(), true);
                }
                _seconds = 0;
            }
            else
            {
                _seconds += time;
            }
        }
    }

    private void IdleUpdateJobs(float time)
    {
        UpdateSpecificJobTool(time, _player.Tools.GetTool(Tool.ToolID.SCRIPT), 0);
        UpdateSpecificJobTool(time, _player.Tools.GetTool(Tool.ToolID.ONLINE_TUTORIALS), 1);
        UpdateSpecificJobTool(time, _player.Tools.GetTool(Tool.ToolID.OUTSOURCING), 2);
        UpdateSpecificJobTool(time, _player.Tools.GetTool(Tool.ToolID.HARDWORKER), 3);
        UpdateSpecificJobTool(time, _player.Tools.GetTool(Tool.ToolID.PAIR_PROGRAMMER), 4);
        UpdateUpgradeModifier(time, _player.Tools.GetTool(Tool.ToolID.TECHNICIAN), _player.Technology);
        UpdateUpgradeModifier(time, _player.Tools.GetTool(Tool.ToolID.CRAFTSMAN), _player.Design);
        UpdateUpgradeModifier(time, _player.Tools.GetTool(Tool.ToolID.PRODUCER), _player.Audio);
        Tool expertTool = _player.Tools.GetTool(Tool.ToolID.EXPERT);
        if (expertTool != null)
        {
            ActiveJobData[] shallow = _activeJobs.ToArray();
            foreach (ActiveJobData job in shallow)
            {
                job.ActiveJob.IncreaseProgress(BigRational.Multiply(expertTool.GetModifier(), new BigRational(time)), true);
            }
        }
    }

    private void UpdateSpecificJobTool(float time, Tool tool, int index)
    {
        if (tool != null)
        {
            if (_activeJobs.Count > index)
            {
                Job job = GetJobFromIndex(index);
                job.IncreaseProgress(BigRational.Multiply(tool.GetModifier(), new BigRational(time)), true);
            }
        }
    }

    private void UpdateUpgradeModifier(float time, Tool tool, BigRational value)
    {
        if (tool != null)
        {
            ActiveJobData[] shallow = _activeJobs.ToArray();
            foreach (ActiveJobData job in shallow)
            {
                job.ActiveJob.IncreaseProgress(value * tool.GetModifier() * new BigRational(time), true);
            }
        }
    }

    private Job GetJobFromIndex(int index)
    {
        foreach (ActiveJobData job in _activeJobs)
        {
            if (job.Index == index)
            {
                return job.ActiveJob;
            }
        }
        return null;
    }

    private void UpdateSlots()
    {
        Tool toolUpgrade = _player.Tools.GetTool(Tool.ToolID.TOOL_UPGRADE);
        float slotLimit = (float) toolUpgrade.GetModifier();
        while (slots < slotLimit)
        {
            slots++;
        }
        if (_activeJobs.Count < slots)
        {
            for (int i = _activeJobs.Count; i < slots; i++)
            {
                Job job = GenerateJob(i);
                _activeJobs.Add(new ActiveJobData(job, i));
            }
        }
    }

    public void JobFinished(Job job)
    {
        _player.ExperiencePoints += job.GetExperienceReward();
        _player.Money += job.GetMoneyReward();
        for (int i = _activeJobs.Count - 1; i >= 0; i--)
        {
            ActiveJobData jobData = _activeJobs[i];
            if (jobData.ActiveJob == job)
            {
                Job newJob = GenerateJob(jobData.Index);
                _activeJobs[i] = new ActiveJobData(newJob, jobData.Index);
            }
        }
    }

    private Job GenerateJob(int index)
    {
        Job newJob;
        float legendaryChance = LEGENDARY_JOB_CHANCE_PERCENTAGE;
        if (_player.Tools.GetTool(Tool.ToolID.STARTUP_KING) != null)
        {
            Tool startupKing = _player.Tools.GetTool(Tool.ToolID.STARTUP_KING);
            legendaryChance += (float)startupKing.GetModifier();
        }
        bool generateLegendary = UnityEngine.Random.Range(0f, 100f) <= legendaryChance;
        if (generateLegendary)
        {
            newJob = JobGenerator.GenerateLegendaryJob(_player);
        }
        else
        {
            newJob = JobGenerator.GenerateJob(_player);
        }
        if (_autoGenerate)
        {
            newJob.OnJobFinished += AutoGenerate;
        }
        OnJobGenerated?.Invoke(this, new JobUIArgs(newJob, index));
        return newJob;
    }

    private void AutoGenerate(object sender, JobArgs args)
    {
        Job job = sender as Job;
        job.OnJobFinished -= AutoGenerate;
        JobFinished(args.Job);
    }

    private class ActiveJobData
    {
        public Job ActiveJob { get; private set; }
        public int Index { get; private set; }

        public ActiveJobData(Job activeJob, int index)
        {
            ActiveJob = activeJob;
            Index = index;
        }
    }

    // Events
    public EventHandler<JobUIArgs> OnJobGenerated;
}
