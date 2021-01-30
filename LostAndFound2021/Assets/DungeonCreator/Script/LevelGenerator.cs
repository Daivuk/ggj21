using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
namespace LostAndFound.Dungeon
{
    public class LevelGenerator : MonoBehaviour
    {
        public NavMeshSurface2d mesh;
        public GameObject layoutRoom;
        public GameObject originLocationsParent;
        public GameObject RoomParent;
        public GameObject RoomProps;
        public GameObject EnemyParent;
        public Color startColor, endColor;
        public int distanceToEnd;
        public Transform generationPoint;
        public enum Direction
        {
            up, right, down, left
        };
        public Direction selectedDirection;
        public int xOffset = 18, yOffset = 10;
        private GameObject startingRoom;
        private GameObject endRoom;
        private List<Vector3> roomPositions;
        private List<GameObject> roomList;

        private List<GameObject> doorList;
        private List<int> treasureRoomIndexs;
        private List<int> combatRoomIndexs;
        public LayerMask roomMask;

        [HideInInspector] public DungeonStairs upStairs;
        [HideInInspector] public DungeonStairs downStairs;

        public RoomPrefabs RoomTypes;
        public RoomObjectPrefabs RoomObjPrefabs;

        private float ChanceForCombatRoom;
        private float ChanceForTreasureRoom;
        private int MaxChestCount;
        private int chestCount;

        private bool firstUpdate;
        public enum RoomConditions
        {
            DefaultRoom,
            StairRoom,
            TreasureRoom,
            CombatRoom,
        };

        public List<Attacker> EnemyList;
        private bool newMeshBuild;

