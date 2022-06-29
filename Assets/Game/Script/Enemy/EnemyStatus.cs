using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField][Tooltip("�ő�HP")] private int _maxHp;
    [SerializeField][Tooltip("HP")] private float _hp;
    [SerializeField][Tooltip("�U����")] private float _power;
    [SerializeField][Tooltip("�s���̉�")] private int _actionNum;
    //public int MaxHp1 { get => MaxHp; set => MaxHp = value; }

    
    /// <summary>
    /// �ő�Hp
    /// </summary>
    /// <param name="hp"></param>
    public void SetMaxHp(int hp)
    {
        this._maxHp = hp;
    }

    public int GetMaxHp() => _maxHp;

    /// <summary>
    /// ���݂�Hp
    /// </summary>
    /// <param name="hp"></param>
    public void SetHp(float hp)
    {
        this._hp = Mathf.Max(0, Mathf.Min(GetMaxHp(), hp));
    }


    public float GetHp() => _hp;

    /// <summary>
    /// ���݂̍U����
    /// </summary>
    /// <param name="power">��</param>
    public void SetPower(float power)
    {
        this._power = power;
    }

    public float GetPower() => _power;
}
