using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostAndFound.Dungeon
{
    public class DungeonEntranceController : MonoBehaviour
    {
        void Start()
        {
            if (DungeonTracker.instance != null)
            {
                DungeonTracker.instance.partyMemebers.gameObject.SetActive(false);
            }
        }
    }
}


