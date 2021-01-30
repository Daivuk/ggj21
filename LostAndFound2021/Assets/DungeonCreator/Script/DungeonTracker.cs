using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

namespace LostAndFound.Dungeon
{
    public class DungeonTracker : MonoBehaviour
    {
        public static DungeonTracker instance;
        public string dungeonName;
        public int currentFloor;
        public GameObject levelGeneratorPrefab;
        public List<LevelGenerator> floors;

        public LevelGenerator activeFloor;
        public string EntranceName;
        public List<SpecialFloors> specialFloors;

        public UnityEvent TravelDown;
        public UnityEvent TravelUp;

        public float chancesForCombatRoom;
        public float chancesForChestRoom;
        public float chancesForSpecialRoom;

        public int maxChestOnLevel;
        public DungeonChestDropList dropList;

        public DungeonEnemySpawnList EnemySpawnList;

        public Transform partyMemebers;

        [HideInInspector]  public bool travelingBetweenFloors;

        [HideInInspector] public bool finishedLoadingLevel;

        private bool dungeonActive = false;
        private bool dungeonLoading;
        private void Awake()
        {
            if (DungeonTracker.instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
                dungeonLoading = false;
            }
            else
            {
                DungeonTracker.instance.LoadFloor();
                Destroy(gameObject);
            }
        }

        public void StartDungeon()
        {
            if (dungeonLoading == false)
            {
                travelingBetweenFloors = true;
                PlayerController.instance.getFocusObject().SetActive(false);
                dungeonLoading = true;
                floors = new List<LevelGenerator>();
                currentFloor = 0;
                finishedLoadingLevel = false;

                LevelGenerator floor = Instantiate(levelGeneratorPrefab, this.gameObject.transform).GetComponent<LevelGenerator>();

                floor.initGenerator();
                floors.Add(floor);
                LoadFloor();
                dungeonActive = true;

                finishedLoadingLevel = true; //telling all the chest and enemy to launch
                travelingBetweenFloors = false;
                PlayerController.instance.getFocusObject().transform.position = activeFloor.upStairs.playerOffset.position;
                PlayerController.instance.getFocusObject().SetActive(true);
            }
            else if(floors.Count >= 1)
            {
                currentFloor = 0;
                LoadFloor();
                PlayerController.instance.getFocusObject().SetActive(false);
                PlayerController.instance.getFocusObject().transform.position = activeFloor.upStairs.playerOffset.position;
                PlayerController.instance.getFocusObject().SetActive(true);
            }

            
        }

        public void LoadFloor()
        {
            PlayerController.instance.LockedCharacter = false;
            if (currentFloor < 0)
            {
                currentFloor = 0;
            }
            activeFloor = floors[currentFloor]; //start at 1
            activeFloor.gameObject.SetActive(true);
            //

            // partyMemebers.gameObject.SetActive(true);

            //MenuController.instance.locationInfo.displayNewLocationName(dungeonName + " F" + (currentFloor + 1));
        }

        public void setPlayerToStartingStairs()
        {
            PlayerController.instance.getFocusObject().transform.position = activeFloor.upStairs.playerOffset.position;
        }
        #region stair logic

        public void Update()
        {
            if (dungeonActive == false) return;

            //hope this improve frame rate
            for(int i =0; i < activeFloor.EnemyList.Count; i++)
            {
                float distanceFromPlayer = Vector3.Distance(PlayerController.instance.getFocusObject().transform.position, activeFloor.EnemyList[i].transform.position);
                if( distanceFromPlayer < 15) //activeFloor.EnemyList[i].InCombat ||
                {
                    activeFloor.EnemyList[i].gameObject.SetActive(true);
                    //activeFloor.EnemyList[i].checkIfAlive();
                }
                else
                {
                    activeFloor.EnemyList[i].gameObject.SetActive(false);
                }
            }
        }

        public void Down()
        {
            if(DungeonTracker.instance.travelingBetweenFloors == false)
            {
                DungeonTracker.instance.travelingBetweenFloors = true; //stop the event being called twice while transitioning
                currentFloor++;
                StartCoroutine(ChangeFloorSequence(1));
            }
        }
        
        public void Up()
        {
            if (DungeonTracker.instance.travelingBetweenFloors == false)
            {
                DungeonTracker.instance.travelingBetweenFloors = true; //stop the event being called twice while transitioning
                currentFloor--;
                StartCoroutine(ChangeFloorSequence(-1));
            }
        }

