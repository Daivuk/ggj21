using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStash : MonoBehaviour
{
    public List<Item> Stash;
    public List<ItemDropBase> OriginalItemList;

    private void Awake()
    {

        if (Stash == null)
        {
            init();
        }

    }
    public void clearList()
    {
        Stash.Clear();
    }
    public void init()
    {
        Stash = new List<Item>();
        for (int i = 0; i < OriginalItemList.Count; i++)
        {
            Stash.Add(null);
        }
        for (int i = 0; i < OriginalItemList.Count; i++)
        {
            if (OriginalItemList[i].items != null)
            {
                Item item = new Item(ItemListManager.getItemStats(OriginalItemList[i].items.itemID));

                int amount = Random.Range((int)OriginalItemList[i].RandomAmount.x, (int)OriginalItemList[i].RandomAmount.y);
                item.currentStack = amount;
                Stash[i] = item;
            }
        }
    }

    public bool isStashEmpty()
    {
        if (Stash.Count == 0) return true;
        return false;
    }
}
