using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BoxControllerObject : Interactable
{
    public GameObject brokenPeicePrefab;
    public Sprite[] shards;
    public int maxBreakAblePiece = 5;
    //public ItemDropManager itemManager;
    private float ItemSpawnTimer;
    public Health health;
    private Animator animator;
    public float HeartSpawnChance = 0.2f;
    public float ItemSpawnChance = 0.1f;
    public GameObject heartPrefab;
    public PlayableAsset openCrateAnimation;
    public void Awake()
    {
        animator = GetComponent<Animator>();
        ItemSpawnTimer = 5f;
        //interactionDistance = 5;
    }
    public override void Interact()
    {
        //base.Interact();
    }

    public override void Damage(int damage)
    {
        if(health.currentHealth > 0)
        {
            health.currentHealth -= damage;

            float randomHit = Random.Range(0.0f, 1.0f);
            Debug.Log("random box hit = " + randomHit);
            animator.SetFloat("damage", randomHit); //play different animation each time make it look like it bounces
            animator.SetTrigger("hit");
        }
        else
        {
            int randomPiece = Random.Range(2, maxBreakAblePiece);

            for (int i = 0; i < randomPiece; i++)
            {
                int random = Random.Range(0, shards.Length);
                float OffsetX = Random.Range(-0.5f, 0.5f);
                float OffsetY = Random.Range(-0.5f, 0.5f);
                Instantiate(brokenPeicePrefab, new Vector3(transform.position.x + OffsetX, transform.position.y + OffsetY), transform.rotation).GetComponent<BrokenPieces>().setUp(shards[random], 30f);
            }


            // Crates mostly spawn health
            if (Random.Range(0.0f, 1.0f) <= HeartSpawnChance)
            {
                var heart = Instantiate(heartPrefab, transform.position, transform.rotation);
                heart.transform.position = transform.position;
            }
            else if (Random.Range(0.0f, 1.0f) <= ItemSpawnChance)
            {
                Debug.Log("TRYING TO SPAWN OBJECT FROM CRATE");
                Item item = ItemListManager.instance.getDrop();
                
                if (item != null)
                {
                    Debug.Log("Got drop");
                    GameHandler.instance.AddItemToInventory(item);

                        
                    var popup = Instantiate(GameHandler.instance.popUpItemPrefab);
                    popup.transform.position = transform.position;

                    var popupComp = popup.GetComponent<Popup>();
                    // popup.activate(item);

                    // isOpened = true;
                    // Debug.Log(item);
                    popupComp.itemRef.sprite = item.stats.icon;
                    // popupComp.director.Play(openCrateAnimation);
                    GameHandler.instance.audioSystem.playSoundEffect("chime");
                    // ItemPickUpHandler.createTimedPickUp(GameController.instance.ItemPickUpPrefab, item, gameObject.transform, ItemSpawnTimer);
                }
            }

            GameHandler.instance.audioSystem.playSoundEffect("crate");
            Destroy(this.gameObject);
        }
    }
}
