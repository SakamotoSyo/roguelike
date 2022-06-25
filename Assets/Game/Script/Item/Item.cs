using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable][CreateAssetMenu(fileName = "ItemData", menuName = "ItemData")]
public class Item : ScriptableObject
{
    public enum ItemType 
    {
        RecoveryItem,
        PowerUpItem,
        SpecialItem, 

    } 

    [SerializeField ,Header("アイテムの種類")]
    private ItemType _itemType;
    public ItemType GetItemType => _itemType;

    [SerializeField,Header("アイテムの名前")]
     private string _itemName;
    public string GetItemName => _itemName;

    [SerializeField,Header("効果の値")]
    private float _itemEffectNum;
    public float GetItemEffect => _itemEffectNum;

    [SerializeField,Header("アイテムに関する情報")]
    private string _itemInformationText;
    public string GetItemInformationText => _itemInformationText;
}
