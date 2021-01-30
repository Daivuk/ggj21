using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatonZone : MonoBehaviour
{
    [HideInInspector] public CapsuleCollider2D circleCollider;
    public Interactable partent;
    public void Awake()
    {
        circleCollider = GetComponent<CapsuleCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {

        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "player")
        {

        }
    }

    //can use this for highlighting
}
