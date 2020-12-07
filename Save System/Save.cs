using UnityEngine;
using static SaveSystem;

public class Save : MonoBehaviour
{
    public Player player;
    public GameStudio studio;

    private float _seconds = 0f;
#if UNITY_WEBGL
    private readonly float MAX_SECONDS = 30f;
#else
    private readonly float MAX_SECONDS = 120f;
#endif

    private void Awake()
    {
        SaveData data = Load();
        if (data != null)
        {
            player.Initialize(data.PlayerData, studio);
            studio.Initialize(data.GameStudioData, player);
        }
    }

    private void OnApplicationQuit()
    {
        Save(player, studio);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save(player, studio);
        }
    }

    private void Update()
    {
        if (_seconds > MAX_SECONDS)
        {
            Save(player, studio);
            _seconds = 0f;
        }
        else
        {
            _seconds += Time.deltaTime;
        }
    }
}
