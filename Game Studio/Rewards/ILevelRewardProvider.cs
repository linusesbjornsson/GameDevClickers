
public interface ILevelRewardProvider
{
    RewardManager GetManager();
    void AddReward(Reward reward);
}
