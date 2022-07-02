using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IDamageble
{
    /// <summary> �������Ă��镐���Ԃ�</summary>
    public Item WeaponEquip => _weaponEquip;
    /// <summary> �������Ă��鏂��Ԃ�</summary>
    public Item ShieldEquip => _shieldEquip;
    /// <summary> �A�C�e�����X�g��Ԃ�</summary>
    public List<Item> PlayerItemList => _playerItemList;

    [SerializeField, Header("�ő�HP")]
    private int _maxHp;
    [SerializeField, Header("HP")]
    private float _playerHp;
    [SerializeField, Header("�U����")]
    private float _power;
    [SerializeField, Header("�s���̉�")]
    private int _actionNum;
    [SerializeField, Header("�������Ă��镐��")]
    private Item _weaponEquip;
    [SerializeField, Header("�������Ă��鏂")]
    private Item _shieldEquip;
    [SerializeField, Header("�A�C�e���̏������X�g")]
    private List<Item> _playerItemList;
    [SerializeField, Header("ItemDateBase")]
    private ItemDataBase _itemDataBase;


    /// <summary>
    /// Hp�̒l��ύX����
    /// </summary>
    /// <param name=""></param>
    public void SetHp(float value) 
    {
        _playerHp = Mathf.Min(_playerHp += value, _maxHp);
    }

    /// <summary>
    /// �_���[�W���󂯂鏈��
    /// </summary>
    /// <param name="damage">�H�炤�_���[�W</param>
    public void AddDamage(float damage)
    {
        Debug.Log($"�v���C���[��{damage}�̃_���[�W���󂯂�");
        _playerHp -= damage;
    }

    /// <summary>
    /// �A�C�e���f�[�^�[�x�[�X����A�C�e�����ŃA�C�e���f�[�^���擾����
    /// </summary>
    /// <param name="searchName">�A�C�e����</param>
    /// <returns></returns>
    public Item GetItem(string searchName)
    {
        return _itemDataBase.GetItemLists().Find(itemName => itemName.GetItemName == searchName);
    }

    /// <summary>
    /// �v���C���[�ɃA�C�e�����Z�b�g����
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(Item item)
    {
        _playerItemList.Add(item);
    }

    /// <summary>
    /// �A�C�e�����X�g����A�C�e�����폜����
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item) 
    {
        _playerItemList.Remove(item);
    }

   
}
