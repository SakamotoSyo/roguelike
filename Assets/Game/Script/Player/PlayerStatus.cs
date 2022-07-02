using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IDamageble
{
    /// <summary> 装備している武器を返す</summary>
    public Item WeaponEquip => _weaponEquip;
    /// <summary> 装備している盾を返す</summary>
    public Item ShieldEquip => _shieldEquip;
    /// <summary> アイテムリストを返す</summary>
    public List<Item> PlayerItemList => _playerItemList;

    [SerializeField, Header("最大HP")]
    private int _maxHp;
    [SerializeField, Header("HP")]
    private float _playerHp;
    [SerializeField, Header("攻撃力")]
    private float _power;
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
    public void AddDamage(float damage)
    {
        Debug.Log($"プレイヤーは{damage}のダメージを受けた");
        _playerHp -= damage;
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
