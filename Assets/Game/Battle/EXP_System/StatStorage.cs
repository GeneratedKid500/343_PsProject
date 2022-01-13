using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatStorage : MonoBehaviour
{
    [Header("Class Stats")]
    public string characterName;
    public bool player;
    public bool moveSpecial = true;

    [HideInInspector] public EnemyDecisionMaking edm;
    [HideInInspector] public BattleInputAnimators bia;
    [HideInInspector] public LevelSys ls;
    [HideInInspector] public AttackHit ah;

    bool alive = true;
    int level;
    int storedExp = 0;
    int currentHP;
    int maxHP;
    float atk;
    float atkDivBounds;
    float def;
    float defDivBounds;
    float spd;
    float spdDivBounds;

    int accuracy = 0;
    int evasiveness = 0;

    void Start()
    {
        bia = GetComponent<BattleInputAnimators>();
        ah = GetComponentInChildren<AttackHit>();

        if (!player)
        {
            edm = GetComponent<EnemyDecisionMaking>();
            ls.ReturnToBaseStats(false);
        }
        SetStats();
    }

    void SetStats()
    {
        if (!player) ls.IncreaseStatsByLevel();

        level = ls.GetLevel();
        maxHP = ls.GetMaxHP();
        if (!player) currentHP = maxHP;
        else currentHP = ls.GetCurrentHP();
        atk = ls.GetAtk();
        def = ls.GetDef();
        spd = ls.GetSpd();

        atkDivBounds = (int)atk / 5;
        defDivBounds = (int)def / 5;
        spdDivBounds = (int)spd / 5;
    }

    /// <returns>Alive state</returns>
    public bool SetAlive()
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

    /// <returns>Adjusted HP Value</returns>
    public int AdjustHP(int damage, bool heal = false)
    {
        if (!heal)
        {
            currentHP -= damage;
        }
        else
        {
            if (currentHP < maxHP)
            {
                currentHP += damage;
                if (currentHP > maxHP)
                {
                    currentHP = maxHP;
                }
            }
        }
        SetAlive();
        return currentHP;
    }

    /// <returns>adjusted stat value OR -1 if incorrect char is passed</returns>
    public float AdjustStat(char statName, bool dir = false)
    {
        float temp = 0;
        float tempBounds = 0;

        switch (statName)
        {
            case 'a':
            case 'A':
                temp = atk;
                tempBounds = atkDivBounds;
                break;
            case 'd':
            case 'D':
                temp = def;
                tempBounds = defDivBounds;
                break;
            case 's':
            case 'S':
                temp = spd;
                tempBounds = spdDivBounds;
                break;
        }

        if (dir)
        {
            temp += tempBounds;
            if (temp > tempBounds * 10)
            {
                temp = tempBounds * 10;
            }
        }
        else
        {
            temp -= tempBounds;
            if (temp < tempBounds)
            {
                temp = tempBounds;
            }
        }

        switch (statName)
        {
            case 'a':
            case 'A':
                atk = temp;
                return atk;
            case 'd':
            case 'D':
                def = temp;
                return def;
            case 's':
            case 'S':
                spd = temp;
                return spd;
        }

        Debug.LogError("INCORRECT CHARACTER PASSED THROUGH: AtkDefSpd: Storage Script");
        return -1;
    }

    /// <summary>
    /// Adjusts Accuracy and Evasion vars from -5 to 5
    /// </summary>
    /// <returns>Adjusted stat value OR -1 if incorrect char is passed</returns>
    public int AdjustDodge(char statName, bool dir = false)
    {
        int temp = 0;
        switch (statName)
        {
            case 'a':
            case 'A':
                temp = accuracy;
                break;

            case 'e':
            case 'E':
                temp = evasiveness;
                break;
        }

        if (dir)
        {
            temp++;
            if (temp >= 6)
                temp = 6;
        }
        else
        {
            temp--;
            if (temp <= -6)
                temp = -6;
        }

        switch (statName)
        {
            case 'a':
            case 'A':
                accuracy = temp;
                return accuracy;

            case 'e':
            case 'E':
                evasiveness = temp;
                return evasiveness;
        }
        Debug.LogError("INCORRECT CHARACTER PASSED THROUGH: EvaAcc: Storage Script");
        return -1;
    }

    public void AddStoredExp(int exp)
    {
        storedExp += exp;
    }

    public bool GetAlive() { return alive; }
    public int GetLevel() { return level; }
    public int GetMaxHP() { return maxHP; }
    public int GetCurrentHP() { return currentHP; }
    public int GetStoredEXP() { return storedExp; }
    public float GetAtk() { return atk; }
    public float GetDef() { return def; }
    public float GetSpd() { return spd; }
    public float GetAcc() { return accuracy; }
    public float GetEva() { return evasiveness; }
}
