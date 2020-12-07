using UnityEngine;
using static Job;

public static class JobGenerator
{
    private static readonly JobData[] JOBS = {
        new JobData("Website Developer", new FocusArea[] { new FocusArea(FocusArea.FocusPoint.TECHNOLOGY, 1f)}),
        new JobData("Photoshop Designer", new FocusArea[] { new FocusArea(FocusArea.FocusPoint.DESIGN, 1f)}),
        new JobData("Band Member", new FocusArea[] { new FocusArea(FocusArea.FocusPoint.AUDIO, 1f)}),
        new JobData("Music Producer", new FocusArea[] { new FocusArea(FocusArea.FocusPoint.AUDIO, 1f), new FocusArea(FocusArea.FocusPoint.TECHNOLOGY, 0.5f)}),
        new JobData("Game Tool", new FocusArea[] { new FocusArea(FocusArea.FocusPoint.TECHNOLOGY, 1f)}),
        new JobData("Mobile App", new FocusArea[] { new FocusArea(FocusArea.FocusPoint.TECHNOLOGY, 1f), new FocusArea(FocusArea.FocusPoint.DESIGN, 0.75f), new FocusArea(FocusArea.FocusPoint.AUDIO, 0.25f)}),
        new JobData("Commercial", new FocusArea[] { new FocusArea(FocusArea.FocusPoint.DESIGN, 1f), new FocusArea(FocusArea.FocusPoint.AUDIO, 0.75f)}),
        new JobData("Movie Trailer", new FocusArea[] { new FocusArea(FocusArea.FocusPoint.DESIGN, 1f), new FocusArea(FocusArea.FocusPoint.AUDIO, 0.75f)}),
        new JobData("Cleaner", new FocusArea[] { new FocusArea(FocusArea.FocusPoint.DESIGN, 0.25f), new FocusArea(FocusArea.FocusPoint.AUDIO, 0.25f), new FocusArea(FocusArea.FocusPoint.TECHNOLOGY, 0.25f)}),
        new JobData("Tutor", new FocusArea[] { new FocusArea(FocusArea.FocusPoint.DESIGN, 1f), new FocusArea(FocusArea.FocusPoint.AUDIO, 1f), new FocusArea(FocusArea.FocusPoint.TECHNOLOGY, 1f)}),
        new JobData("Song Release", new FocusArea[] { new FocusArea(FocusArea.FocusPoint.AUDIO, 1f), new FocusArea(FocusArea.FocusPoint.DESIGN, 0.5f)})
    };

    private static readonly JobData[] LEGENDARY_JOBS = {
        new JobData("Open Source Project", new FocusArea[] { new FocusArea(FocusArea.FocusPoint.TECHNOLOGY, 1f)}),
        new JobData("Indie Movie", new FocusArea[] { new FocusArea(FocusArea.FocusPoint.DESIGN, 1f)}),
        new JobData("Song Album", new FocusArea[] { new FocusArea(FocusArea.FocusPoint.AUDIO, 1f)})
    };

    public static Job GenerateJob(Player player)
    {
        Tool infrastructure = player.Tools.GetTool(Tool.ToolID.INFRASTRUCTURE_UPGRADE);
        int access = Random.Range(0, JOBS.Length);
        JobData data = JOBS[access];
        return new Job(player, data.Name, (ulong)infrastructure.GetModifier().WholePart, data.Focus);
    }

    public static Job GenerateLegendaryJob(Player player)
    {
        Tool infrastructure = player.Tools.GetTool(Tool.ToolID.INFRASTRUCTURE_UPGRADE);
        int access = Random.Range(0, LEGENDARY_JOBS.Length);
        JobData data = LEGENDARY_JOBS[access];
        return new LegendaryJob(player, data.Name, (ulong)infrastructure.GetModifier().WholePart, data.Focus);
    }

    private class JobData
    {
        public string Name { get; set; }
        public FocusArea[] Focus { get; set; }

        public JobData(string name, FocusArea[] focus)
        {
            Name = name;
            Focus = focus;
        }
    }
}
