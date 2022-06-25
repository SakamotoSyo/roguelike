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

    [SerializeField ,Header("�A�C�e���̎��")]
    private ItemType _itemType;
    public ItemType GetItemType => _itemType;

    [SerializeField,Header("�A�C�e���̖��O")]
     private string _itemName;
    public string GetItemName => _itemName;

    [SerializeField,Header("���ʂ̒l")]
    private float _itemEffectNum;
    public float GetItemEffect => _itemEffectNum;

    [SerializeField,Header("�A�C�e���Ɋւ�����")]
    private string _itemInformationText;
    public string GetItemInformationText => _itemInformationText;
}
