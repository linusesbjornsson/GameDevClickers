using UnityEngine;

public class JobManagerUI : MonoBehaviour
{
    public Player player;
    public ObjectPooler pooler;

    private JobManager manager;

    private void Start()
    {
        manager = new JobManager(player);
        manager.OnJobGenerated += OnJobGenerated;
        manager.Start();
    }

    private void Update()
    {
        manager.Update(Time.deltaTime);
    }

    private void OnJobGenerated(object sender, JobUIArgs args)
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y - 125f - (args.Index * 175f));
        JobUI ui;
        if (args.Job is LegendaryJob)
        {
            ui = LegendaryJobUI.Create(pooler, transform, player, position, args.Job as LegendaryJob);
        }
        else
        {
            ui = JobUI.Create(pooler, transform, player, position, args.Job);
        }
        ui.OnJobFinished += OnJobFinished;
    }

    private void OnJobFinished(object sender, JobArgs job)
    {
        JobUI ui = sender as JobUI;
        ui.OnJobFinished -= OnJobFinished;
        manager.JobFinished(job.Job);
    }
}
