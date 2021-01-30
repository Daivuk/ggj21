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

    public void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (loadItem)
        {
            itemStash.init();
        }
    }

    public void ChestInit(List<ItemDropBase> itemDrops)
    {
        if (itemStash == null)
        {
            Debug.Log("ItemStash == null ");
        }

        itemStash.clearList();
        itemStash.OriginalItemList = itemDrops;
        itemStash.init();
    }
    public override void Interact()
    {
        //base.Interact();
        if(isOpened == false)
        {
            isOpened = true;
            itemRef.sprite = itemStash.Stash[0].stats.icon;
            director.Play(openChestAnimation);
        }
    }

    public void AddItemToInventory()
    {
        GameHandler.instance.AddItemToInventory(itemStash.Stash[0]);
    }


    
}
