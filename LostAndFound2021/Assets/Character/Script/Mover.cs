using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mover : MonoBehaviour
{
    public characterState state;
    public FacingDirection facing;
    private FacingDirection lastFacingDirction;
    public bool animateMovement;
    [HideInInspector] public Rigidbody2D rigidbody2D;
    [HideInInspector] public Vector2 direction;
    public Animator animatorController;
    public float speed;
    public float swimSpeedMultiplier;
    private float activeMoveSpeed;
    public float dashSpeed;
    public float dashLength;
    public float dashCoolDown;
    public float dashInvinciblity;
    [HideInInspector] public bool isInWater = false;

    public enum FacingDirection
    {
        up,
        down,
        left,
        right
    }

    public enum characterState
    {
        Idle,
        Walking,
        Attack,
        Swimming,
        Dead
    }
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
    public void Animation()
    {
        switch (state)
        {
            case characterState.Idle:
                animatorController.SetBool("Walking", false);
                animatorController.SetBool("Swimming", false);
                break;
            case characterState.Walking:
                animatorController.SetBool("Walking", true);
                animatorController.SetBool("Swimming", false);
                activeMoveSpeed = speed;
                break;
            case characterState.Attack:
                animatorController.SetTrigger("Attack");
                break;
            case characterState.Swimming:
                animatorController.SetBool("Swimming", true);
                animatorController.SetBool("Walking", false);
                activeMoveSpeed = swimSpeedMultiplier;
                break;
            case characterState.Dead:
                break;
        }
    }
    public void walk(float angle,bool useRigidBody)
    {
        if (angle <= 45 || angle >= 315)
        {
            facing = FacingDirection.left;
        }
        else if (angle > 45 && angle <= 135)
        {
            facing = FacingDirection.up;
        }
        else if (angle > 135 && angle <= 225)
        {
            facing = FacingDirection.right;
        }
        else
        {
            facing = FacingDirection.down;
        }

        setFaceDirect();

        state = characterState.Walking;

        direction = UtilityHelper.GetVectorFromAngle(angle);
        if (useRigidBody)
        {
            rigidbody2D.velocity = direction * activeMoveSpeed;
        }
        Animation();
    }
    public void walk(Vector2 movementDirection)
    {
        if (animateMovement == true)
        {
            if (movementDirection.x < -0.1)
            {
                facing = FacingDirection.left;
            }
            else if (movementDirection.x > 0.1)
            {
                facing = FacingDirection.right;
            }
            

            if (movementDirection.y < -0.1)
            {
                facing = FacingDirection.down;
            }
            else if (movementDirection.y > 0.1)
            {
                facing = FacingDirection.up;
            }
            

            if (movementDirection.magnitude > 1)
            {
                movementDirection.Normalize();
            }

            if(lastFacingDirction != facing)
            {
                setFaceDirect();
            }

            if (isInWater)
            {
                state = characterState.Swimming;
            }
            else if (Vector2.Distance(movementDirection,Vector2.zero) > 0.15)
            {
                state = characterState.Walking;
            }
            else
            {
                state = characterState.Idle;
            }

            Animation();
        }

        direction = movementDirection;
        rigidbody2D.velocity = direction * activeMoveSpeed;
    }
    private void setFaceDirect()
    {
        switch (facing)
        {
            case FacingDirection.up:
                animatorController.SetFloat("DirectionX", 0);
                animatorController.SetFloat("DirectionY", 1);
                break;
            case FacingDirection.down:
                animatorController.SetFloat("DirectionX", 0);
                animatorController.SetFloat("DirectionY", -1);
                break;
            case FacingDirection.left:
                animatorController.SetFloat("DirectionX", -1);
                animatorController.SetFloat("DirectionY", 0);
                break;
            case FacingDirection.right:
                animatorController.SetFloat("DirectionX", 1);
                animatorController.SetFloat("DirectionY", 0);
                break;
        }
        lastFacingDirction = facing;
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
            GameHandler.instance.audioSystem.playSoundEffect("dash");
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
    public void FinishedDeathAnimation()
    {
        Destroy(this.gameObject);
    }

    public void playattackAnimation()
    {
        if (state != Mover.characterState.Swimming)
        {
            state = Mover.characterState.Attack;
            Animation();
        }
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
