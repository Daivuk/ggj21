﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mover : MonoBehaviour
{
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
    public GameObject map;
    public GameObject water;

    public enum characterState
    {
        Idle,
        Walking,
        Attack,
        Swimming,
        Dead
    }

    characterState state;

    public float pushSpeed;
    private float dashCounter, dashCoolCounter;

    void Start()
    {
        // In case of not set in inespector. Turns out lot of prefabs have a "mover".
        // Didn't feel like going through them all. GAME JAM!
        if (!map) map = GameObject.Find("Map");
        if (!water) water = GameObject.Find("Water");

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
        float speedMultiplier = 1.0f;

        if (animateMovement == true)
        {
            if (movementDirection.x < -0.1)
            {
                movementDirection.x = -1;
            }
            else if (movementDirection.x > 0.1)
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
            }
            else if (movementDirection.y > 0.5)
            {
                movementDirection.y = 1;
            }
            else
            {
                movementDirection.y = 0;
            }

            if (movementDirection.magnitude > 1)
            {
                movementDirection.Normalize();
            }

            animatorController.SetFloat("DirectionX", movementDirection.x);
            animatorController.SetFloat("DirectionY", movementDirection.y);



            // Normal movement
            var grid = map.GetComponent<Grid>();
            var tilemap = water.GetComponent<Tilemap>();

            Vector3Int lPos = grid.WorldToCell(gameObject.transform.position);
            var tile = tilemap.GetTile(lPos);

            if (tile)
            {
                state = characterState.Swimming;
            }
            else if (movementDirection != Vector2.zero)
            {
                state = characterState.Walking;
            }
            else
            {
                state = characterState.Idle;
            }

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
                    break;
                case characterState.Swimming:
                    animatorController.SetBool("Swimming", true);
                    animatorController.SetBool("Walking", false);
                    activeMoveSpeed = swimSpeedMultiplier;
                    break;
            }
        }

            
        
        direction = movementDirection;
        rigidbody2D.velocity = direction * activeMoveSpeed * speedMultiplier;
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
