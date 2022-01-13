using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); 

        gm.lcd.ChangeSceneID(false);
        NewGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NewGame()
    {
        for(int i = 0; i < gm.playerLevels.Length; i++)
        {
            gm.playerLevels[i].ReturnToBaseStats(true);
            gm.lcd.returnFromBattle = false;
            gm.ToOverworld();
        }
    }

    void ContinueGame()
    {
        gm.lcd.returnFromBattle = true;
    }
}
