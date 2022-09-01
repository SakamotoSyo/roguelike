using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class PlayerStatus : MonoBehaviour, IDamageble
{
    [Tooltip("GameManagerのインスタンス")]
    private GameManager _gameManager;

    /// <summary>現在のレベルを返す</summary>
    public int Level => _playerLevel;
    ///// <summary>現在の最大HPを返す</summary>
    //public float MaxHp => _maxHp;
    ///// <summary>現在のHPを返す</summary>
    //public float PlayerHp => _playerHp;
    /// <summary>現在の攻撃力を返す</summary>
    public float Power => _playerPower;
    /// <summary>レベルアップまでの経験値を返す</summary>
    public float EXP => _playerExp;
    /// <summary> 装備している武器を返す</summary>
    public Item WeaponEquip => _weaponEquip;
    /// <summary> 装備している盾を返す</summary>
    public Item ShieldEquip => _shieldEquip;
    /// <summary> アイテムリストを返す</summary>
    public List<Item> PlayerItemList => _playerItemList;

    [Header("現在のレベル")]
    [SerializeField] private int _playerLevel = 1;
    [SerializeField, Header("最大HP")]
    private float _setmaxHp;
    [SerializeField, Header("HP")]
    private float _playerHp;
    [SerializeField, Header("攻撃力")]
    private float _playerPower;
    [Header("レベルアップまでの残り経験値")]
    private float _playerExp;
    [SerializeField, Header("行動の回数")]
    private int _actionNum;
    [SerializeField, Header("装備している武器")]
    private Item _weaponEquip;
    [SerializeField, Header("装備している盾")]
    private Item _shieldEquip;
    [SerializeField, Header("アイテムの所持リスト")]
    private List<Item> _playerItemList;
    [SerializeField, Header("ItemDateBase")]
    private ItemDataBase _itemDataBase;
    [SerializeField, Header("LevelUpDataScript")]
    private LevelDataScript _levelDataScript;
    /// <summary>levelが変わった時に通知する</summary>
    public event Action<int> OnLevelChanged;

    /// <summary>MVPパターンにおけるModelクラス</summary>
    public float MaxHp { get => _maxHp.Value; set => _maxHp.Value = value; }
    public IObservable<float> MaxChanged => _maxHp;
    private readonly ReactiveProperty<float> _maxHp = new ReactiveProperty<float>();

    public float CurrentHp { get => _currentHp.Value; set => _currentHp.Value = value; }
    public IObservable<float> CurrentChanged => _currentHp;
    private readonly ReactiveProperty<float> _currentHp = new ReactiveProperty<float>();

    void Awake()
    {
        Debug.Log($"{_maxHp.Value}に{_setmaxHp}をセット");
        _maxHp.Value = _setmaxHp;
        _currentHp.Value = _playerHp;
    }

    void Start()
    {
        _gameManager = GameManager.Instance;
    }

    /// <summary>
    /// 現在のレベルを変更できる
    /// </summary>
    public void LevelUpSetData(int level)
    {
        _playerLevel = level;
        //レベルアップしたことを通知する
        OnLevelChanged(level);
        //レベルアップしたステータスデータを取得する
        PlayerStatusData LevelUpData = _levelDataScript.GetLevelStatus(level);

        //レベルアップしたデータをセット
        float next = _maxHp.Value;
        next = LevelUpData.Maxhp;
        _maxHp.Value = next;
        _playerPower = LevelUpData.Attack;
        _playerExp = LevelUpData.Exp;

        Debug.Log("このメソッドが呼ばれました");
    }


    /// <summary>
    /// Hpの値を変更する
    /// </summary>
    /// <param name=""></param>
    public void SetHp(float value)
    {
        // 値を引き出す・書き換える際はValueプロパティを参照すること
        float _next = _currentHp.Value;
        _next = Mathf.Min(_next += value, MaxHp);
        _currentHp.Value = _next;
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damage">食らうダメージ</param>
    public void AddDamage(float damage, GameObject obj)
    {
        LogScript.Instance.OutPutLog($"{damage}のダメージを受けた");
        // 値を引き出す・書き換える際はValueプロパティを参照すること
        float _next = _currentHp.Value;
        _next -= damage;
        if (_next < 0)
        {
            //死ぬときの処理
        }

        _currentHp.Value = _next;
    }

    /// <summary>
    /// 経験値の総量を変更する処理を行うメソッド
    /// </summary>
    /// <param name="expPoint"></param>
    public void SetExp(float expPoint)
    {
        _playerExp = expPoint;
    }

    /// <summary>
    /// アイテムデーターベースからアイテム名でアイテムデータを取得する
    /// </summary>
    /// <param name="searchName">アイテム名</param>
    /// <returns></returns>
    public Item GetItem(string searchName)
    {
        return _itemDataBase.GetItemLists().Find(itemName => itemName.GetItemName == searchName);
    }

    /// <summary>
    /// プレイヤーにアイテムをセットする
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(Item item)
    {
        _playerItemList.Add(item);
    }

    /// <summary>
    /// アイテムリストからアイテムを削除する
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
        _playerItemList.Remove(item);
    }

}
