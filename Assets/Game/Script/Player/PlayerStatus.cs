using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStatus : MonoBehaviour, IDamageble
{
    [Tooltip("GameManagerのインスタンス")]
    private GameManager _gameManager;

    /// <summary>現在のレベルを返す</summary>
    public int Level => _playerLevel;
    /// <summary>現在の攻撃力を返す</summary>
    public float Power => _power;
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
    private int _maxHp;
    [SerializeField, Header("HP")]
    private float _playerHp;
    [SerializeField, Header("攻撃力")]
    private float _power;
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
    /// <summary>levelが変わった時に通知する</summary>
    public event Action<int> OnLevelChanged;

    void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void Update()
    {

    }

    /// <summary>
    /// 現在のレベルを変更できる
    /// </summary>
    public void SetLevel(int level) 
    {
        _playerLevel = level;
        OnLevelChanged(level);
        Debug.Log("このメソッドが呼ばれました");
    }

    /// <summary>
    /// Hpの値を変更する
    /// </summary>
    /// <param name=""></param>
    public void SetHp(float value)
    {
        _playerHp = Mathf.Min(_playerHp += value, _maxHp);
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damage">食らうダメージ</param>
    public void AddDamage(float damage, GameObject obj)
    {
        LogScript.Instance.OutPutLog($"{damage}のダメージを受けた");
        _playerHp -= damage;
    }

    /// <summary>
    /// 経験値の取得する処理を行うメソッド
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