        private IEnumerator ChangeFloorSequence(int floorDrop)
        {
            //SceneManagerController.instance.sceneTransitionController.playScene(SceneTransitionType.Fade, false);

            //yield return SceneManagerController.instance.sceneTransitionController.FinishedTransitionCoroutine();

            //PlayerController.instance.LockedCharacter = false;
            //CombatController.instance.ForceCombatEnding();

            activeFloor.gameObject.SetActive(false);
            bool SpecialFloor = false;
            for (int i = 0; i < specialFloors.Count; i++)
            {
                if (currentFloor == specialFloors[i].floorID)
                {
                    if (specialFloors[i].teleportCurrentScene && specialFloors[i].loadNewScene == false)
                    {
                        GameObject spawnLocation = GameObject.Find(specialFloors[i].spawnPoint);
                        PlayerController.instance.getFocusObject().transform.position = spawnLocation.transform.position;
                    }

                    //LaunchNewScene(specialFloors[i].loadingScene, specialFloors[i].spawnPoint);
                    SpecialFloor = true;
                }
            }


            if (SpecialFloor == false)
            {
                if ((floors.Count - 1) < currentFloor)
                {
                    //create floor
                    LevelGenerator floor = Instantiate(levelGeneratorPrefab, this.gameObject.transform).GetComponent<LevelGenerator>();
                    floor.initGenerator();
                    floors.Add(floor);
                    activeFloor = floor;
                    LoadFloor();
                }
                else
                {
                    activeFloor = floors[currentFloor]; //start at 1
                    activeFloor.gameObject.SetActive(true);
                }


                if(floorDrop > 0)
                {
                    PlayerController.instance.getFocusObject().transform.position = activeFloor.upStairs.playerOffset.position;
                }
                else
                {
                    PlayerController.instance.getFocusObject().transform.position = activeFloor.downStairs.playerOffset.position;
                }

                /*
                for (int i = 0; i < CombatController.instance.Allies.Count; i++)
                {
                    if (CombatController.instance.Allies[i] != null)
                    {
                        CombatController.instance.Allies[i].gameObject.GetComponent<AIController>().StopNavMeshAgent(true); //true off

                        //placing them by the stairs
                        if (floorDrop < 0)
                        {
                            CombatController.instance.Allies[i].gameObject.transform.position = activeFloor.downStairs.playerOffset.position;
                        }
                        else if (floorDrop > 0)
                        {
                            CombatController.instance.Allies[i].gameObject.transform.position = activeFloor.upStairs.playerOffset.position;
                        }

                        if (CombatController.instance.Allies[i].playerControlled == false)
                        {
                            CombatController.instance.Allies[i].gameObject.GetComponent<AIController>().StopNavMeshAgent(false); //true on
                        }
                    }
                }
                */
                //MasterParticelSystemHandler.Instance.ClearParticleSystem();
                //MenuController.instance.locationInfo.displayNewLocationName(dungeonName + " F" + (currentFloor + 1));

                //SceneManagerController.instance.sceneTransitionController.playScene(SceneTransitionType.Fade, true);

               // yield return SceneManagerController.instance.sceneTransitionController.FinishedTransitionCoroutine();

            }

            DungeonTracker.instance.travelingBetweenFloors = false;
            yield return null;
        }

        public void LaunchNewScene(string SceneName, string spawnLocation)
        {
            //EventTransitionManager.instance.SceneTransition(SceneName, spawnLocation, SceneTransitionType.circle_player, true);
        }

        public EnemyDrop getFloorEnemyList()
        {
            return instance.EnemySpawnList.getRandomEnemySpawn(instance.currentFloor);
        }

        public void EscapeDungeon(string sceneName)
        {
            DungeonTracker.instance.travelingBetweenFloors = true; //stop the event being called twice while transitioning
            currentFloor = -1;
            StartCoroutine(ChangeFloorSequence(0));
        }

        /*
        public void removePartyMemeberObject(string name)
        {
            for(int i = 0; i < partyMemebers.childCount; i++)
            {
                if(partyMemebers.GetChild(i).GetComponent<Attacker>().CharacterSaveName == name)
                {
                    Destroy(partyMemebers.GetChild(i).gameObject);
                    break;
                }
            }
        } 
        */
        #endregion
    }

    [System.Serializable]
    public class SpecialFloors
    {
        public int floorID;
        public bool loadNewScene;
        public bool teleportCurrentScene;
        public string loadingScene;
        public string spawnPoint;
    }
}


