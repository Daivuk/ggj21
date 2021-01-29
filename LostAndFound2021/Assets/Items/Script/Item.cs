using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemBaseStat stats;
    public int currentStack;

    public Item(ItemBaseStat stat)
    {
        stats = stat;
        currentStack = 0;
    }
}
