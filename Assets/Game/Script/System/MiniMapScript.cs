using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    GameManager _gameManager;

    [Header("�ǂ̂��炢PL���������̎���𖾂邭���邩")]
    [SerializeField] int _searchNum = 0;

    [Tooltip("MiniMap�ɕ\�����邽�߂�Object")]
    [SerializeField] GameObject _miniMapObj;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.PlayerObj.GetComponent<PlayerMove>().MiniMapUpdate += MapUpdate;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>Player���ړ������Ƃ���Map���X�V����</summary>
    void MapUpdate() 
    {
        var PosNum  = Mathf.Abs(_gameManager.PlayerObj.transform.position.x - transform.position.x) 
            + Mathf.Abs(_gameManager.PlayerObj.transform.position.y - transform.position.y);

        if (PosNum < _searchNum) 
        {
            _miniMapObj.SetActive(false);
        }

    }
}
