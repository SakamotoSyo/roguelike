using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour,IDamageble
{

    [Header("敵の名前")]
    [SerializeField] string _enemyName;
    [Header("最大HP")]
    [SerializeField] int _maxHp;
    [Header("HP")]
    [SerializeField] float _hp;
    [Header("攻撃力")]
    [SerializeField] float _power;
    [Header("行動の回数")]
    [SerializeField] int _actionNum;
    [Header("敵の経験値")]
    [SerializeField] float _enemyExp;
    [Tooltip("EnemyBaseのスクリプト")]
    private EnemyBase _enemyBase;
    [Tooltip("GameManagerのインスタンス")]
    private GameManager _gameManager;
    //public int MaxHp1 { get => MaxHp; set => MaxHp = value; }

    private void Start()
    {
      _gameManager = GameManager.Instance;
    }

    /// <summary>
    /// 最大Hp
    /// </summary>
    /// <param name="hp"></param>
    public void SetMaxHp(int hp)
    {
        _maxHp = hp;
    }

    public int GetMaxHp() => _maxHp;

    /// <summary>
    /// 現在のHp
    /// </summary>
    /// <param name="hp"></param>
    public void SetHp(float hp)
    {
        _hp = Mathf.Max(0, Mathf.Min(GetMaxHp(), hp));
    }


    public float GetHp() => _hp;

    /// <summary>
    /// 現在の攻撃力
    /// </summary>
    /// <param name="power">力</param>
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
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damage">受けるダメージ</param>
    public void AddDamage(float damage, GameObject obj)
    {
         _hp -= damage;
        LogScript.Instance.OutPutLog($"{damage}のダメージを与えた");
        if (_hp <= 0)
        {
            OnDeath(obj);
        }
        Debug.Log(damage);
    }

    /// <summary>自分自身が倒されたときに呼ばれる</summary>
    private void OnDeath(GameObject obj)
    {
        LogScript.Instance.OutPutLog("Enemyは倒れた");
        if (obj == _gameManager.PlayerObj)
        {
            var data = new EnemyStatusData(_enemyName, _enemyExp);
            //EnemyManagerからEnemyBaseの参照を削除する
            EnemyManager.Instance.RemoveEnemyData(this.gameObject);
            //プレイヤーがリザルトに使うデータを渡す
            EnemyManager.Instance.SetEnemyStatusList(data);
            //自分自身をDestroyする
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
