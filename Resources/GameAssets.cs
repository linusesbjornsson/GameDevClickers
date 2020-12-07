using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;

    public static GameAssets Instance
    {
        get
        { 
            if (_instance == null)
            {
                _instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            }
            return _instance;
        }
    }

    public Transform pfNumberPopup;
    public Transform pfProjectTemplate;
    public Transform pfTool;
    public Transform pfEmployee;
    public Transform pfGameProject;
}
