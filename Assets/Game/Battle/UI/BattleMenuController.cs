using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class BattleMenuController : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] GameObject OptionSelectMenu;
    [SerializeField] GameObject PlayerSelectMenu, EnemySelectMenu, InventorySelectMenu;
    [SerializeField] GameObject textbox;
    TextMeshProUGUI tbt;

    [Header("FirstSelectOptions")]
    [SerializeField] GameObject FirstOptionSelectMenu;
    [SerializeField] GameObject FirstPlayerSelectMenu, FirstEnemySelectMenu, ClosedSelectMenu;
    [SerializeField] GameObject ClosedInventorySelectMenu;

    int lastChoice;
    bool itemHealing;

    private void Start()
    {
        tbt = textbox.GetComponent<TextMeshProUGUI>();
    }

    void SetSelectedGameObject(ref GameObject ga)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(ga);
    } 

    public void OptionSelect(int choice)
    {
        lastChoice = choice;
        switch (choice) 
        {
            case 0:
            case 1:
                EnemySelectMenu.SetActive(true);
                SetSelectedGameObject(ref FirstEnemySelectMenu);
                break;
            case 2:
                EnemySelectMenu.SetActive(false);
                PlayerSelectMenu.SetActive(false);
                break;
            case 3:
                break;
        }
        OptionSelectMenu.SetActive(false);
    }

    public void GetItemDetails(bool isForHealing)
    {
        itemHealing = isForHealing;
    }

    public void ItemPointerSelector()
    {
        if (itemHealing)
        {
            PlayerSelectMenu.SetActive(true);
            SetSelectedGameObject(ref FirstPlayerSelectMenu);
        }
        else
        {
            EnemySelectMenu.SetActive(true);
            SetSelectedGameObject(ref FirstPlayerSelectMenu);
        }
    }

    public void EnemySelect()
    {
        EnemySelectMenu.SetActive(false);
    }

    public void EnemyCancel()
    {
        OptionSelectMenu.SetActive(true);
        switch (lastChoice)
        {
            case 0:
            default:
                SetSelectedGameObject(ref FirstOptionSelectMenu);
                break;
            case 1:
                SetSelectedGameObject(ref ClosedSelectMenu);
                break;
            case 2:
                SetSelectedGameObject(ref ClosedInventorySelectMenu);
                break;
        }
    }

    public void CheckPlayerTurn(bool check)
    {
        if (check)
        {
            OptionSelectMenu.SetActive(true);
            SetSelectedGameObject(ref FirstOptionSelectMenu);
        }
        else
        {
            OptionSelectMenu.SetActive(false);
            PlayerSelectMenu.SetActive(false);
            EnemySelectMenu.SetActive(false);
            //InventorySelectMenu.SetActive(false);
        }
    }

    public void SetText(string textToDisplay)
    {
        StopAllCoroutines();
        tbt.text = null;
        textbox.SetActive(false);
        tbt.text = textToDisplay;
        StartCoroutine(DisplayText());
    }

    IEnumerator DisplayText()
    {
        textbox.SetActive(true);
        yield return new WaitForSeconds(2);
        textbox.SetActive(false);
    }

}
