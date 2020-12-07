using System;
using UnityEngine;

public class LegendaryJobUI : JobUI
{
    public static new JobUI Create(ObjectPooler pooler, Transform parent, Player player, Vector3 position, Job job)
    {
        GameObject jobObject = pooler.InitializeFromPool("legendaryjob");
        jobObject.transform.SetParent(parent, false);
        jobObject.transform.localPosition = position;
        jobObject.transform.rotation = Quaternion.identity;
        JobUI jobUi = jobObject.transform.Find("Job Data").GetComponent<LegendaryJobUI>();
        jobUi.Setup(pooler, jobObject, player, job);
        return jobUi;
    }

    protected override string GetPrefab()
    {
        return "legendaryjob";
    }
}
