using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IDamageble
{
    [Tooltip("GameManager�̃C���X�^���X")]
    private GameManager _gameManager;

    /// <summary>���݂̍U���͂�Ԃ�</summary>
    public float Power => _power;
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
    [Header("���x���A�b�v�܂ł̎c��o���l")]
    private float _exp;
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

    void Start() 
    {
        _gameManager = GameManager.Instance;
    }

    private void Update()
    {
        
    }



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
    public void AddDamage(float damage, GameObject obj)
    {
        _gameManager.OutPutLog($"{damage}�̃_���[�W���󂯂�");
        _playerHp -= damage;
    }

    /// <summary>
    /// �o���l�̎擾���鏈�����s�����\�b�h
    /// </summary>
    /// <param name="expPoint"></param>
    public void SetExp(float expPoint)
    {
        _exp = expPoint;
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


    /// <summary>
    /// �G��|�������ɌĂ΂�郁�\�b�h
    /// </summary>
    public void GetResult(float exp) 
    {
        SetExp(exp);
    }
   
}
