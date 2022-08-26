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

    void Awake()
    {
        _gameManager = GameManager.Instance;
        _gameManager.SetPlayerObj(this.gameObject);
    }

    /// <summary>
    /// プレイヤがどう行動するか決定する
    /// </summary>
    void Update()
    {
        if (_gameManager.TurnType == GameManager.TurnManager.Player)
        {
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
            _playerAttackCs.Attack();
        }
    }
}
