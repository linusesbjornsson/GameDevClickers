
using static GameProject;

public interface IGameStudioElement : IRewardProvider
{
    string GetName();

    float GetMultiplier();

    BigRational GetCost();

    ProjectElementType GetProjectType();
}
