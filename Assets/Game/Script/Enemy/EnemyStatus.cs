using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour, IDamageble
{
    public float Exp => _enemyExp;
    public float GetHp => _hp;
    public int GetMaxHp => _maxHp;
    public float GetPower => _power;

    [Header("敵の名前")]
    [SerializeField] string _enemyName;
    [Header("最大HP")]
    [SerializeField] int _maxHp;
    [Header("HP")]
    [SerializeField] float _hp;
    [Header("攻撃力")]
    [SerializeField] float _power = 2;
    [Header("行動の回数")]
    [SerializeField] int _actionNum;
    [Header("敵の経験値")]
    [SerializeField] float _enemyExp;
    [Header("階層によってかかるステータス倍率")]
    [SerializeField] float _statusUp;
    [Header("ダメージを受けるUI")]
    [SerializeField] GameObject _damageUI;
    [Header("Animator")]
    [SerializeField] Animator _anim;
    [Tooltip("EnemyBaseのスクリプト")]
    private EnemyBase _enemyBase;
    [Tooltip("GameManagerのインスタンス")]
    private GameManager _gameManager;
    //public int MaxHp1 { get => MaxHp; set => MaxHp = value; }

    void Start()
    {
        _gameManager = GameManager.Instance;
        StartStatus();
    }

    /// <summary>
    /// 階層によってステータスに倍率をかける
    /// </summary>
    void StartStatus()
    {
        _statusUp = 1.2f;
        _maxHp = (int)(_maxHp * _gameManager.NowFloor * _statusUp);
        _hp = _hp * _gameManager.NowFloor * _statusUp;
        _power = (int)(_power * _gameManager.NowFloor * _statusUp);
        _enemyExp = _enemyExp * _gameManager.NowFloor * _statusUp;
    }

    /// <summary>
    /// 最大Hp
    /// </summary>
    /// <param name="hp"></param>
    public void SetMaxHp(int hp)
    {
        _maxHp = hp;
    }

    /// <summary>
    /// 現在のHp
    /// </summary>
    /// <param name="hp"></param>
    public void SetHp(float hp)
    {
        _hp = Mathf.Max(0, Mathf.Min(GetMaxHp, hp));
    }

    /// <summary>
    /// 現在の攻撃力
    /// </summary>
    /// <param name="power">力</param>
    public void SetPower(float power)
    {
        _power = power;
    }


    public void SetExp(float exp)
    {
        _enemyExp = exp;
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damage">受けるダメージ</param>
    public void AddDamage(float damage, GameObject obj)
    {
        _hp -= damage;
        _anim.SetTrigger("Damage");
        LogScript.Instance.OutPutLog($"{damage}のダメージを与えた");
        //ダメージの数字を表示する処理
        var UI = Instantiate(_damageUI, transform.position, transform.rotation);
        UI.GetComponentInChildren<Text>().text = damage.ToString();

        if (_hp <= 0)
        {
            if (gameObject.name == "Boss") 
            {
                _gameManager.GameClearBool(true);
            }
            OnDeath(obj);
        }
        Debug.Log(damage);
    }

    /// <summary>自分自身が倒されたときに呼ばれる</summary>
    void OnDeath(GameObject obj)
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

        EnemyManager.Instance.SetNowEnemyNum(-1);
        Debug.Log("-1adsdas");
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