        public void initGenerator()
        {
            Debug.Log("LevelGen init");
            firstUpdate = false;
            roomPositions = new List<Vector3>();
            startingRoom = Instantiate(layoutRoom, originLocationsParent.transform);
            generationPoint.position = Vector3.zero;
            startingRoom.transform.position = generationPoint.position;
            startingRoom.GetComponent<SpriteRenderer>().color = startColor;
            roomPositions.Add(new Vector3(0, 0, 0)); // starting position;

            //create generic position
            for (int i = 0; i < distanceToEnd; i++)
            {
                selectedDirection = (Direction)Random.Range(0, 4);
                movegenerationPoint();

                //while (Physics2D.OverlapCircle(generationPoint.position, 0.2f, roomMask))
                bool FreeSpot = false;
                while(FreeSpot == false)
                {
                    FreeSpot = true;
                    for (int y = 0; y < roomPositions.Count; y++)
                    {
                        if(roomPositions[y] == generationPoint.position)
                        {
                            FreeSpot = false;
                            break;
                        }
                    }
                    if(FreeSpot == false)
                    {
                        movegenerationPoint();
                    }
                }

                GameObject room = Instantiate(layoutRoom, originLocationsParent.transform);
                room.transform.position = generationPoint.position;

                if (i + 1 == distanceToEnd)
                {
                    endRoom = room;
                    endRoom.GetComponent<SpriteRenderer>().color = endColor;
                    roomPositions.Add(generationPoint.position);
                }
                else
                {
                    roomPositions.Add(generationPoint.position);
                }
            }
            //create room outlines


            if (DungeonTracker.instance != null)
            {
                ChanceForTreasureRoom = DungeonTracker.instance.chancesForChestRoom;
                MaxChestCount = DungeonTracker.instance.maxChestOnLevel;
                ChanceForCombatRoom = DungeonTracker.instance.chancesForCombatRoom;

                GenterateDungeon();
            }
            else
            {
                Debug.LogError("could find dungeon tracker");
            }

            originLocationsParent.SetActive(false);
            Debug.Log("LevelGen init finished");
        }
        public void GenterateDungeon()
        {
            DungeonTracker.instance.finishedLoadingLevel = false;
            Debug.Log("GenterateDungeon: Entered");

            roomList = new List<GameObject>();
            int roomIndex = 0; //help to figure out if certain room should be special

            treasureRoomIndexs = new List<int>();
            combatRoomIndexs = new List<int>();
            calcuateSpecailRooms();

            //build out the rooms
            
            CreateRoomsOutLine(Vector3.zero, RoomConditions.StairRoom); //starting room


            for (int i = 1; i < roomPositions.Count - 1; i++)
            {
                RoomConditions conditions = RoomConditions.DefaultRoom;
                
                if (treasureRoomIndexs.Contains(roomIndex))
                {
                    conditions = RoomConditions.TreasureRoom;
                }
                
                CreateRoomsOutLine(roomPositions[i], conditions);
                roomIndex++;
            }

            CreateRoomsOutLine(endRoom.transform.position, RoomConditions.StairRoom);
            

            doorList = new List<GameObject>();

            //creating props for the room
            for(int i = 0; i < roomList.Count;i++)
            {
                RoomPlacementLogic rp = roomList[i].GetComponent<RoomPlacementLogic>();

                if (i == 0)
                //if (roomList[i].transform.position == Vector3.zero)
                {
                    //start position
                    upStairs = Instantiate(RoomObjPrefabs.StairsUp, RoomProps.transform).GetComponent<DungeonStairs>();
                    upStairs.transform.position = rp.StairPositions[0].position;
                }
                else if (i == roomList.Count - 1) //(roomList[i].transform.position == endRoom.transform.position)
                {
                    //end room
                    downStairs = Instantiate(RoomObjPrefabs.StairsDown, RoomProps.transform).GetComponent<DungeonStairs>();
                    downStairs.transform.position = rp.StairPositions[0].position;
                }
                else
                {
                    //all other room types
                }

                //door logic
                //AddDoorLogic(rp);

            }
            
            DungeonTracker.instance.finishedLoadingLevel = true;
            
            //StartCoroutine(buildMesh());
            //Debug.Log("levelGenerator: finished adding stairs and doors");
        }
        public void Start()
        {
            //StartCoroutine(buildMesh());
        }
        public void LateUpdate()
        {
            if(mesh.navMeshData != null && newMeshBuild == true)
            {
                newMeshBuild = false;
                spawnEnemys();
            }
        }
        private IEnumerator buildMesh()
        {
            
            yield return new WaitForEndOfFrame();
            mesh.BuildNavMesh();
            yield return new WaitForEndOfFrame();

            newMeshBuild = true;

            /*
             *  old spawn enemy
             * 
             * 
            //turn on all the nav mesh enemy agents
            foreach (GameObject room in roomList)
            {
                RoomPlacementLogic rp = room.GetComponent<RoomPlacementLogic>();
                rp.SpawnEnemies();
            }
            yield return null;
            */

        }
        //show speed up loading
        private void spawnEnemys()
        {
            EnemyList = new List<Attacker>();
            foreach (GameObject room in roomList)
            {
                RoomPlacementLogic rp = room.GetComponent<RoomPlacementLogic>();
                rp.SpawnEnemies(EnemyParent.transform, EnemyList);
            }
        }
        void movegenerationPoint()
        {
            switch (selectedDirection)
            {
                case Direction.up:
                    generationPoint.position += new Vector3(0, yOffset, 0);
                    break;
                case Direction.left:
                    generationPoint.position += new Vector3(-xOffset, 0, 0);
                    break;
                case Direction.down:
                    generationPoint.position += new Vector3(0, -yOffset, 0);
                    break;
                case Direction.right:
                    generationPoint.position += new Vector3(xOffset, 0, 0);
                    break;
            }
        }

        #region creating rooms

