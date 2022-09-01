using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class PlayerStatus : MonoBehaviour, IDamageble
{
    [Tooltip("GameManager�̃C���X�^���X")]
    private GameManager _gameManager;

    /// <summary>���݂̃��x����Ԃ�</summary>
    public int Level => _playerLevel;
    ///// <summary>���݂̍ő�HP��Ԃ�</summary>
    //public float MaxHp => _maxHp;
    ///// <summary>���݂�HP��Ԃ�</summary>
    //public float PlayerHp => _playerHp;
    /// <summary>���݂̍U���͂�Ԃ�</summary>
    public float Power => _playerPower;
    /// <summary>���x���A�b�v�܂ł̌o���l��Ԃ�</summary>
    public float EXP => _playerExp;
    /// <summary> �������Ă��镐���Ԃ�</summary>
    public Item WeaponEquip => _weaponEquip;
    /// <summary> �������Ă��鏂��Ԃ�</summary>
    public Item ShieldEquip => _shieldEquip;
    /// <summary> �A�C�e�����X�g��Ԃ�</summary>
    public List<Item> PlayerItemList => _playerItemList;

    [Header("���݂̃��x��")]
    [SerializeField] private int _playerLevel = 1;
    [SerializeField, Header("�ő�HP")]
    private float _setmaxHp;
    [SerializeField, Header("HP")]
    private float _playerHp;
    [SerializeField, Header("�U����")]
    private float _playerPower;
    [Header("���x���A�b�v�܂ł̎c��o���l")]
    private float _playerExp;
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
    [SerializeField, Header("LevelUpDataScript")]
    private LevelDataScript _levelDataScript;
    /// <summary>level���ς�������ɒʒm����</summary>
    public event Action<int> OnLevelChanged;

    /// <summary>MVP�p�^�[���ɂ�����Model�N���X</summary>
    public float MaxHp { get => _maxHp.Value; set => _maxHp.Value = value; }
    public IObservable<float> MaxChanged => _maxHp;
    private readonly ReactiveProperty<float> _maxHp = new ReactiveProperty<float>();

    public float CurrentHp { get => _currentHp.Value; set => _currentHp.Value = value; }
    public IObservable<float> CurrentChanged => _currentHp;
    private readonly ReactiveProperty<float> _currentHp = new ReactiveProperty<float>();

    void Awake()
    {
        Debug.Log($"{_maxHp.Value}��{_setmaxHp}���Z�b�g");
        _maxHp.Value = _setmaxHp;
        _currentHp.Value = _playerHp;
    }

    void Start()
    {
        _gameManager = GameManager.Instance;
    }

    /// <summary>
    /// ���݂̃��x����ύX�ł���
    /// </summary>
    public void LevelUpSetData(int level)
    {
        _playerLevel = level;
        //���x���A�b�v�������Ƃ�ʒm����
        OnLevelChanged(level);
        //���x���A�b�v�����X�e�[�^�X�f�[�^���擾����
        PlayerStatusData LevelUpData = _levelDataScript.GetLevelStatus(level);

        //���x���A�b�v�����f�[�^���Z�b�g
        float next = _maxHp.Value;
        next = LevelUpData.Maxhp;
        _maxHp.Value = next;
        _playerPower = LevelUpData.Attack;
        _playerExp = LevelUpData.Exp;

        Debug.Log("���̃��\�b�h���Ă΂�܂���");
    }


    /// <summary>
    /// Hp�̒l��ύX����
    /// </summary>
    /// <param name=""></param>
    public void SetHp(float value)
    {
        // �l�������o���E����������ۂ�Value�v���p�e�B���Q�Ƃ��邱��
        float _next = _currentHp.Value;
        _next = Mathf.Min(_next += value, MaxHp);
        _currentHp.Value = _next;
    }

    /// <summary>
    /// �_���[�W���󂯂鏈��
    /// </summary>
    /// <param name="damage">�H�炤�_���[�W</param>
    public void AddDamage(float damage, GameObject obj)
    {
        LogScript.Instance.OutPutLog($"{damage}�̃_���[�W���󂯂�");
        // �l�������o���E����������ۂ�Value�v���p�e�B���Q�Ƃ��邱��
        float _next = _currentHp.Value;
        _next -= damage;
        if (_next < 0)
        {
            //���ʂƂ��̏���
        }

        _currentHp.Value = _next;
    }

    /// <summary>
    /// �o���l�̑��ʂ�ύX���鏈�����s�����\�b�h
    /// </summary>
    /// <param name="expPoint"></param>
    public void SetExp(float expPoint)
    {
        _playerExp = expPoint;
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
