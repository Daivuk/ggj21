using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Core/Dungeon/ChestDropList")]
public class DungeonChestDropList : ScriptableObject
{
    public List<ItemChances> itemDrop;

    public ItemChances getItemDrop()
    {
        int random = Random.Range(0, 100);

        for(int i = 0; i < itemDrop.Count; i++)
        {
           
            bool min = random >= itemDrop[i].chance.x;
            bool max = random < itemDrop[i].chance.y;
            if (min && max)
            {
                return itemDrop[i];
            }
        }
        return itemDrop[0];
        
    }
}
[System.Serializable]
public class ItemChances
{
    public ItemBaseStat item;
    public Vector2 chance;
    public Vector2 StackAmount;
}