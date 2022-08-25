using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour,IDamageble
{

    [Header("�G�̖��O")]
    [SerializeField] string _enemyName;
    [Header("�ő�HP")]
    [SerializeField] int _maxHp;
    [Header("HP")]
    [SerializeField] float _hp;
    [Header("�U����")]
    [SerializeField] float _power;
    [Header("�s���̉�")]
    [SerializeField] int _actionNum;
    [Header("�G�̌o���l")]
    [SerializeField] float _enemyExp;
    [Tooltip("EnemyBase�̃X�N���v�g")]
    private EnemyBase _enemyBase;
    [Tooltip("GameManager�̃C���X�^���X")]
    private GameManager _gameManager;
    //public int MaxHp1 { get => MaxHp; set => MaxHp = value; }

    private void Start()
    {
      _gameManager = GameManager.Instance;
    }

    /// <summary>
    /// �ő�Hp
    /// </summary>
    /// <param name="hp"></param>
    public void SetMaxHp(int hp)
    {
        _maxHp = hp;
    }

    public int GetMaxHp() => _maxHp;

    /// <summary>
    /// ���݂�Hp
    /// </summary>
    /// <param name="hp"></param>
    public void SetHp(float hp)
    {
        _hp = Mathf.Max(0, Mathf.Min(GetMaxHp(), hp));
    }


    public float GetHp() => _hp;

    /// <summary>
    /// ���݂̍U����
    /// </summary>
    /// <param name="power">��</param>
    public void SetPower(float power)
    {
        _power = power;
    }

    public float GetPower() => _power;

    public void SetExp(float exp) 
    {
        _enemyExp = exp;
    }

    public float Exp => _enemyExp;

    /// <summary>
    /// �_���[�W���󂯂鏈��
    /// </summary>
    /// <param name="damage">�󂯂�_���[�W</param>
    public void AddDamage(float damage, GameObject obj)
    {
         _hp -= damage;
        LogScript.Instance.OutPutLog($"{damage}�̃_���[�W��^����");
        if (_hp <= 0)
        {
            OnDeath(obj);
        }
        Debug.Log(damage);
    }

    /// <summary>�������g���|���ꂽ�Ƃ��ɌĂ΂��</summary>
    private void OnDeath(GameObject obj)
    {
        LogScript.Instance.OutPutLog("Enemy�͓|�ꂽ");
        if (obj == _gameManager.PlayerObj)
        {
            var data = new EnemyStatusData(_enemyName, _enemyExp);
            //EnemyManager����EnemyBase�̎Q�Ƃ��폜����
            EnemyManager.Instance.RemoveEnemyData(this.gameObject);
            //�v���C���[�����U���g�Ɏg���f�[�^��n��
            EnemyManager.Instance.SetEnemyStatusList(data);
            //�������g��Destroy����
            Destroy(this.gameObject);
        }

        EnemyManager.Instance.SetTotalEnemyNum(-1);
    }
}

public struct EnemyStatusData 
{
    public EnemyStatusData(string name, float exp) 
    {
        Name = name;
        Exp = exp;
    }

    public string Name;
    public float Exp;
}
