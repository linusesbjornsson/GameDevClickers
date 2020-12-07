public class Reward
{
    public ulong Level { get; private set; }
    public object Payload { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public RewardType Type { get; private set; }

    public Reward(ulong level, object payload, string name, string description, RewardType type)
    {
        Level = level;
        Payload = payload;
        Name = name;
        Description = description;
        Type = type;
    }

    public enum RewardType
    {
        TYPE,
        FEATURE,
        UPGRADE,
        INFO
    }
}
