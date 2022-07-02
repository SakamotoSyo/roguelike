using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectScript : MonoBehaviour
{
    private SpriteRenderer _sp;

    private Item _itemInformation;
    public Item ItemInfomation => _itemInformation;

    private void Start()
    {
        
    }

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
