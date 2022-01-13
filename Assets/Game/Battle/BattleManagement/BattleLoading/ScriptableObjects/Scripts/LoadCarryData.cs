using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="NewLoadCarryData", menuName ="Scriptable Objs/Add CarryLoadData")]
public class LoadCarryData : ScriptableObject
{
    [Header("Assets")]
    [SerializeField] GameObject[] playerOrder;
    [SerializeField] GameObject[] enemyTypes;

    [Header("Scene")]
    [Tooltip("false = overworld, true = battle")]
    [SerializeField] bool sceneID;

    [Header("Overworld")]
    [Tooltip("0 = Main, 1 = Mando, 2 = Robot, 3 = Thanos")] public int currentPlayerModel;
    public Vector3 playerPos;
    public Quaternion playerRot;
    public bool returnFromBattle = false;

    [Header("Battle")]
    [SerializeField] GameObject collidedEnemy;
    [SerializeField] int collidedEnemyMaterialID;
    [SerializeField] int numberOfEnemies;
    [SerializeField] GameObject[] otherEnemies;

    /// <param name="id">false = overworld, true = battle</param>
    public void ChangeSceneID(bool id)
    {
        sceneID = id;
    }
    public void ChangeSceneID()
    {
        sceneID = !sceneID;
    }

    public bool GetSceneID() { return sceneID; }

    /// <param name="modelID">-1 or no input to return current model</param>
    public int CarryCurrentPlayerModel(int modelID = -1)
    {
        if (modelID == -1) return currentPlayerModel;

        return currentPlayerModel = modelID;
    }

    public void SetCarryPlayerPos(Transform player)
    {
        playerPos = player.position;
        playerRot = player.rotation;
    }

    public GameObject GetPartyOrder(int index)
    {
        return playerOrder[index];
    }

    /// <summary>
    /// Sets the first enemy to be the one the player faces
    /// Also generates a random number of enemies to be faced
    /// </summary>
    public void SetCarryCollidedEnemy(Transform enemy)
    {
        switch (enemy.name) 
        {
            case "MushroomMon(Clone)":
                collidedEnemy = enemyTypes[0];
                break;

            case "Polygonal Metalon(Clone)":
                collidedEnemy = enemyTypes[1];
                break;

            case "Slime(Clone)":
                collidedEnemy = enemyTypes[3];
                break;

            case "TurtleShell(Clone)":
                collidedEnemy = enemyTypes[4];
                break;

            case "Wolf(Clone)":
                collidedEnemy = enemyTypes[5];
                break;
        }

        collidedEnemyMaterialID = enemy.GetComponentInChildren<RandomTextureAllocator>().GetMaterialID();

        numberOfEnemies = Random.Range(1, 9);
        SetCarryOtherEnemies();
    }

    public int GetEnemyType(int i)
    {
        for (int j = 0; j < enemyTypes.Length; j++)
        {
            if (otherEnemies[i] == enemyTypes[j])
            {
                return j;
            }
        }
        return -1;
    }

    public GameObject GetCollidedEnemy()
    {
        return collidedEnemy;
    }

    public int GetCollidedEnemyMaterialID()
    {
        return collidedEnemyMaterialID;
    }

    public void SetCarryOtherEnemies()
    {
        otherEnemies = new GameObject[numberOfEnemies];
        otherEnemies[0] = collidedEnemy;
        for (int i = 1; i < otherEnemies.Length; i++)
        {
            int rand = Random.Range(0, enemyTypes.Length - 1);
            otherEnemies[i] = enemyTypes[rand];
        }
    }

    public GameObject GetCarryOtherEnemies(int index)
    {
        return otherEnemies[index];
    }

    /// <summary>
    /// Override to fight specific number of enemies
    /// </summary>
    /// <param name="number">-1 or nothing to return number</param>
    public int GetSetNumberOfEnemies(int number = -1)
    {
        if (number == -1) return numberOfEnemies;

        return numberOfEnemies = number;
    }
}
