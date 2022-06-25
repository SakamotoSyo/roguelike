using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
[CreateAssetMenu(fileName = "ItemDataBase", menuName = "ItemDataBase")]
public class ItemDataBase : ScriptableObject
{

    public List<Item> ItemList = new List<Item>();

    public List<Item> GetItemLists()
    {
        return ItemList;
    }

}
