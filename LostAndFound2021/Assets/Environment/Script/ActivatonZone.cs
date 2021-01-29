using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatonZone : MonoBehaviour
{
    [HideInInspector] public CapsuleCollider2D circleCollider;

    public void Awake()
    {
        circleCollider = GetComponent<CapsuleCollider2D>();
    }

    //can use this for highlighting
}
