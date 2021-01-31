using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using LostAndFound.Dungeon;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    private bool Alive;
    [HideInInspector] public Mover mover;
    [HideInInspector] public Attacker attacker;
    public bool LockedCharacter;
    public GameObject FocusObject; //change it for others
    public Health characterHealth;

    private PlayerInput inputActions;
    public int viewDistance;
    public LayerMask activationZonesMask;

    public Interactable currentlyInteractingWith;
    public Interactable currentlyAttacking;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.Alive = true;
            inputActions = new PlayerInput();

            inputActions.Player.DashAction.performed += PreformAction1;
            inputActions.Player.Interact.performed += InteractWithObject;
            inputActions.Player.Attack.performed += AttackAction;
            inputActions.Player.MainMenu.performed += AccessMenu;

            setUpCharacter(FocusObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void setUpCharacter(GameObject Character)
    {
        mover = FocusObject.GetComponent<Mover>();
        characterHealth = FocusObject.GetComponent<Health>();
        attacker = FocusObject.GetComponent<Attacker>();
        attacker.Died.AddListener(() => DeathLogic());
    }
    public GameObject getFocusObject()
    {
        return FocusObject;
    }

    public void Update()
    {
        if (LockedCharacter) return;

        MovementInput(inputActions.Player.Move.ReadValue<Vector2>());
    }

    public void OnEnable()
    {
        inputActions.Enable();
    }
    public void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Disable();
        }
    }

    public void MovementInput(Vector2 Movement)
    {
        /*
        if(Movement != Vector2.zero)
        {
            Debug.Log(Movement.ToString());
        }
        */
        mover.walk(Movement);
        if(currentlyInteractingWith != null && currentlyInteractingWith.tag == "box")
        {
            currentlyInteractingWith.GetComponent<Mover>().walk(Movement,mover.pushSpeed);
        }
    }

    private void InteractWithObject(InputAction.CallbackContext obj)
    {
        if (Alive == false) return;
        //angle UtilityHelper.GetVectorFromAngle(mover.direction)

        RaycastHit2D[] raycastArray = Physics2D.RaycastAll(FocusObject.transform.position, mover.direction, viewDistance, activationZonesMask);

        if (raycastArray.Length > 0)
        {
            for (int x = 0; x < raycastArray.Length; x++)
            {
                ActivatonZone zone = raycastArray[x].collider.gameObject.GetComponent<ActivatonZone>();
                if(zone != null)
                {
                    Interactable selectedInteractable = zone.partent;

                    switch (selectedInteractable.tag)
                    {
                        case "box":
                            if (currentlyInteractingWith != selectedInteractable)
                            {
                                //grab the box
                                currentlyInteractingWith = selectedInteractable;
                            }
                            else
                            {
                                //drop the box
                                currentlyInteractingWith = null;
                            }
                            break;
                        case "chest":
                            ((ChestController)selectedInteractable).Interact();
                            break;
                        case "MrSassyMain":
                            Item sassyItem = ((MrSassyController)selectedInteractable).currentRequestItem;

                            if (((MrSassyController)selectedInteractable).debugTestAccept)
                            {
                                ((MrSassyController)selectedInteractable).Interact();
                            }
                            else
                            {
                                if(GameHandler.instance.hasItemInInventory(sassyItem.stats.itemID))
                                {
                                    ((MrSassyController)selectedInteractable).Interact();
                                    GameHandler.instance.RemoveItemFromInventory(sassyItem.stats.itemID);
                                }
                            }
                            
                            break;
                    }
                    break;
                }
               
               
            }
        }
        else
        {
            currentlyInteractingWith = null;
        }
    }

    private void GenerateDungeon(InputAction.CallbackContext obj)
    {
        //DungeonTracker.instance.StartDungeon();
    }

    private void PreformAction1(InputAction.CallbackContext obj)
    {
        if (Alive == false) return;
        Debug.Log("Dash!");
        mover.Dash();
    }

    private void AttackAction(InputAction.CallbackContext obj)
    {
        if (Alive == false) return;
        RaycastHit2D[] raycastArray = Physics2D.RaycastAll(FocusObject.transform.position, mover.direction, viewDistance, activationZonesMask);

        if (raycastArray.Length > 0)
        {
            for (int x = 0; x < raycastArray.Length; x++)
            {
                ActivatonZone zone = raycastArray[x].collider.gameObject.GetComponent<ActivatonZone>();
                if(zone != null)
                {
                    Interactable selectedInteractable = zone.partent;
                    switch (selectedInteractable.tag)
                    {
                        case "box":
                            ((BoxControllerObject)selectedInteractable).Damage(attacker.AttackDamage);
                            break;
                        case "Enemy":
                            ((EnemyAI)selectedInteractable).Damage(attacker.AttackDamage);
                            break;
                    }
                    break;
                }
               
            }
        }
        else
        {
            currentlyAttacking = null;
        }

        GameHandler.instance.audioSystem.playSoundEffect("hit1");
        mover.playattackAnimation();
    }

    private void PreformAction3(InputAction.CallbackContext obj)
    {
        /*
        if (attacker.attackOrder.Count == 0) return;
        if (attacker.attackOrder[0].attackerActionState == actionState.IDLE) //stops rappid button pressing
        {
            attacker.Act(2 + (attacker.attackOrder[0].actionActionMenuCount * 3));
        }
        */
    }

    private void AccessMenu(InputAction.CallbackContext obj)
    {
        if (Alive == false) return;
        GameHandler.instance.RequestMainMenu();
    }

    public void DeathLogic()
    {
        Alive = false;
        Debug.Log("Player died you should star loading the game over screen");
        GameHandler.instance.ShowGameOver();
    }

}
