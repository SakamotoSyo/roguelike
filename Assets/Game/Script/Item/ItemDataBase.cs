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

    /// <summary>
    /// �A�C�e�����X�g�̒��g�������_���ɕԂ�
    /// </summary>
    /// <returns></returns>
    public Item GetRandamItemLists() 
    {
        return ItemList[UnityEngine.Random.Range(0, ItemList.Count)];
    }

}
