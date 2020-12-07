using UnityEngine;
using UnityEngine.UI;

public class ButtonUnlock : MonoBehaviour
{
    public UnlockableType type;
    public Player player;
    public GameStudio studio;
    public Button button;

    private void Start()
    {
        studio.OnLevelGained += OnStudioLevelGained;
    }

    public void OnStudioLevelGained(object sender, LevelArgs args)
    {
        switch (type)
        {
            case UnlockableType.LAB:
                if (studio.Level >= 25)
                {
                    button.interactable = true;
                }
                break;
        }
    }

    public enum UnlockableType
    {
        LAB
    }
}
