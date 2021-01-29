using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rigidbody2D;
    [HideInInspector] public Vector2 direction;
    public Animator animatorController;
    public float speed;
    private float activeMoveSpeed;
    public float dashSpeed;
    public float dashLength;
    public float dashCoolDown;
    public float dashInvinciblity;
    private float dashCounter, dashCoolCounter;

    void Start()
    {
        dashCoolCounter = -1;
        dashCounter = -1;
        activeMoveSpeed = speed;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (dashCounter > 0 && dashCoolCounter < 0)
        {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                activeMoveSpeed = speed;
                dashCoolCounter = dashCoolDown;
            }
        }
        else if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
    }

    public void walk(Vector2 movementDirection)
    {
        animatorController.SetFloat("DirectionX", movementDirection.x);
        animatorController.SetFloat("DirectionY", movementDirection.y);

        if(movementDirection != Vector2.zero)
        {
            animatorController.SetBool("Walking", true);
        }
        else
        {
            animatorController.SetBool("Walking", false);
        }
        

        direction = movementDirection;
        rigidbody2D.velocity = direction * activeMoveSpeed;
    }
    public bool Dash()
    {
        if(dashCoolCounter < 0)
        {
            activeMoveSpeed = dashSpeed;
            dashCounter = dashLength;
            return true;
        }
        return false;
    }
}
