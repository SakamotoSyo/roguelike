using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectScript : MonoBehaviour
{
    SpriteRenderer _sp;

    Item _itemInformation;
    public Item ItemInfomation => _itemInformation;

    /// <summary>
    /// アイテムの情報をセットする
    /// </summary>
    /// <param name="item"></param>
    public void SetItemInfor(Item item) 
    {
        _itemInformation = item;
    }

    /// <summary>
    /// アイテムのスプライトをセットする
    /// </summary>
    /// <param name="sp"></param>
    public void SetItemSprite(Sprite sp)
    {
        _sp = GetComponent<SpriteRenderer>();
        _sp.sprite = sp;
    }

    /// <summary>
    /// ObjectをDestroyする
    /// </summary>
    public void DestroyObj() 
    {
        gameObject.SetActive(false);
        Destroy(this.gameObject, 5);
    }

}
