using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    [Header("どのくらいPlayerの周りを明るくするか")]
    [SerializeField] int _searchNum = 0;

    [Tooltip("MiniMapに表示するためのObject")]
    [SerializeField] GameObject _miniMapObj;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            _miniMapObj.SetActive(false);
        }
        
    }
}

    ///// <summary>Playerが移動したときにMapを更新する</summary>
    //void MapUpdate() 
    //{
    //    var PosNum  = Mathf.Abs(_gameManager.PlayerObj.transform.position.x - transform.position.x) 
    //        + Mathf.Abs(_gameManager.PlayerObj.transform.position.y - transform.position.y);

    //    if (PosNum < _searchNum) 
    //    {
    //        _miniMapObj.SetActive(false);
    //    }

    //}