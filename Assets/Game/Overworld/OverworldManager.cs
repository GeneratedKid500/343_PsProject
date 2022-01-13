using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldManager : MonoBehaviour
{
    GameManager gm;

    public GameObject[] playerModels;
    private GameObject player;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");

        gm.lcd.ChangeSceneID(false);
        if (gm.lcd.returnFromBattle)
        {
            gm.lcd.returnFromBattle = false;
            player.transform.position = gm.lcd.playerPos;
            player.transform.rotation = gm.lcd.playerRot;
        }
    }
}