        public void CreateRoomsOutLine(Vector3 roomPosition, RoomConditions conditions)
        {

            Debug.Log("RoomPosition " + roomPosition.ToString() + " room condition " + conditions.ToString());

            
            bool roomAbove = false;
            bool roomBelow = false;
            bool roomLeft = false;
            bool roomRight = false;
            Vector3 postionAbove = roomPosition + new Vector3(0, yOffset, 0);
            Vector3 postionBelow = roomPosition + new Vector3(0, -yOffset, 0);

            Vector3 postionLeft = roomPosition + new Vector3(-xOffset, 0, 0);
            Vector3 postionRight = roomPosition + new Vector3(xOffset, 0, 0);

            for(int i = 0;  i < roomPositions.Count; i++)
            {
                if(postionAbove == roomPositions[i])
                {
                    roomAbove = true;
                }
                if(postionBelow == roomPositions[i])
                {
                    roomBelow = true;
                }
                if(postionLeft == roomPositions[i])
                {
                    roomLeft = true;
                }
                if(postionRight == roomPositions[i])
                {
                    roomRight = true;
                }
            }
           
            int directionCount = 0;

            if (roomAbove)
            {
                directionCount++;
            }
            if (roomBelow)
            {
                directionCount++;
            }
            if (roomLeft)
            {
                directionCount++;
            }
            if (roomRight)
            {
                directionCount++;
            }

            Debug.Log("create room at " + roomPosition + " roomAbove: " + roomAbove + " roomBelow: " + roomBelow + " roomleft: " + roomLeft + " roomRight: " + roomRight);

            switch (directionCount)
            {
                case 1:
                    if (roomAbove)
                    {
                        roomList.Add(RandomRoom(RoomTypes.singleUp, roomPosition, conditions));
                    }
                    else if (roomBelow)
                    {
                        roomList.Add(RandomRoom(RoomTypes.singleDown, roomPosition, conditions));
                    }
                    else if (roomLeft)
                    {
                        roomList.Add(RandomRoom(RoomTypes.singleLeft, roomPosition, conditions));
                    }
                    else
                    {
                        //right room
                        roomList.Add(RandomRoom(RoomTypes.singleRight, roomPosition, conditions));
                    }
                    break;
                case 2:
                    if (roomAbove && roomBelow)
                    {
                        roomList.Add(RandomRoom(RoomTypes.DoubleUpDown, roomPosition, conditions));
                    }
                    else if (roomLeft && roomRight)
                    {
                        roomList.Add(RandomRoom(RoomTypes.DoubleLeftRight, roomPosition, conditions));
                    }
                    else if (roomAbove && roomLeft)
                    {
                        roomList.Add(RandomRoom(RoomTypes.DoubleUpLeft, roomPosition, conditions));
                    }
                    else if (roomAbove && roomRight)
                    {
                        roomList.Add(RandomRoom(RoomTypes.DoubleUpRight, roomPosition, conditions));
                    }
                    else if (roomBelow && roomLeft)
                    {
                        roomList.Add(RandomRoom(RoomTypes.DoubleDownLeft, roomPosition, conditions));
                    }
                    else if (roomBelow && roomRight)
                    {
                        roomList.Add(RandomRoom(RoomTypes.DoubleDownRight, roomPosition, conditions));
                    }
                    else
                    {
                        Debug.LogError("three way room error no logic found");
                    }
                    break;
                case 3:
                    if (roomAbove && roomBelow && roomLeft)
                    {
                        roomList.Add(RandomRoom(RoomTypes.trippleUpDownLeft, roomPosition, conditions));
                    }
                    else if (roomAbove && roomBelow && roomRight)
                    {
                        roomList.Add(RandomRoom(RoomTypes.trippleUpDownRight, roomPosition, conditions));
                    }
                    else if (roomAbove && roomLeft && roomRight)
                    {
                        roomList.Add(RandomRoom(RoomTypes.trippleUpLeftRight, roomPosition, conditions));
                    }
                    else if (roomBelow && roomLeft && roomRight)
                    {
                        roomList.Add(RandomRoom(RoomTypes.trippleDownLeftRight, roomPosition, conditions));
                    }
                    break;
                case 4:
                    roomList.Add(RandomRoom(RoomTypes.Fourway, roomPosition, conditions));
                    break;
                default:
                    //Debug.LogError("found no room exisits");
                    break;
            }
        }
        private GameObject RandomRoom(RoomPrefabSubTypes selection, Vector3 position, RoomConditions conditions)
        {
            GameObject roomType = null;

            //make it so the enterance and exit will never be a hallway
            switch (conditions)
            {
                case RoomConditions.StairRoom:

                    roomType = RoomTypes.getRandomRoom(selection, RoomPlacementLogic.RoomType.OpenRoom);

                    break;
                case RoomConditions.TreasureRoom:

                    roomType = RoomTypes.getRandomRoom(selection, RoomPlacementLogic.RoomType.TreasureRoom);
                    break;
                case RoomConditions.CombatRoom:
                    roomType = RoomTypes.getRandomRoom(selection, RoomPlacementLogic.RoomType.CombatRoom);
                    break;
                case RoomConditions.DefaultRoom:

                    //everything but treasure room and hidden rooms
                    roomType = RoomTypes.getRandomRoom(selection, RoomPlacementLogic.RoomType.OpenRoom); // RoomTypes.getRandomRoom(selection);

                    /*
                    while (roomType.GetComponent<RoomPlacementLogic>().roomType == RoomPlacementLogic.RoomType.TreasureRoom || roomType.GetComponent<RoomPlacementLogic>().roomType == RoomPlacementLogic.RoomType.CombatRoom)
                    {
                        roomType = RoomTypes.getRandomRoom(selection);
                    }
                    */
                    break;
            }

            if (roomType == null)
            {
                Debug.LogError("Room creation returned Null. Issue with setup " + selection.ToString() + " conditions " + conditions.ToString());
            }

            GameObject room = Instantiate(roomType);
            room.transform.parent = RoomParent.transform;
            room.transform.localPosition = position;

            return room;
        }
        #endregion

