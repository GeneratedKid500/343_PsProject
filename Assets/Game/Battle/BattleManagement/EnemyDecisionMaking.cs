using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDecisionMaking : MonoBehaviour
{
    GameManager gm;
    PlayBattleManager pbm;
    StatStorage ss;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pbm = GameObject.Find("Center").GetComponent<PlayBattleManager>();
        ss = GetComponent<StatStorage>();
    }

    public void ActionChoice()
    {
        int rand = 0;

        int maxHP = ss.GetMaxHP();
        int currentHP = ss.GetCurrentHP();
        if (currentHP < maxHP / 4)
        {
            rand = Random.Range(0, 4);
        } 
        else if (currentHP < maxHP / 2)
        {
            rand = Random.Range(0, 3);
        }
        else
        {
            rand = Random.Range(0, 2);
        }


        switch (rand)
        {
            case 0:
                pbm.Attack();
                break;

            case 1:
                pbm.Special();
                break;

            default:
                pbm.Flee();
                break;
        }
    }
}
