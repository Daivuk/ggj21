using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LostAndFound.Dungeon
{
    public class DungeonStairs : MonoBehaviour
    {
        public bool downStairs;
        public Transform playerOffset;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(DungeonTracker.instance.travelingBetweenFloors == false)
            {
                if (collision.gameObject == PlayerController.instance.getFocusObject())
                {
                    if (downStairs)
                    {
                        DungeonTracker.instance.Down();
                    }
                    else
                    {
                        DungeonTracker.instance.Up();
                    }
                }
            }
        }
    }
}
