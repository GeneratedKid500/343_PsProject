using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject PauseFirstSelected;

    ThirdPersonControl player;
    [SerializeField] GameObject inventorySystem;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonControl>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Options") && !inventorySystem.activeInHierarchy)
        {
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
            if (pauseMenu.activeInHierarchy)
            {
                SetNewStartPoint(PauseFirstSelected);
                Time.timeScale = 0;
                player.enabled = false;
            }
            else
            {
                Unpause();
            }
        }
    }

    public void SetNewStartPoint(GameObject ga)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(ga);
    }

    public void Unpause()
    {
        pauseMenu.SetActive(false);
        player.enabled = true;
        Time.timeScale = 1;
    }
}
