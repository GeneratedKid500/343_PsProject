using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleSetup : MonoBehaviour
{
    GameManager gm;
    public PlayBattleManager pbm;

    [Header("Stations")]
    [SerializeField] Transform[] playerStations;
    [SerializeField] Transform[] enemyStations;

    private int currentEnemies = 0;
    private int bonusEnemies = 0;
    private int currentWave = 0;

    GameObject[] playerList;
    GameObject[] currentEnemyList;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        pbm = GetComponent<PlayBattleManager>();
        pbm.enabled = false;

        gm.lcd.ChangeSceneID(true);
        gm.lcd.returnFromBattle = true;

        for (int i = 0; i < playerStations.Length; i++)
        {
            playerStations[i].GetComponent<MeshRenderer>().enabled = false;
        }

        for (int i = 0; i < enemyStations.Length; i++)
        {
            enemyStations[i].GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void Start()
    {
        LoadPlayers();

        SetEnemyPlatforms(gm.lcd.GetSetNumberOfEnemies());

        LoadEnemies();

        pbm.enabled = true;
    }

    void Update()
    {
    }

    void SetEnemyPlatforms(int temp)
    {
        if (temp > 4)
        {
            bonusEnemies = temp - 4;
        }
        currentEnemies = temp -= bonusEnemies;

        switch (currentEnemies)
        {
            case 1:
                enemyStations[0].position = new Vector3(3, 0.01f, 0);
                for (int i = 1; i < enemyStations.Length; i++)
                {
                    enemyStations[i].gameObject.SetActive(false);
                }
                break;

            case 2:
                enemyStations[0].position = new Vector3(3, 0.01f, 1);
                enemyStations[1].position = new Vector3(3, 0.01f, -1);
                for (int i = 2; i < enemyStations.Length; i++)
                {
                    enemyStations[i].gameObject.SetActive(false);
                }
                break;

            case 3:
                enemyStations[0].position = new Vector3(3, 0.01f, 2);
                enemyStations[1].position = new Vector3(3, 0.01f, 0);
                enemyStations[2].position = new Vector3(3, 0.01f, -2);
                for (int i = 3; i < enemyStations.Length; i++)
                {
                    enemyStations[i].gameObject.SetActive(false);
                }
                break;

            case 4:
                enemyStations[0].position = new Vector3(3, 0.01f, 3);
                enemyStations[1].position = new Vector3(3, 0.01f, 1);
                enemyStations[2].position = new Vector3(3, 0.01f, -1);
                enemyStations[3].position = new Vector3(3, 0.01f, -3);
                break;

            default:
                Debug.Log("X" + temp);
                return;
        }
    }

    void LoadEnemies()
    {
        int playerLevel = gm.playerLevels[0].GetLevel();

        currentEnemyList = new GameObject[currentEnemies];
        currentWave++;
        // add support for multiple waves

        for (int i = 0; i < currentEnemyList.Length; i++)
        {
            currentEnemyList[i] = Instantiate(gm.lcd.GetCarryOtherEnemies(i), enemyStations[i]);
            if (i == 0 && currentWave == 1) currentEnemyList[0].GetComponentInChildren<RandomTextureAllocator>().MaterialOverride(gm.lcd.GetCollidedEnemyMaterialID());
            gm.enemyLevels[gm.lcd.GetEnemyType(i)].SetLevel(Random.Range(playerLevel - 1, playerLevel + 1));
            currentEnemyList[i].GetComponent<StatStorage>().ls = gm.enemyLevels[gm.lcd.GetEnemyType(i)];
        }
    }

    void LoadPlayers()
    {
        playerList = new GameObject[playerStations.Length];
        for (int i = 0; i < playerStations.Length; i++)
        {
            playerList[i] = Instantiate(gm.lcd.GetPartyOrder(i), playerStations[i]);
        }
    }
}
