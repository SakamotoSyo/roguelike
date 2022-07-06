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

    [Header("�A�C�e���̎��")]
    [SerializeField]private ItemType _itemType;
    public ItemType GetItemType => _itemType;

    [Header("�A�C�e���̖��O")]
    [SerializeField] private string _itemName;
    public string GetItemName => _itemName;

    [Header("���ʂ̒l")]
    [SerializeField] private float _itemEffectNum;
    public float GetItemEffect => _itemEffectNum;

    [Header("�A�C�e���Ɋւ�����")]
    [SerializeField]private string _itemInformationText;
    public string GetItemInformationText => _itemInformationText;

    [Header("�A�C�e���̃C���[�W")]
    [SerializeField] private Sprite _itemImage;
    public Sprite GetItemImage => _itemImage;

   [Header("�A�C�e�����ʂ̎��")]
   [SerializeField]private ItemEffectType _itemEffectType;
   public ItemEffectType GetEffectType => _itemEffectType; 
}
