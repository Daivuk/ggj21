using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected float interactionDistance;
    protected bool combatObject;
    protected string currentActionText;
    protected string currentAction;

    public ActivatonZone activatonZone;

    public virtual void Interact()
    {

    }

    public void Damage(int damageAmount)
    {

    }
}
