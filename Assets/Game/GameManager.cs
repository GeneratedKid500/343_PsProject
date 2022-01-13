using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public LoadCarryData lcd;

    public LevelSys[] playerLevels;
    public LevelSys[] enemyLevels;

    AudioSource audioS;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        audioS = GetComponent<AudioSource>();
        //ToOverworld();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.RightControl))
        {
            ToOverworld();
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift))
        {
            ToBattle();
        }
#endif
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ToOverworld()
    {
        for(int i = 0; i < enemyLevels.Length; i++)
        {
            enemyLevels[i].ReturnToBaseStats(false);
        }

        for (int i = 0; i < playerLevels.Length; i++)
        {
            playerLevels[i].SetHP(playerLevels[i].GetMaxHP());
        }

        SceneManager.LoadScene("Overworld");
    }

    public void ToOverworld(StatStorage[] newStats)
    {
        for(int i = 0; i < playerLevels.Length; i++)
        {
            playerLevels[i].SetHP(newStats[i].GetCurrentHP());
            playerLevels[i].RestoreStats();
            playerLevels[i].AddExp(newStats[i].GetStoredEXP());
        }

        SceneManager.LoadScene("Overworld");
    }

    public void ToBattle()
    {
        for (int i = 0; i < playerLevels.Length; i++)
        {
            playerLevels[i].SaveStats();
        }
        SceneManager.LoadScene("BattleScene");
    }

    public void PlaySong(AudioClip clip)
    {
        audioS.clip = clip;
        audioS.Play();
    }
}
