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

    void assureItem()
    {
        if (item == null)
        {
            item = ItemListManager.instance.getDrop();
        }
    }

    public override void Interact()
    {
        //base.Interact();
        if(isOpened == false)
        {
            if (loadItem == false)
            {
                assureItem();
            }
            else
            {
                item = itemStash.Stash[0];
            }
            isOpened = true;
            Debug.Log(item);
            itemRef.sprite = item.stats.icon;
            director.Play(openChestAnimation);
            GameHandler.instance.audioSystem.playSoundEffect("chime");
        }
    }

    //called by signel
    public void AddItemToInventory()
    {
        GameHandler.instance.AddItemToInventory(item);
    }
}
