using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    GameManager _gameManager;

    [Header("どのくらいPLあいぇｒの周りを明るくするか")]
    [SerializeField] int _searchNum = 0;

    [Tooltip("MiniMapに表示するためのObject")]
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

    /// <summary>Playerが移動したときにMapを更新する</summary>
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