        private void calcuateSpecailRooms()
        {
            List<int> availableRooms = new List<int>();

            for (int i = 0; i < roomPositions.Count; i++)
            {
                availableRooms.Add(i);
            }

            /*
            //combatRooms first
            for (int i = 0; i < availableRooms.Count; i++)
            {
                float random = Random.Range(0.0f, 1.0f);
                
                if (random <= ChanceForCombatRoom)
                {
                    combatRoomIndexs.Add(availableRooms[i]);
                }
            }
            //remove availableRooms
            for(int i = 0; i< combatRoomIndexs.Count; i++)
            {
                availableRooms.Remove(combatRoomIndexs[i]);
            }
            */


            //treasure rooms
            for (int i = 1; i < availableRooms.Count; i++) //fist and last room can't be treasure rooms
            {
                if (treasureRoomIndexs.Count < MaxChestCount)
                {
                    float random = Random.Range(0.0f, 1.0f);
                    if (random <= ChanceForTreasureRoom)
                    {
                        treasureRoomIndexs.Add(availableRooms[i]);
                    }
                }
                else
                {
                    break;
                }
            }
            
            //remove availableRooms
            for (int i = 0; i < treasureRoomIndexs.Count; i++)
            {
                availableRooms.Remove(treasureRoomIndexs[i]);
            }
        }

