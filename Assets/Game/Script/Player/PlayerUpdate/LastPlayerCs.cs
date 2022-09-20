using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlayerCs : MonoBehaviour
{
    [Header("アニメーション")]
    [SerializeField] Animator _animator;
    [Header("GSSReaderのスクリプト")]
    [SerializeField] GSSReader _gssReader;

    [Tooltip("GameManagerのインスタンス")]
    GameManager _gameManager;
    [Tooltip("DgGeneratorのインスタンス")]
    DgGenerator _dgGenerator;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _dgGenerator = DgGenerator.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.TurnType == GameManager.TurnManager.Story) 
        {
            _animator.SetBool("Move", true);
        }
    }

    /// <summary>
    /// ボス戦をスタートする
    /// </summary>
    public void BossStart() 
    {
        _gameManager.SetPlayerPosition((int)transform.position.x, (int)transform.position.y * -1);
        _gameManager.PlayerObj.transform.position = transform.position;
        _gameManager.PlayerObj.SetActive(true);
        gameObject.SetActive(false);
       
        _gameManager.TurnType = GameManager.TurnManager.Player;
    }
}
