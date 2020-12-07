using System.Numerics;
using UnityEngine;

public class GameStudioRewardPanel : RewardPanelBase
{
    public GameStudio studio;

    private void Start()
    {
        studio.OnLevelGained += OnEntityLevel;
    }

    public void ShowTooltip()
    {
        ShowTooltip(studio);
    }
}
