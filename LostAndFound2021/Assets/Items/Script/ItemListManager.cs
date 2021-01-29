using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListManager : MonoBehaviour
{
    public static ItemListManager instance;
    public List<ItemBaseStat> GameItems;
    public Dictionary<string, ItemBaseStat> ItemLookUpTable;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
            instance.initItems();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void initItems()
    {
        ItemLookUpTable = new Dictionary<string, ItemBaseStat>();
        AddToListToDictionary(GameItems);
    }

    private void AddToListToDictionary(List<ItemBaseStat> ItemsList)
    {
        for (int i = 0; i < ItemsList.Count; i++)
        {
            ItemLookUpTable[ItemsList[i].itemID] = ItemsList[i];
        }
    }
    public static ItemBaseStat getItemStats(string ID)
    {
        if (instance.ItemLookUpTable.ContainsKey(ID))
        {
            return (ItemBaseStat)instance.ItemLookUpTable[ID];
        }
        else
        {
            Debug.Log("could not load item with ID: " + ID);
            return null;
        }
    }
    public static string getItemName(string ID)
    {
        return instance.ItemLookUpTable[ID].itemName;
    }
}
