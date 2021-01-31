using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class ChestController : Interactable
{
    public bool loadItem;
    [SerializeField] public ItemStash itemStash;
    public Sprite closeChest;
    public Sprite openChest;
    private bool isOpened;

    private SpriteRenderer sprite;
    public SpriteRenderer itemRef;

    public PlayableDirector director;
    public PlayableAsset openChestAnimation;

    Item item = null;

    public void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void ChestInit(List<ItemDropBase> itemDrops)
    {
        if (itemStash == null)
        {
            Debug.Log("ItemStash == null ");
        }

        // itemStash.clearList();
        // itemStash.OriginalItemList = itemDrops;
        // itemStash.init();
    }

    void assureItem() // Yea. Item would get null on some chest. no idea why... clock is ticking
    {
        if (item == null)
        {
            // itemStash.init(); // ... ffs we just need one

            Debug.Log(itemStash.OriginalItemList);

            var original_item = itemStash.OriginalItemList[Random.Range(0, itemStash.OriginalItemList.Count)];

            ItemBaseStat stat = ItemListManager.getItemStats(original_item.items.itemID);
            item = new Item(stat);

            // int amount = Random.Range((int)OriginalItemList[i].RandomAmount.x, (int)OriginalItemList[i].RandomAmount.y);
            item.currentStack = 1; // We don't care about stack from chest.
        }
    }

    public override void Interact()
    {
        //base.Interact();
        if(isOpened == false)
        {
            assureItem();
            isOpened = true;
            Debug.Log(item);
            itemRef.sprite = item.stats.icon; // HOW IS ITEM STILL NULL HERE!!!??
            director.Play(openChestAnimation);
            GameHandler.instance.audioSystem.playSoundEffect("chime");
        }
    }

    public void AddItemToInventory()
    {
        assureItem();
        GameHandler.instance.AddItemToInventory(item);
    }


    
}
