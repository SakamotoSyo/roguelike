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
    /// �A�C�e���̏����Z�b�g����
    /// </summary>
    /// <param name="item"></param>
    public void SetItemInfor(Item item) 
    {
        _itemInformation = item;
    }

    /// <summary>
    /// �A�C�e���̃X�v���C�g���Z�b�g����
    /// </summary>
    /// <param name="sp"></param>
    public void SetItemSprite(Sprite sp)
    {
        _sp = GetComponent<SpriteRenderer>();
        _sp.sprite = sp;
    }

    /// <summary>
    /// Object��Destroy����
    /// </summary>
    public void DestroyObj() 
    {
        gameObject.SetActive(false);
        Destroy(this.gameObject, 5);
    }

}
