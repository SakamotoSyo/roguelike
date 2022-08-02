using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    EnemyManager _enemyManager;
    GameManager _gameManager;

    [Tooltip("playerStatusのスクリプト")]
    [SerializeField] PlayerStatus _playerStatus;

    [Tooltip("playerMoveのスクリプト")]
    [SerializeField] PlayerMove _playerMoveCs;


    private void Start()
    {
        _gameManager = GameManager.Instance;
        _enemyManager = EnemyManager.Instance;
    }

    /// <summary>
    ///プレイヤーの攻撃処理
    /// </summary>
    public void Attack()
    {
        if (Input.GetButtonDown("Submit"))
        {
            Debug.Log("攻撃がよばれた");
            //プレイヤーが向いている方向に敵がいた場合
            foreach (var i in _enemyManager.EnemyBaseList)
            {
                if (transform.position.x + _playerMoveCs.PlayerDirection.x == i.EnemyPos.x && transform.position.y + _playerMoveCs.PlayerDirection.y == i.EnemyPos.y)
                {
                    LogScript.Instance.OutPutLog("攻撃の処理が成功した");
                    //アニメーションの処理を入れる
                    i.GetComponent<IDamageble>().AddDamage(_playerStatus.Power, this.gameObject);
                    //ダメージを与え終わったら処理を終える
                    break;

                }
            }

            //経験値をゲットしたかどうか確認する
            _gameManager.TurnType = GameManager.TurnManager.Result;

            //元の設定
            //_gameManager.TurnType = GameManager.TurnManager.Enemy;
        }
    }

}
