using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public bool animateMovement;
    [HideInInspector] public Rigidbody2D rigidbody2D;
    [HideInInspector] public Vector2 direction;
    public Animator animatorController;
    public float speed;
    private float activeMoveSpeed;
    public float dashSpeed;
    public float dashLength;
    public float dashCoolDown;
    public float dashInvinciblity;

    public float pushSpeed;
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
        if(animateMovement == true)
        {
            if (movementDirection.x < -0.1)
            {
                movementDirection.x = -1;
            }else if (movementDirection.x > 0.1)
            {
                movementDirection.x = 1;
            }
            else
            {
                movementDirection.x = 0;
            }

            if (movementDirection.y < -0.1)
            {
                movementDirection.y = -1;
            }else if (movementDirection.y > 0.5)
            {
                movementDirection.y = 1;
            }
            else
            {
                movementDirection.y = 0;
            }


            animatorController.SetFloat("DirectionX", movementDirection.x);
            animatorController.SetFloat("DirectionY", movementDirection.y);


            if (movementDirection != Vector2.zero)
            {
                animatorController.SetBool("Walking", true);
            }
            else
            {
                animatorController.SetBool("Walking", false);
            }
        }
        
        direction = movementDirection;
        rigidbody2D.velocity = direction * activeMoveSpeed;
    }
    public void walk(Vector2 movementDirection, float Speed)
    {
        Debug.Log("pushing crate " + movementDirection.ToString() + " " + speed);
        activeMoveSpeed = speed;
        walk(movementDirection);
    }
    public bool Dash()
    {
        if (activeMoveSpeed == pushSpeed) return false; // you cant dash if your pushing
        if (dashCoolCounter < 0)
        {
            GameHandler.instance.playSoundEffect("dash");
            activeMoveSpeed = dashSpeed;
            dashCounter = dashLength;
            return true;
        }
        return false;
    }
    public void StartDrag()
    {
        activeMoveSpeed = pushSpeed;
    }
    public void StopDrag()
    {
        activeMoveSpeed = speed;
    }
    /*
    public void KnockBack(Vector3 SourceOfForce, float knockbackStrength)
    {
        Vector3 dirFromAttacker = (SourceOfForce - transform.position).normalized;
        Vector2 force = new Vector2(dirFromAttacker.x, dirFromAttacker.y) * knockbackStrength;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.position = rigidbody2D.position + force;
    }
    */
}
