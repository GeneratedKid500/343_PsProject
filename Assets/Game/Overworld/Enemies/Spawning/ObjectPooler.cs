using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool (string tag, Vector3 pos, Quaternion rot)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject spawnObj = poolDictionary[tag].Dequeue();

        spawnObj.SetActive(true);
        spawnObj.transform.position = pos;
        spawnObj.transform.rotation = rot;

        iPooledObject poolObj = spawnObj.GetComponent<iPooledObject>();
        if (poolObj != null)
        {
            poolObj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(spawnObj);

        return spawnObj;
    }

}
