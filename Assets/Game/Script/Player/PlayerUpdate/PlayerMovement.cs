using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("GameManagerのインスタンス")]
    GameManager _gameManager;

    [Header("PlayerMoveのスクリプト")]
    [SerializeField] PlayerMove _playerMoveCs;
    [Header("PlayerAttackのスクリプト")]
    [SerializeField] PlayerAttack _playerAttackCs;

    UIManager _uiManager;

    public bool FindStairs = false;

    void Awake()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _gameManager = GameManager.Instance;
        _gameManager.SetPlayerPosition((int)transform.position.x, (int)transform.position.y * -1);
        _gameManager.SetPlayerObj(this.gameObject);
    }

    /// <summary>
    /// プレイヤがどう行動するか決定する
    /// </summary>
    void Update()
    {
        if (_gameManager.TurnType == GameManager.TurnManager.Player)
        {
            //自分の足元に階段があるどうか
            StairCheck(false);
            //プレイヤーの攻撃処理
            _playerAttackCs.Attack();
        }
        else if (_gameManager.TurnType == GameManager.TurnManager.LogOpen)
        {

        }
        else if (_gameManager.TurnType == GameManager.TurnManager.Result)
        {

        }
        else if (_gameManager.TurnType == GameManager.TurnManager.Enemy)
        {

        }

    }

    void FixedUpdate()
    {
        if (_gameManager.TurnType == GameManager.TurnManager.Player)
        {
            //移動の入力処理
            _playerMoveCs.MoveInputKey();
            //プレイヤーの攻撃処理
            //_playerAttackCs.Attack();
        }
    }

    public void StairCheck(bool find)
    {
        if (FindStairs) 
        {
            _gameManager.TurnType = GameManager.TurnManager.MenuOpen;
           // Debug.Log("階段があります");
            _uiManager.StairUI();
        }
        FindStairs = find;
    }
}
