using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelSys", menuName = "Scriptable Objs/Add New Level System")]
public class LevelSys : ScriptableObject
{
    // var
    [Header("State")]
    [SerializeField] bool alive = true;

    [Header("Level")]
    [SerializeField] int level;
    private int maxLevel = 100;

    [Header("EXP")]
    [SerializeField] int currentExp;
    [SerializeField] int nextLevelExp;

    [Header("HP")]
    [SerializeField] int baseMaxHP;
    [SerializeField] int maxHP;
    [SerializeField] int currentHP;

    [Header("Stats")]
    [SerializeField] int baseAtk;
    private float atk;
    private float regAtk;
    [SerializeField] int baseDef;
    private float def;
    private float regDef;
    [SerializeField] float baseSpd;
    private float spd;
    private float regSpd;
    private int accuracy = 0;
    private int evasiveness = 0;

    // func
    public int GetLevel() { return level; }
    public int GetCurExp() { return currentExp; }
    public int GetExpToNextLevel() { return nextLevelExp - currentExp; }
    public int GetMaxHP() { return maxHP; }
    public int GetCurrentHP() { return currentHP;}
    public float GetAtk() { return atk; }
    public float GetDef() { return def; }
    public float GetSpd() { return spd; }

    /// <returns>Alive state</returns>
    private bool SetAlive()
    {
        if (currentHP <= 0)
        {
            alive = false;
        }
        else
        {
            alive = true;
        }
        return alive;
    }

    public int SetLevel(int newLevel)
    {
        level = newLevel;
        return level;
    }

    /// <returns>New level value</returns>
    private int LevelUp()
    {
        if (level != maxLevel)
        {
            level++;
            IncreaseAllStats();
        }
        return level;
    }

    /// <returns>Exp required for new level</returns>
    public int AddExp(int exp)
    {
        currentExp += exp;
        if (currentExp >= nextLevelExp)
        {
            int overflowExp = currentExp - nextLevelExp;

            currentExp = 0;
            nextLevelExp = CalcNextLevelExp();
            if (overflowExp > 0)
            {
                currentExp += overflowExp;
                if (currentExp > nextLevelExp) AddExp(0);
            }

            LevelUp();
        }
        return nextLevelExp - currentExp;
    }

    /// <returns>the exp necessary to reach the next level, scaled by level</returns>
    int CalcNextLevelExp()
    {
        int baseExpNeeded = 100;
        for (int i = 1; i < level; i++)
        {
            baseExpNeeded += 10;
        }
        return baseExpNeeded; 
    }

    /// <returns>the exp reward for defeating this enemy, randomised and scaled by level</returns>
    public int CalcExpRewards()
    {
        int baseExpReward = 20;
        for (int i = 0; i < level; i++)
        {
            baseExpReward += Random.Range(2, 5);
        }
        return baseExpReward;
    }

    public int SetHP(int newHP)
    {
        currentHP = newHP;
        return currentHP;
    }

    /// <returns>stat permanently increased based on level OR -1 if incorrect char is passed</returns>
    int CalcStatLevelIncrease(char statName)
    {
        float temp = 0;
        switch (statName)
        {
            case 'h':
            case 'H':
                temp = maxHP;
                break;
            case 'a':
            case 'A':
                temp = atk;
                break;
            case 'd':
            case 'D':
                temp = def;
                break;
            case 's':
            case 'S':
                temp = spd;
                break;
        }

        int increase = 3;
        increase = Random.Range(increase - 2, increase + 2);
        if (statName == 'h' || statName == 'H')
        {
            temp += increase;
        }
        temp += increase;

        int newTemp = Mathf.RoundToInt(temp);

        switch (statName)
        {
            case 'h':
            case 'H':
                maxHP = newTemp;
                return maxHP;
            case 'a':
            case 'A':
                atk = temp;
                return Mathf.RoundToInt(atk);
            case 'd':
            case 'D':
                def = temp;
                return Mathf.RoundToInt(def);
            case 's':
            case 'S':
                spd = temp;
                return Mathf.RoundToInt(spd);
        }

        Debug.LogError("INCORRECT CHARACTER PASSED THROUGH: LevelUp");
        return -1;
    }

    void IncreaseAllStats()
    {
        CalcStatLevelIncrease('h');
        CalcStatLevelIncrease('a');
        CalcStatLevelIncrease('d');
        CalcStatLevelIncrease('s');
    }

    public void IncreaseStatsByLevel()
    {
        for (int i = 1; i < level; i++)
        {
            IncreaseAllStats();
        }
    }

    public void ReturnToBaseStats(bool levelReset = true)
    {
        if (levelReset)
        {
            level = 1;
            //currentExp = 0;
        }
        alive = true;

        maxHP = baseMaxHP;
        atk = baseAtk;
        def = baseDef;
        spd = baseSpd;

        accuracy = 0;
        evasiveness = 0;
    }

    public void SaveStats()
    {
        regAtk = (int)atk;
        regDef = (int)def;
        regSpd = (int)spd;
    }

    public void RestoreStats()
    {
        atk = regAtk;
        def = regDef;
        spd = regSpd;
    }
}
