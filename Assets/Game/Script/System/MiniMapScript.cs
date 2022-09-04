using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    GameManager _gameManager;

    [Header("‚Ç‚Ì‚­‚ç‚¢PL‚ ‚¢‚¥‚’‚Ìü‚è‚ğ–¾‚é‚­‚·‚é‚©")]
    [SerializeField] int _searchNum = 0;

    [Tooltip("MiniMap‚É•\¦‚·‚é‚½‚ß‚ÌObject")]
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

    /// <summary>Player‚ªˆÚ“®‚µ‚½‚Æ‚«‚ÉMap‚ğXV‚·‚é</summary>
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
