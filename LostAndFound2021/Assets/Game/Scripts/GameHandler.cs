using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.UI;

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

    public GameObject InvItemPrefab;

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
        inventory.Add(item); // We don't increment a stack? TODO I guess
        refreshInvHud();
    }
    public void RemoveItemFromInventory(Item item)
    {
        inventory.Remove(item);
        refreshInvHud();
    }
    public void refreshInvHud()
    {
        var inv_bar = GameObject.Find("InventoryBackground");
        foreach (Transform child in inv_bar.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        var bar_trans = inv_bar.GetComponent<RectTransform>();
        for (var i = 0; i < inventory.Count; ++i)
        {
            var item = inventory[i];

            var inv_item = Instantiate(InvItemPrefab);
            inv_item.GetComponent<Image>().sprite = item.stats.icon;
            inv_item.transform.parent = inv_bar.transform;

            var trans = inv_item.GetComponent<RectTransform>();
            trans.localPosition = new Vector2(-(float)(inventory.Count - 1) * 0.5f * 72.0f + (float)i * 72.0f, 0);
        }
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
