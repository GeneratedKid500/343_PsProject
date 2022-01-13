using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealthBarController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] Sprite[] portraits;

    [Header("Player Health Bars")]
    [SerializeField] GameObject[] healthBars;

    StatStorage[] playerOrder;
    Slider[] hbSliders;
    Image[] healthBarPortraits;
    TextMeshProUGUI[] healthBarTexts;

    void Awake()
    {
        playerOrder = new StatStorage[healthBars.Length];
        hbSliders = new Slider[healthBars.Length];  

        healthBarPortraits = new Image[healthBars.Length];
        healthBarTexts = new TextMeshProUGUI[healthBars.Length];
        for (int i = 0; i < healthBarPortraits.Length; i++)
        {
            healthBarPortraits[i] = healthBars[i].GetComponentsInChildren<Image>()[3];
            healthBarTexts[i] = healthBars[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void InstantiateBars(StatStorage setPlayerOrder, int loop)
    {
        playerOrder[loop] = setPlayerOrder;
        hbSliders[loop] = healthBars[loop].GetComponentInChildren<Slider>();
        hbSliders[loop].maxValue = setPlayerOrder.GetMaxHP();
        hbSliders[loop].value = setPlayerOrder.GetCurrentHP();
        switch (playerOrder[loop].transform.name)
        {
            case "BattleMando(Clone)":
                healthBarPortraits[loop].sprite = portraits[2];
                healthBarTexts[loop].text = "The Mandalorian";
                break;
            case "BattleRobot(Clone)":
                healthBarPortraits[loop].sprite = portraits[1];
                healthBarTexts[loop].text = "Chica the Robot";
                break;
            case "BattleThanos(Clone)":
                healthBarPortraits[loop].sprite = portraits[3];
                healthBarTexts[loop].text = "Thanos";
                break;
            case "DorothyBattleController(Clone)":
                healthBarPortraits[loop].sprite = portraits[0];
                healthBarTexts[loop].text = "Ramirez";
                break;
        }
    }

    public void SetTurn(StatStorage turn)
    {
        for (int i = 0; i < playerOrder.Length; i++)
        {
            if (playerOrder[i] == turn)
            {
                healthBarPortraits[i].gameObject.SetActive(true);
            }
            else
            {
                healthBarPortraits[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateHealthBar(StatStorage turn)
    {
        for (int i = 0; i < playerOrder.Length; i++)
        {
            if (playerOrder[i] == turn)
            {
                hbSliders[i].value = playerOrder[i].GetCurrentHP();
            }
        }
    }
}
