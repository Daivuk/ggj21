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
            if(DungeonTracker.instance.travelingBetweenFloors == false &&
               DungeonTracker.instance.travelingBetweenFloorsCoolDown <= 0) // I know this is super hacky, please dont hate me. hashtag gamejam
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
