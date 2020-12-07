
using System.Numerics;

public interface IRewardProvider
{
    BigInteger GetExperienceReward();
    BigRational GetMoneyReward();
    BigRational GetAwardReward();
}
