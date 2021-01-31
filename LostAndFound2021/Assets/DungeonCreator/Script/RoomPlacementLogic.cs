using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LostAndFound.Dungeon
{
    public class RoomPlacementLogic : MonoBehaviour
    {
        public enum RoomType
        {
            OpenRoom,
            Hallway,
            CombatRoom,
            TreasureRoom,
            SeceretRoom
        };

        public RoomType roomType;

        public GameObject PropParent;
        public List<Transform> StairPositions;
        public ChestController chest;

        public List<Transform> enemyPositions;
        public bool leftDoor, rightDoor, upDoor, downDoor;
        public bool combatRoom;
        private bool firstUpdate;
        private void Awake()
        {
            disableEditorGrid(); //need to be removed for the navmesh
        }
        private void Start()
        {
            firstUpdate = true;
        }
        public int getDoorRequest()  //if 2 hallways are connecting you don't need a door between them
        {
            if (roomType == RoomType.Hallway)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        private void Update()
        {
            if (firstUpdate && DungeonTracker.instance.finishedLoadingLevel) //need to be done here not on start because it locks up dungeon init as well as unity
            {
                firstUpdate = false;
                if (chest != null)
                {
                    initChest(DungeonTracker.instance.dropList.getItemDrop());
                }
            }
        }
        private void disableEditorGrid()
        {
            //distory local grid so that partent grid can take over used for nav mesh
            if (gameObject.GetComponent<GridLayout>() != null)
            {
                Destroy(gameObject.GetComponent<GridLayout>());
            }

        }
        public void initChest(ItemChances item)
        {

            if (item == null)
            {
                Debug.Log("LevelGenerator: item from dropList == null");
            }

            List<ItemDropBase> chestStash = new List<ItemDropBase>();
            ItemDropBase drop = new ItemDropBase();

            //converts it
            drop.items = item.item;
            drop.RandomAmount = item.StackAmount;
            chestStash.Add(drop);

            chest.ChestInit(chestStash);
        }
        public void SpawnEnemies(Transform partent, List<Attacker> AttackerList)
        {
            if (combatRoom == false) return;

            
            EnemyDrop enemyList = DungeonTracker.instance.getFloorEnemyList();
            int enemyPositionIndex = Random.Range(0, enemyPositions.Count);
            foreach (GameObject enemy in enemyList.Enemies)
            {
                GameObject obj = Instantiate(enemy, partent);
                if(enemyPositionIndex >= enemyPositions.Count)
                {
                    Debug.LogError("one of the room does not have enemy spawn points");
                }
                obj.transform.position = enemyPositions[enemyPositionIndex].position;

                //random position
                //NPCController NPCController = obj.GetComponentInChildren<NPCController>();
                //if(NPCController != null)
                //{
                    //NPCController.RotateCharacterToAngle(Random.Range(0, 360));

                    //AttackerList.Add(obj.GetComponentInChildren<Attacker>());
                //}

                enemyPositionIndex++;
                if (enemyPositionIndex >= enemyPositions.Count)
                {
                    enemyPositionIndex = 0;
                }
            }
        }
    }

}

