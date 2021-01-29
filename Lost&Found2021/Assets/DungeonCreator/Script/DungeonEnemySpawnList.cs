using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Core/Dungeon/EnemySpawnList")]
public class DungeonEnemySpawnList : ScriptableObject
{
    public List<EnemyLevelGrouping> CoreList;

    public EnemyDrop getRandomEnemySpawn(int currentLevel)
    {
        List<EnemyLevelGrouping> masterGroup = new List<EnemyLevelGrouping>();

        for(int i = 0; i < CoreList.Count; i++)
        {
            if(currentLevel >= CoreList[i].MinMaxLevel.x && currentLevel < CoreList[i].MinMaxLevel.y)
            {
                masterGroup.Add(CoreList[i]);
            }
        }


        //find enemy group
        int random = Random.Range(0, masterGroup.Count);
        int randomEnemy = Random.Range(0, masterGroup[random].DropList.Count);


        EnemyDrop enemy = masterGroup[random].DropList[randomEnemy];
        masterGroup.Clear();

        return enemy;
    }
}
[System.Serializable]
public class EnemyLevelGrouping
{
    public string name;
    public Vector2 MinMaxLevel;
    public List<EnemyDrop> DropList;
}
[System.Serializable]
public class EnemyDrop
{
    public string name;
    public List<GameObject> Enemies;
}
