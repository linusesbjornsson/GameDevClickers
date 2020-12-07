using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static SaveData _cache = null;

#if UNITY_WEBGL
    public static void Save(Player player, GameStudio studio)
    {
        PlayerData playerData = new PlayerData(player);
        GameStudioData gameStudioData = new GameStudioData(studio);
        SaveData data = new SaveData
        {
            Version = 1,
            Date = System.DateTime.Now.ToBinary().ToString(),
            PlayerData = playerData,
            GameStudioData = gameStudioData
        };
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("save", json);
        _cache = null;
    }

    public static SaveData Load()
    {
        if (_cache == null)
        {
            if (PlayerPrefs.HasKey("save"))
            {
                string json = PlayerPrefs.GetString("save");
                if (!json.Equals("{}"))
                {
                    SaveData save = JsonUtility.FromJson<SaveData>(json);
                    _cache = save;
                }
            }
        }
        return _cache;
    }
#else
    public static void Save(Player player, GameStudio studio)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.sav";
        FileStream file = new FileStream(path, FileMode.Create);
        PlayerData playerData = new PlayerData(player);
        SaveData data = new SaveData
        {
            Version = 1,
            Date = System.DateTime.Now.ToBinary().ToString(),
            PlayerData = playerData
        };
        formatter.Serialize(file, data);
        _cache = null;
        file.Close();
    }

    public static SaveData Load()
    {
        if (_cache == null)
        {
            string path = Application.persistentDataPath + "/player.sav";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = new FileStream(path, FileMode.Open);
                SaveData save = formatter.Deserialize(file) as SaveData;
                file.Close();
                _cache = save;
            }
        }
        return _cache;
    }
#endif

    [System.Serializable]
    public class SaveData
    {
        public ulong Version;
        public string Date;
        public PlayerData PlayerData;
        public GameStudioData GameStudioData;
    }
}
