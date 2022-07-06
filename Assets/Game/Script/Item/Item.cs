using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable][CreateAssetMenu(fileName = "ItemData", menuName = "ItemData")]
public class Item : ScriptableObject
{
    public enum ItemType 
    {
        MagicBook,
        Candy,
        Weapon,
        Armor,
        SpecialItem, 

    }

    public enum ItemEffectType 
    {
        Hearing,
        Food,

    }

    [Header("アイテムの種類")]
    [SerializeField]private ItemType _itemType;
    public ItemType GetItemType => _itemType;

    [Header("アイテムの名前")]
    [SerializeField] private string _itemName;
    public string GetItemName => _itemName;

    [Header("効果の値")]
    [SerializeField] private float _itemEffectNum;
    public float GetItemEffect => _itemEffectNum;

    [Header("アイテムに関する情報")]
    [SerializeField]private string _itemInformationText;
    public string GetItemInformationText => _itemInformationText;

    [Header("アイテムのイメージ")]
    [SerializeField] private Sprite _itemImage;
    public Sprite GetItemImage => _itemImage;

   [Header("アイテム効果の種類")]
   [SerializeField]private ItemEffectType _itemEffectType;
   public ItemEffectType GetEffectType => _itemEffectType; 
}
