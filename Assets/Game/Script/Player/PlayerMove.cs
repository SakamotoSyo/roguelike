using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour
{
    public enum MoveType
    {
        WaitKey,
        Move,
    }

    [SerializeField] private float _startTime;

    //ゲームマネージャー
    private GameManager _gameManagerIns;

    //プレイヤーが次に移動する場所
    private Vector3 _nextPosition;

    //動作中かどうか
    private bool _isMoving;
    //プレイヤーのムーブタイプ
    private MoveType _moveType = MoveType.WaitKey;

    //Test用
    private float _waitTime = 0.1f;
    private float _countTime = 0;

    private void Start()
    {
        _gameManagerIns = GameManager.Instance;
        _gameManagerIns.PlayerX = (int)transform.position.x;
        _gameManagerIns.PlayerY = -1 * (int)transform.position.y;

    }
    private void Update()
    {
        if (GameManager.Instance.TurnType == GameManager.TurnManager.Player && _waitTime < _countTime) 
        {
         　InputKey();
        }
        _countTime += Time.deltaTime;   
    }

    /// <summary>
    /// 移動の入力処理
    /// </summary>
    private void InputKey()
    {
        _countTime = 0;
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        //シフトを押しているときは移動できなくする
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            _isMoving = judgeMove((int)x, (int)y);
            //移動先に障害物がないかどうか
            if (_isMoving && (x != 0 || y != 0))
            {
                //ここに岩や敵があった時移動できないという処理を追加する
                _nextPosition = transform.position + new Vector3(x, y, 0);
 
                //ゲームマネージャーにプレイヤーの場所を渡す
                GameManager.Instance.PlayerX += (int)x;
                GameManager.Instance.PlayerY += (int)y * -1;

                transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);
                
                GameManager.Instance.TurnType = GameManager.TurnManager.Enemy;
            }
            else 
            {
                
            }
        }
        else
        {
            Debug.Log("Shift");
            ////ここに岩や敵があった時移動できないという処理を追加する
            //_nextPosition = transform.position + new Vector3(x, y, 0);
            //_moveType = MoveType.Move;
        }

        _isMoving = false;
    }

    /// <summary>
    /// 実際の移動処理
    /// </summary>
    private void CharacterMove()
    {

        float i = Mathf.PingPong(Time.time - _startTime, 1);
        

        //テストプレイ用
        if (_nextPosition == transform.position)
        {
            _countTime += Time.deltaTime;
            if (_countTime > _waitTime)
            {
              
               
                _countTime = 0;
            }

        }
    }

    /// <summary>
    /// ダンジョンの区画にアクセスして次の場所が移動可能か調べる
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool judgeMove(int x, int y)
    {
        //マップデータにアクセス
        int value = DgGenerator.Instance.Layer.GetMapData(_gameManagerIns.PlayerX + x, _gameManagerIns.PlayerY + y * -1);
       
       // Debug.Log(value);
        //0は壁、1は道
        if (value == 0)
        {
            Debug.Log("かべ");
            return false;
            
        }
        else if (value == 1)
        {
            Debug.Log("呼ばれた");
            return true;
           
        }

        return false;

    }
}
