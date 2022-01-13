using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RandomEnemySpawning : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject[] enemies;
    public GameObject waypoint;

    [Header("Spawning Conditions")]
    public float maxSpawnDistance = 10;
    public LayerMask groundLayer;
    private Transform player;

    [Header("Enemy Handling")]
    [SerializeField] int maxEnemies = 5;
    [SerializeField] List<GameObject> activeEnemies;
    [Tooltip("Percentage chance of object spawning each frame")]
    [SerializeField] float randomSpawnChance = 10f;

    [Header("Enemy Waypoint Handling")]
    public float maxWaypointSpawnDistance = 5;
    public int numberOfWaypoints = 5;
    
    private int randomNum;

    void Start()
    {
        player = this.transform;

        activeEnemies = new List<GameObject>();
    }

    void Update()
    {
        if (randomNum < randomSpawnChance && activeEnemies.Count - 1 < maxEnemies)
        {
            float RandomX = Random.Range(player.position.x - maxSpawnDistance, player.position.x + maxSpawnDistance);
            float RandomZ = Random.Range(player.position.z - maxSpawnDistance, player.position.z + maxSpawnDistance);
            Vector3 spawnLoc = new Vector3(RandomX, player.position.y, RandomZ);

            if (GroundCheck(spawnLoc))
            {
                if (player.tag != "Player" && activeEnemies.Count == 0)
                {
                    Spawn(Vector3.zero);
                }
                else
                {
                    GameObject freshSpawn = Spawn(spawnLoc);
                }
            }
        }
    }

    bool GroundCheck(Vector3 spawnLoc)
    {
        return Physics.Raycast(spawnLoc, -Vector3.up, Mathf.Abs(player.position.y) + 2, groundLayer);
    }

    void LateUpdate()
    {
        randomNum = Random.Range(0, 100);
    }

    // spawn function - to be adapted for pooling later
    private GameObject Spawn(Vector3 spawnPos)
    {
        int enemy = Random.Range(0, enemies.Length);
        GameObject spawn = ObjectPooler.Instance.SpawnFromPool(enemies[enemy].name, spawnPos, enemies[enemy].transform.rotation);
        activeEnemies.Add(spawn);
        return spawn;
    }
    // despawn function - to be adapted for pooling later
    private void Despawn(GameObject enemy)
    {
        enemy.SetActive(false);
        activeEnemies.Remove(enemy);
    }

}

#region Editor Drawing
#if UNITY_EDITOR
[CustomEditor(typeof(RandomEnemySpawning))]
public class Draw : Editor
{
    private void OnSceneGUI()
    {
        RandomEnemySpawning res = (RandomEnemySpawning)target;
        Handles.color = new Color(1, 0, 0, 0.1f);
        Vector3 groundPos = new Vector3(res.transform.position.x, 0, res.transform.position.z);

        Handles.DrawSolidDisc(groundPos, res.transform.up, res.maxSpawnDistance);
    }
}
#endif
#endregion



