using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    public bool GamePaused;

    public AudioSystem audioSystem;

    public List<Item> inventory;

    public Transform popUpCanvas;
    public GameObject popUpPrefab;

    public Transform menuCanvas;
    public GameObject MainMenuPrefab;
    public GameObject currentMainMenu;

    public GameObject GameOverScreen;

    [SerializeField] private float MasterVolume;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            inventory = new List<Item>();
            audioSystem.playTheme("title");
        }
        else
        {
            Destroy(gameObject);
        }

    }
   
    public void UnPauseGame()
    {
        GamePaused = false;
        PlayerController.instance.LockedCharacter = false;
    }
    public void PauseGame()
    {
        GamePaused = true;
        PlayerController.instance.LockedCharacter = true;
    }
    public bool hasItemInInventory(string itemID)
    {
        for(int i=0; i < instance.inventory.Count; i++)
        {
            if(instance.inventory[i].stats.itemID == itemID)
            {
                return true;
            }
        }
        return false;
    }
    public void AddItemToInventory(Item item)
    {
        inventory.Add(item);
    }
    public void RemoveItemFromInventory(Item item)
    {
        inventory.Remove(item);
    }
    public void CreateUpgradePopUp(string text)
    {
        GameObject pop = Instantiate(popUpPrefab, popUpCanvas);
        pop.GetComponent<UpgradeInfoPopUp>().SetUp(text);
    }

    public void RequestMainMenu()
    {
        if(currentMainMenu == null)
        {
            PauseGame();
            currentMainMenu = Instantiate(MainMenuPrefab, menuCanvas);
            currentMainMenu.GetComponent<TitleMenuController>().IntroMenu();
        }
        else
        {
            currentMainMenu.GetComponent<TitleMenuController>().OutroMenu();
        }
    }
    public void ShowGameOver()
    {
        PauseGame();
        GameOverScreen.SetActive(true);
    }
}
