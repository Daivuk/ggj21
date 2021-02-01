using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : Interactable
{
    public enum AIBrain
    {
        Scared,
        Chaser
    }

    public AIBrain brain;
    public float AIStoppingDistance;
    public float viewDistance;
    public LayerMask activationZonesMask;

    public bool requireVisual; //mouse
    public bool requireDistance; //bat

    public float hearingDistance;

    public float fleeDistance;
    public float AttackRate;
    private float currentCountForAttackRate;
    public Vector2 lastKnowLocationOfPlayer;
    private Mover mover;
    private Attacker attacker;
    private Health health;

    private NavMeshAgent agent;
    private bool Alive;
    public int perfectOfItemDrop;
    public void Awake()
    {
        Alive = true;
        mover = GetComponent<Mover>();
        attacker = GetComponent<Attacker>();
        health = GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.stoppingDistance = AIStoppingDistance;
        agent.enabled = false;
        attacker.Died.AddListener(() => deathLogic());
        currentCountForAttackRate = 0;
    }

    public bool LookForPlayer()
    {
        RaycastHit2D[] raycastArray = Physics2D.RaycastAll(transform.position, mover.direction, viewDistance, activationZonesMask);

        if (raycastArray.Length > 0)
        {
            for (int x = 0; x < raycastArray.Length; x++)
            {
                if(raycastArray[x].collider.gameObject.tag == "Player")
                {
                    lastKnowLocationOfPlayer = raycastArray[x].collider.gameObject.transform.position;
                    return true;
                }
            }
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Alive == false) return;
        if (GameHandler.instance.GamePaused )
        {
            //pause navmesh
            return;
        }
        else
        {
            //resume navmesh
        }
        AILogic();
    }

    public void AILogic()
    {
        bool foundPlayer = false;
        float distance = Vector2.Distance(PlayerController.instance.getFocusObject().transform.position, transform.position);
        if (requireVisual)
        {
            foundPlayer = LookForPlayer();
        }

        if (requireDistance && foundPlayer == false)
        {
           
            if(distance <= hearingDistance)
            {
                foundPlayer = true;
                lastKnowLocationOfPlayer = PlayerController.instance.getFocusObject().transform.position;
            }
        }

        float directOfPlayer = UtilityHelper.getAngleBetweenTwoPoints(PlayerController.instance.getFocusObject().transform.position, transform.position);
        float directOpositeToPlayer = UtilityHelper.getAngleBetweenTwoPoints(transform.position, PlayerController.instance.getFocusObject().transform.position);

        switch (brain)
        {
            case AIBrain.Chaser:

                if (foundPlayer)
                {
                    agent.enabled = true;
                    agent.SetDestination(lastKnowLocationOfPlayer);
                    Debug.Log("Rat angle = " + directOfPlayer);
                    mover.walk(directOfPlayer,false,true); //face play and change
                    currentCountForAttackRate -= Time.deltaTime;

                    if(distance <= (agent.stoppingDistance -0.1) && currentCountForAttackRate <0)
                    {
                        GameHandler.instance.audioSystem.playSoundEffect("hit2");
                        mover.playattackAnimation();
                        PlayerController.instance.attacker.DamageTarget(attacker.AttackDamage);
                        currentCountForAttackRate = AttackRate;
                        var claw_fx = Instantiate(GameHandler.instance.ClawFxPrefab);
                        claw_fx.transform.position = PlayerController.instance.getFocusObject().transform.position;
                    }
                }

                break;
            case AIBrain.Scared:

                if (distance < fleeDistance)
                {
                    agent.enabled = true;
                    mover.walk(directOpositeToPlayer, true,true); //fleeing move in the other direction face away
                }
                else
                {
                    agent.enabled = false;
                    mover.rigidbody2D.velocity = Vector2.zero;
                    mover.state = Mover.characterState.Idle;
                    mover.Animation();
                }
                break;
        }
    }

    public override void Interact()
    {
     
    }

    public override void Damage(int damage)
    {
        attacker.DamageTarget( damage);
    }

    public void deathLogic()
    {
        if (Alive)
        {
            mover.rigidbody2D.velocity = Vector2.zero;
            agent.enabled = false;
            Alive = false;
            mover.animatorController.SetTrigger("Death");
        }
    }
    public void checkItemDrop()
    {
        int random = Random.Range(0, 100);
        if (random <= perfectOfItemDrop)
        {
            Item item = ItemListManager.instance.getDrop();
            GameHandler.instance.AddItemToInventory(item);
            GameObject popup = Instantiate(GameHandler.instance.popUpItemPrefab);
            popup.transform.position = transform.position;

            Popup popupComp = popup.GetComponent<Popup>();
            popupComp.itemRef.sprite = item.stats.icon;
        }
    }
}
