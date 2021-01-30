using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item 
{
    public ItemBaseStat stats;
    public int currentStack;

    public Item(ItemBaseStat stat)
    {
        stats = stat;
        currentStack = 0;
    }
}