        void AddDoorLogic(RoomPlacementLogic room)
        {
            Vector3 roomPosition = room.transform.position;
            bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffset, 0), 0.2f, roomMask);
            bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), 0.2f, roomMask);

            bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0), 0.2f, roomMask);
            bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0, 0), 0.2f, roomMask);

            for (int i = 0; i < roomList.Count; i++)
            {
                RoomPlacementLogic newRoom = roomList[i].GetComponent<RoomPlacementLogic>();
                if (roomAbove)
                {
                    if (roomList[i].gameObject.transform.localPosition == (roomPosition + new Vector3(0, yOffset, 0)))
                    {
                        if (room.upDoor == false && newRoom.downDoor == false && (room.getDoorRequest() + newRoom.getDoorRequest()) != 0)
                        {
                            room.upDoor = true;
                            newRoom.downDoor = true;
                            GameObject door = Instantiate(RoomObjPrefabs.DoubleDoorStraight, RoomProps.transform);
                            door.GetComponent<Transform>().position = room.transform.position + new Vector3(0, yOffset / 2, 0);
                            doorList.Add(door);
                        }
                    }
                }
                if (roomBelow)
                {
                    if (roomList[i].gameObject.transform.localPosition == (roomPosition + new Vector3(0, -yOffset, 0)))
                    {
                        if (room.downDoor == false && newRoom.upDoor == false && (room.getDoorRequest() + newRoom.getDoorRequest()) != 0)
                        {
                            room.downDoor = true;
                            newRoom.upDoor = true;
                            GameObject door = Instantiate(RoomObjPrefabs.DoubleDoorStraight, RoomProps.transform);
                            door.GetComponent<Transform>().position = room.transform.position + new Vector3(0, -yOffset / 2, 0);
                            doorList.Add(door);
                        }
                    }
                }
                if (roomLeft)
                {
                    if (roomList[i].gameObject.transform.localPosition == (roomPosition + new Vector3(-xOffset, 0, 0)))
                    {
                        if (room.leftDoor == false && newRoom.rightDoor == false && (room.getDoorRequest() + newRoom.getDoorRequest()) != 0)
                        {
                            room.leftDoor = true;
                            newRoom.rightDoor = true;
                            GameObject door = Instantiate(RoomObjPrefabs.DoubleDoorSide, RoomProps.transform);
                            door.GetComponent<Transform>().position = room.transform.position + new Vector3(-xOffset / 2, 0, 0);
                            doorList.Add(door);
                        }
                    }
                }
                if (roomRight)
                {
                    if (roomList[i].gameObject.transform.localPosition == (roomPosition + new Vector3(xOffset, 0, 0)))
                    {
                        if (room.rightDoor == false && newRoom.leftDoor == false && (room.getDoorRequest() + newRoom.getDoorRequest()) != 0)
                        {
                            room.rightDoor = true;
                            newRoom.leftDoor = true;
                            GameObject door = Instantiate(RoomObjPrefabs.DoubleDoorSide, RoomProps.transform);
                            door.GetComponent<Transform>().position = room.transform.position + new Vector3(xOffset / 2, 0, 0);
                            doorList.Add(door);
                        }
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class RoomPrefabs
    {
        [SerializeField] public RoomPrefabSubTypes singleUp;
        public RoomPrefabSubTypes singleDown;
        public RoomPrefabSubTypes singleLeft;
        public RoomPrefabSubTypes singleRight;

        public RoomPrefabSubTypes DoubleLeftRight;
        public RoomPrefabSubTypes DoubleUpDown;
        public RoomPrefabSubTypes DoubleUpLeft;
        public RoomPrefabSubTypes DoubleUpRight;
        public RoomPrefabSubTypes DoubleDownLeft;
        public RoomPrefabSubTypes DoubleDownRight;

        public RoomPrefabSubTypes trippleDownLeftRight;
        public RoomPrefabSubTypes trippleUpLeftRight;
        public RoomPrefabSubTypes trippleUpDownLeft;
        public RoomPrefabSubTypes trippleUpDownRight;

        public RoomPrefabSubTypes Fourway;

        public GameObject getRandomRoom(RoomPrefabSubTypes roomObject)
        {
            return roomObject.getRandomRoom();
        }
        public GameObject getRandomRoom(RoomPrefabSubTypes roomObject, RoomPlacementLogic.RoomType lookingFor)
        {
            return roomObject.getRandomRoom(lookingFor);
        }
    }
    [System.Serializable]
    public class RoomObjectPrefabs
    {
        public GameObject StairsUp;
        public GameObject StairsDown;
        public GameObject DoubleDoorStraight;
        public GameObject DoubleDoorSide;
        public GameObject Chests;
        public GameObject Crates;

    }

    [System.Serializable]
    public class RoomPrefabSubTypes
    {

        public List<ListWrapper> roomSubTypes; //it a unity thing. cant do list<list<>> in the editor

        public GameObject getRandomRoom()
        {
            int choice1 = Random.Range(0, roomSubTypes.Count); //total number of Lists
            int choice2 = Random.Range(0, roomSubTypes[choice1].myList.Count);

            //Debug.Log("choose: " + roomSubTypes[choice1].myList[choice2].GetComponent<RoomPlacementLogic>().roomType);

            return roomSubTypes[choice1].myList[choice2];
        }
        public GameObject getRandomRoom(RoomPlacementLogic.RoomType roomType)
        {
            ListWrapper roomGroup = null;
            for (int i = 0; i < roomSubTypes.Count; i++)
            {
                if (roomSubTypes[i].roomType == roomType)
                {
                    roomGroup = roomSubTypes[i];
                    break;
                }
            }
            if (roomGroup != null)
            {
                int choice = Random.Range(0, roomGroup.myList.Count);
                return roomGroup.myList[choice];
            }


            return null;
        }
    }
    [System.Serializable]
    public class ListWrapper
    {
        public string name; //for editor
        public RoomPlacementLogic.RoomType roomType;
        public List<GameObject> myList;
    }
}

