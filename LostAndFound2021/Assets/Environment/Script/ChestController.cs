using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] public ItemStash itemStash;
    public bool storage;

    public void ChestInit(List<ItemDropBase> itemDrops)
    {
        if (itemStash == null)
        {
            Debug.Log("ItemStash == null ");
        }
        if (storage == false)
        {
            itemStash.clearList();
            itemStash.OriginalItemList = itemDrops;
            itemStash.init();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
