using System.Collections.Generic;
using UnityEngine;
public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class PoolData
    {
        public string key;
        public GameObject prefab;
        public int size;
    }

    public class GameObjectData
    {
        public Vector3 localScale;

        public GameObjectData(Vector3 scale)
        {
            localScale = scale;
        }
    }

    public List<PoolData> pools;
    private Dictionary<string, Queue<GameObject>> _poolSpawn = new Dictionary<string, Queue<GameObject>>();

    public void Start()
    {
        foreach (PoolData data in pools)
        {
            Queue<GameObject> spawn = new Queue<GameObject>();
            for (int i = 0; i < data.size; i++)
            {
                GameObject obj = Instantiate(data.prefab);
                obj.SetActive(false);
                spawn.Enqueue(obj);
            }
            _poolSpawn.Add(data.key, spawn);
        }
    }

    public GameObject InitializeFromPool(string key)
    {
        if (!_poolSpawn.ContainsKey(key))
        {
            return null;
        }
        if (_poolSpawn[key].Count == 0)
        {
            return null;
        }
        GameObject gameObject = _poolSpawn[key].Dequeue();
        gameObject.SetActive(true);
        return gameObject;
    }

    public void ReturnToPool(string key, GameObject gameObject)
    {
        gameObject.SetActive(false);
        _poolSpawn[key].Enqueue(gameObject);
    }
}
