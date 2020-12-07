using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanelBase : MonoBehaviour
{
    public UpgradeTooltip tooltip;

    protected void OnEntityLevel(object sender, LevelArgs args)
    {
        IEntity entity = sender as IEntity;
        ILevelRewardProvider provider = sender as ILevelRewardProvider;
        Reward reward = provider.GetManager().NextReward();
        if (reward != null)
        {
            if (entity.Level >= reward.Level)
            {
                ClaimReward(provider);
            }
        }
        else
        {
            entity.OnLevelGained -= OnEntityLevel;
            Destroy(gameObject);
        }
    }

    protected void ClaimReward(ILevelRewardProvider provider)
    {
        Reward reward = provider.GetManager().ClaimReward();
        provider.AddReward(reward);
    }

    public void ShowTooltip(ILevelRewardProvider provider)
    {
        Reward reward = provider.GetManager().NextReward();
        if (reward != null)
        {
            switch (reward.Type)
            {
                case Reward.RewardType.FEATURE:
                case Reward.RewardType.TYPE:
                case Reward.RewardType.INFO:
                    tooltip.Show(this, reward.Name, reward.Description, "Level " + reward.Level, BigInteger.Zero);
                    break;
            }
        }
    }

    public void HideTooltip()
    {
        tooltip.Hide();
    }
}
