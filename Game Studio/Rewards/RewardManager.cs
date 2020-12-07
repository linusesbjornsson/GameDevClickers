
using System.Collections.Generic;

public class RewardManager
{
    private readonly Queue<Reward> _rewards = new Queue<Reward>();
    private IEntity _entity;

    public RewardManager(IEntity entity)
    {
        _entity = entity;
    }

    public Reward NextReward()
    {
        if (_rewards.Count == 0)
        {
            return null;
        }
        return _rewards.Peek();
    }

    public Reward ClaimReward()
    {
        return _rewards.Dequeue();
    }

    public void FilterRewards()
    {
        while (_rewards.Count > 0 && _entity.Level >= NextReward().Level)
        {
            _rewards.Dequeue();
        }
    }

    public void AddReward(Reward reward)
    {
        _rewards.Enqueue(reward);
    }
}