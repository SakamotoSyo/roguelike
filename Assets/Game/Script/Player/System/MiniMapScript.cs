using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    [Header("‚Ç‚Ì‚­‚ç‚¢Player‚Ìü‚è‚ğ–¾‚é‚­‚·‚é‚©")]
    [SerializeField] int _searchNum = 0;

    [Tooltip("MiniMap‚É•\¦‚·‚é‚½‚ß‚ÌObject")]
    [SerializeField] GameObject _miniMapObj;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            _miniMapObj.SetActive(false);
        }
        
    }
}

    ///// <summary>Player‚ªˆÚ“®‚µ‚½‚Æ‚«‚ÉMap‚ğXV‚·‚é</summary>
    //void MapUpdate() 
    //{
    //    var PosNum  = Mathf.Abs(_gameManager.PlayerObj.transform.position.x - transform.position.x) 
    //        + Mathf.Abs(_gameManager.PlayerObj.transform.position.y - transform.position.y);

    //    if (PosNum < _searchNum) 
    //    {
    //        _miniMapObj.SetActive(false);
    //    }

    //}