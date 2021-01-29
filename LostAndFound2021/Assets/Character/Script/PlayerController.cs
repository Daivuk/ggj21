using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using LostAndFound.Dungeon;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [HideInInspector] public Mover mover;
    public bool LockedCharacter;
    public GameObject FocusObject; //change it for others

    private PlayerInput inputActions;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            inputActions = new PlayerInput();

            inputActions.Player.DashAction.performed += PreformAction1;
            //inputActions.Player.LaunchDungeon.performed += GenerateDungeon;
            //inputActions.Player.Action2.performed += PreformAction2;
            //inputActions.Player.Action3.performed += PreformAction3;


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
    }

    private void InteractWithObject(InputAction.CallbackContext obj)
    {
       
    }

    private void GenerateDungeon(InputAction.CallbackContext obj)
    {
        DungeonTracker.instance.StartDungeon();
    }

    private void PreformAction1(InputAction.CallbackContext obj)
    {
        Debug.Log("Dash!");
        mover.Dash();
    }

    private void PreformAction2(InputAction.CallbackContext obj)
    {
        /*
        if (attacker.attackOrder.Count == 0) return;
        if (attacker.attackOrder[0].attackerActionState == actionState.IDLE)//stops rappid button pressing
        {
            attacker.Act(1 + (attacker.attackOrder[0].actionActionMenuCount * 3));
        }
        */
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

}
