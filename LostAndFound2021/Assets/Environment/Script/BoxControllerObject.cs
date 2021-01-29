using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxControllerObject : Interactable
{
    public GameObject brokenPeicePrefab;
    public Sprite[] shards;
    public int maxBreakAblePiece = 5;
    //public ItemDropManager itemManager;
    private float ItemSpawnTimer;

    public void Awake()
    {
        ItemSpawnTimer = 5f;
        //interactionDistance = 5;
    }
    public override void Interact()
    {
        //base.Interact();
    }

    public void Damage()
    {
        //create a shatter 5 pease
        int randomPiece = Random.Range(2, maxBreakAblePiece);

        for (int i = 0; i < randomPiece; i++)
        {
            int random = Random.Range(0, shards.Length);
            float OffsetX = Random.Range(-0.5f, 0.5f);
            float OffsetY = Random.Range(-0.5f, 0.5f);
            Instantiate(brokenPeicePrefab, new Vector3(transform.position.x + OffsetX, transform.position.y + OffsetY), transform.rotation).GetComponent<BrokenPieces>().setUp(shards[random], 30f);
        }

        //spawn item;
        //Item item = itemManager.getDrop();
        /*
        if (item != null)
        {
            //ItemPickUpHandler.createTimedPickUp(GameController.instance.ItemPickUpPrefab, item, gameObject.transform, ItemSpawnTimer);
        }
        */

        Destroy(this.gameObject);
    }
}
