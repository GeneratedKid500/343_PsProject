using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBattleManager : MonoBehaviour
{
    GameManager gm;

    UIHealthBarController healthBarController;
    DisplayInventory di;
    BattleMenuController bmc;

    public GameObject[] playerButtons;
    public GameObject[] enemyButtons;

    StatStorage[] permanentPlayerStats;
    StatStorage[] permanentEnemyStats;

    List<StatStorage> playerStats;
    List<StatStorage> enemyStats;
    List<StatStorage> turnOrder;
    int currentTurn;

    int option = -1;
    int itemOption = -1;
    bool sorted = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        bmc = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<BattleMenuController>();
        healthBarController = bmc.GetComponentInChildren<UIHealthBarController>();
        di = bmc.GetComponentInChildren<DisplayInventory>();

        turnOrder = new List<StatStorage>();

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Player");
        permanentPlayerStats = new StatStorage[temp.Length];
        playerStats = new List<StatStorage>();
        for (int i = 0; i < temp.Length; i++)
        {
            permanentPlayerStats[i] = temp[i].GetComponent<StatStorage>();
            turnOrder.Add(permanentPlayerStats[i]);
        }

        temp = GameObject.FindGameObjectsWithTag("Enemy");
        permanentEnemyStats = new StatStorage[temp.Length];
        enemyStats = new List<StatStorage>();
        for (int i = 0; i < temp.Length; i++)
        {
            permanentEnemyStats[i] = temp[i].GetComponent<StatStorage>();
            enemyStats.Add(permanentEnemyStats[i]);
            turnOrder.Add(enemyStats[i]);
            enemyButtons[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTurn > turnOrder.Count-1)
        {
            currentTurn = 0;
        }

        if (turnOrder[currentTurn].player)
        {
            if (Input.GetKeyDown(KeyCode.Q)) Attack();
            else if (Input.GetKeyDown(KeyCode.W)) Special();
            else if (Input.GetKeyDown(KeyCode.E)) di.OpenInventory();
            else if (Input.GetKeyDown(KeyCode.R)) Flee();
        }

        if (di.GetOpenInventory() && !di.GetSelectedOpen() && Input.GetButtonDown("Circle"))
        {
            di.OpenInventory();
            bmc.CheckPlayerTurn(true);
        }
    }

    void LateUpdate()
    {
        if (currentTurn == 0 && !sorted)
        {
            SortBySpeed();

            int loop = 0;
            for (int i = 0; i < turnOrder.Count; i++)
            {
                if (turnOrder[i].player)
                {
                    playerStats.Add(turnOrder[i]);
                    healthBarController.InstantiateBars(playerStats[loop], loop);
                    loop++;
                }
            }

            healthBarController.SetTurn(turnOrder[currentTurn]);
            sorted = true;
            currentTurn--;
            NextTurn();
        }
    }

    // calculates the amount of damage an attack will do based upon a variety of factors
    public void CalculateAttackPower(StatStorage attacker, StatStorage defender, int moveDamage)
    {
        int calculatedDamage = (int)(((((2 * attacker.GetLevel() / 5) + 2) * moveDamage * attacker.GetAtk() / defender.GetDef()) / 50) + 2);
        Debug.Log("Calculated Damage: " + calculatedDamage);
        if (calculatedDamage < 1) calculatedDamage = 1;

        bmc.SetText("It did " + calculatedDamage + " damage!");

        defender.AdjustHP(calculatedDamage);

        RecieveAttack(attacker, defender);
    }

    void RecieveAttack(StatStorage attacker, StatStorage defender)
    {
        if (defender.GetCurrentHP() <= 0)
        {
            defender.AdjustHP(Mathf.Abs(defender.GetCurrentHP()), true);
            defender.bia.Die();

            RemoveFromLists(defender);
        }
        else
        {
            if (defender.player) healthBarController.UpdateHealthBar(defender);

            defender.bia.GetHurt(defender.player);
        }
    }

    void RemoveFromLists(StatStorage dead)
    {
        turnOrder.Remove(dead);
        if (!dead.player)
        {
            for (int i = 0; i < playerStats.Count; i++)
            {
                playerStats[i].AddStoredExp(dead.ls.CalcExpRewards());
            }

            enemyStats.Remove(dead);

            if (enemyStats.Count < 1)
            {
                gm.ToOverworld(permanentPlayerStats);
            }
        }
        else
        {
            playerStats.Remove(dead);

            if (playerStats.Count < 1)
            {
                gm.ToMainMenu();
            }
        }
    }

    bool AttackWillHit(StatStorage attacker, StatStorage defender)
    {
        int attAcc = (int)Mathf.Clamp(attacker.GetAcc() + 3, 3, 9);
        int defEva = (int)Mathf.Clamp(defender.GetEva() + 3, 3, 9);

        float result = attAcc / defEva;

        if (result >= 1)
        {
            return true;
        }
        else
        {
            int randomChance = Random.Range(0, 100);
            if (result * 100 > randomChance)
            {
                return true;
            }
            return false;
        }
    }


    public void OnOptionSelect(int opt)
    {
        option = opt;

        if (option == 2)
        {
            di.OpenInventory();
        }
    }

    public void OnItemSelect(int index)
    {
        itemOption = index;
    }

    public void OnTargetConfirm(int target)
    {
        if (!permanentEnemyStats[target].GetAlive())
        {
            bmc.SetText("You cannot attack there!");
            currentTurn--;
            NextTurn();
            return;
        }

        switch (option)
        {
            case 0:
                Attack(target);
                break;
            case 1:
                Special(target);
                break;
            case 2:
                Item(target, itemOption);
                break;
        }
    }

    // button pressed by player to do basic attack
    // called by enemy AI
    public void Attack(int targetNumber = -1)
    {
        StatStorage target = TargetSelect(targetNumber);
        if (AttackWillHit(turnOrder[currentTurn], target))
        {
            bmc.SetText(turnOrder[currentTurn].characterName + " used " + turnOrder[currentTurn].ah.SetAttackDefend(turnOrder[currentTurn], target, 'a'));
            turnOrder[currentTurn].bia.Attack(target.transform);
        }
    }

    // button pressed by player to do special attack
    // called by enemy AI
    public void Special(int targetNumber = -1)
    {
        StatStorage target = TargetSelect(targetNumber);
        if (AttackWillHit(turnOrder[currentTurn], target))
        {
            bmc.SetText(turnOrder[currentTurn].characterName + " used " + turnOrder[currentTurn].ah.SetAttackDefend(turnOrder[currentTurn], target, 's'));
            turnOrder[currentTurn].bia.Special(target.transform, turnOrder[currentTurn].moveSpecial);
        }
    }

    // button pressed by player to use item 
    // NOT called by AI
    public void Item(int targetNumber, int itemIndex)
    {
        Debug.Log("Item");
        if (turnOrder[currentTurn].player)
        {
            StatStorage target = TargetSelect(targetNumber, itemIndex);
            // select item
            bmc.SetText(turnOrder[currentTurn].characterName + " used " + di.inventory.itemSlots[itemIndex].itemObject.itemName);

            // use item on selected character
            target.AdjustHP(di.inventory.itemSlots[itemIndex].itemObject.hpValue, di.inventory.itemSlots[itemIndex].itemObject.forHealing);
            di.inventory.RemoveItem(di.inventory.itemSlots[itemIndex].itemObject, 1);
            if (target.player) healthBarController.UpdateHealthBar(target);

            // play animation
            turnOrder[currentTurn].bia.Item();
            itemOption = -1;
        }
    }

    // button pressed by player to do special attack
    // called by enemy AI
    public void Flee()
    {
        Debug.Log("Flee");

        //if (turnOrder[currentTurn].player || !turnOrder[currentTurn].player && turnOrder[currentTurn].GetCurrentHP() < turnOrder[currentTurn].GetMaxHP() / 2)
        //{
            bmc.SetText((turnOrder[currentTurn].characterName + " is trying to flee..."));
            turnOrder[currentTurn].bia.Flee();
        //}
        //else
        //{
        //    bmc.SetText((turnOrder[currentTurn].characterName + " can't make its mind up..."));
        //    Debug.Log("The enemy can't make its mind up!!");
        //    // the enemy struggles to choose its next move...
        //    NextTurn();
        //}
    }

    // called to decide what to do when either side of a battle flees
    // removes enemies and returns to overworld with players
    public void SuccessfulFlee()
    {
        bmc.SetText((turnOrder[currentTurn].name + " successfully fled!"));
        if (turnOrder[currentTurn].player)
        {
            gm.ToOverworld();
        }
        else
        {
            turnOrder[currentTurn].AdjustHP(turnOrder[currentTurn].GetCurrentHP());
            RemoveFromLists(turnOrder[currentTurn]);
        }
    }

    public void NextTurn()
    {
        if (currentTurn < turnOrder.Count - 1) currentTurn++;
        else currentTurn = 0;

        if (!turnOrder[currentTurn].player)
        {
            turnOrder[currentTurn].edm.ActionChoice();
        }
        healthBarController.SetTurn(turnOrder[currentTurn]);
        if (turnOrder[currentTurn].player) bmc.CheckPlayerTurn(turnOrder[currentTurn].player);
    }

    StatStorage TargetSelect(int targetNumber = -1, int itemIndex = -1)
    {
        if (turnOrder[currentTurn].player && targetNumber != -1)
        {
            // if healing item return players
            if (itemIndex != -1)
            {
                if (di.inventory.itemSlots[itemIndex].itemObject.forHealing)
                {
                    return permanentPlayerStats[targetNumber];
                }
            }

            // if attacking / attacking item return enemies
            if (enemyStats.Count > 0)
            {
                return permanentEnemyStats[targetNumber];
            }
            else
            {
                gm.ToOverworld(permanentPlayerStats);
                return null;
            }
        }
        else
        {
            // decision making
            return playerStats[Random.Range(0, playerStats.Count)];
        }
    }

    // bubble sort - ok as there will only be maximum of 8 units on screen at once
    void SortBySpeed()
    {
        int n = turnOrder.Count;

        bool swapped = true;
        while (swapped)
        {
            swapped = false;

            for (int i = 0; i < n - 1; i++)
            {
                if (turnOrder[i].GetSpd() < turnOrder[i + 1].GetSpd())
                {
                    StatStorage temp = turnOrder[i];
                    turnOrder[i] = turnOrder[i + 1];
                    turnOrder[i + 1] = temp;
                    swapped = true;
                }
            }
        }
    }
}
