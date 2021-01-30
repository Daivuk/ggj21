﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LostAndFound.Dungeon;
public class DungeonDoor : MonoBehaviour
{
    public DungeonTracker dungeon;
    public bool entering;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            dungeon.StartDungeon();
            dungeon.setPlayerToStartingStairs();
            if (entering)
            {
                GameHandler.instance.playTheme("dungeon");
            }
            else
            {
                GameHandler.instance.playTheme("title");
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}