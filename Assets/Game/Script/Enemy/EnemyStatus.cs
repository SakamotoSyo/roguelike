using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField][Tooltip("ç≈ëÂHP")] private int _maxHp;
    [SerializeField][Tooltip("HP")] private float _hp;
    [SerializeField][Tooltip("çUåÇóÕ")] private float _power;
    [SerializeField][Tooltip("çsìÆÇÃâÒêî")] private int _actionNum;
    //public int MaxHp1 { get => MaxHp; set => MaxHp = value; }

    
    /// <summary>
    /// ç≈ëÂHp
    /// </summary>
    /// <param name="hp"></param>
    public void SetMaxHp(int hp)
    {
        this._maxHp = hp;
    }

    public int GetMaxHp() => _maxHp;

    /// <summary>
    /// åªç›ÇÃHp
    /// </summary>
    /// <param name="hp"></param>
    public void SetHp(float hp)
    {
        this._hp = Mathf.Max(0, Mathf.Min(GetMaxHp(), hp));
    }


    public float GetHp() => _hp;

    /// <summary>
    /// åªç›ÇÃçUåÇóÕ
    /// </summary>
    /// <param name="power">óÕ</param>
    public void SetPower(float power)
    {
        this._power = power;
    }

    public float GetPower() => _power;
}
