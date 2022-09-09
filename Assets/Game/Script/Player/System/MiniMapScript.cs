using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    [Header("�ǂ̂��炢Player�̎���𖾂邭���邩")]
    [SerializeField] int _searchNum = 0;

    [Tooltip("MiniMap�ɕ\�����邽�߂�Object")]
    [SerializeField] GameObject _miniMapObj;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            _miniMapObj.SetActive(false);
        }
        
    }
}

    ///// <summary>Player���ړ������Ƃ���Map���X�V����</summary>
    //void MapUpdate() 
    //{
    //    var PosNum  = Mathf.Abs(_gameManager.PlayerObj.transform.position.x - transform.position.x) 
    //        + Mathf.Abs(_gameManager.PlayerObj.transform.position.y - transform.position.y);

    //    if (PosNum < _searchNum) 
    //    {
    //        _miniMapObj.SetActive(false);
    //    }

    //}