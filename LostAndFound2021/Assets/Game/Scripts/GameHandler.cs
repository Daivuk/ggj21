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
    public void RemoveItemFromInventory(string itemID)
    {
        for(int i=0; i < instance.inventory.Count; i++)
        {
            if(instance.inventory[i].stats.itemID == itemID)
            {
                instance.inventory.RemoveAt(i);
                break;
            }
        }
        // inventory.Remove(item); // Different instance
        refreshInvHud();
    }
    public void refreshInvHud()
    {
        var inv_bar = GameObject.Find("InventoryBackground");
        foreach (Transform child in inv_bar.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        Dictionary<Sprite, int> dict = new Dictionary<Sprite, int>();
        int i = 0;
        for (i = 0; i < inventory.Count; ++i)
        {
            var item = inventory[i];
            if (dict.ContainsKey(item.stats.icon))
            {
                dict[item.stats.icon]++;
            }
            else
            {
                dict.Add(item.stats.icon, 1);
            }
        }

        var bar_trans = inv_bar.GetComponent<RectTransform>();
        i = 0;
        foreach (var kv in dict)
        {
            var item_spr = kv.Key;

            var inv_item = Instantiate(InvItemPrefab);

            var text = inv_item.transform.GetChild(0);
            if (kv.Value == 1)
            {
                text.gameObject.SetActive(false);
            }
            else
            {
                text.GetComponent<Text>().text = kv.Value.ToString();
            }

            inv_item.GetComponent<Image>().sprite = item_spr;
            inv_item.transform.parent = inv_bar.transform;

            var trans = inv_item.GetComponent<RectTransform>();
            trans.localPosition = new Vector2(-(float)(dict.Count - 1) * 0.5f * 72.0f + (float)i * 72.0f, 0);

            ++i;
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
