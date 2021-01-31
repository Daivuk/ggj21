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
    public Health health;
    private Animator animator;
    public float HeartSpawnChance = 0.2f;
    public GameObject heartPrefab;
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
            animator.SetFloat("damage", randomHit);
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
            //spawn item;
            //Item item = itemManager.getDrop();
            /*
            if (item != null)
            {
                //ItemPickUpHandler.createTimedPickUp(GameController.instance.ItemPickUpPrefab, item, gameObject.transform, ItemSpawnTimer);
            }
            */

            // Crates mostly spawn health
            if (Random.Range(0.0f, 1.0f) <= HeartSpawnChance)
            {
                var heart = Instantiate(heartPrefab, transform.position, transform.rotation);
                heart.transform.position = transform.position;
            }

            Destroy(this.gameObject);
        }




        
    }
}
