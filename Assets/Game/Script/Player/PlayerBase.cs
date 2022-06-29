using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBase : MonoBehaviour
{
   

    [Tooltip("ゲームマネージャー")]
    private GameManager _gameManagerIns;

    [Tooltip("プレイヤーが次に移動する場所")]
    private Vector3 _nextPosition;

    [Tooltip("動作中かどうか")]
    private bool _isMoving;

  
    //Test用
    private float _waitTime = 0.1f;
    private float _countTime = 0;

    private void Start()
    {
        _gameManagerIns = GameManager.Instance;
        _gameManagerIns.SetPlayerObj(this.gameObject);

    }
    private void Update()
    {
        if (_gameManagerIns.TurnType == GameManager.TurnManager.Player && _waitTime < _countTime)
        {
            MoveInputKey();
        }
        _countTime += Time.deltaTime;
    }

    /// <summary>
    /// 移動の入力処理
    /// </summary>
    private void MoveInputKey()
    {
        _countTime = 0;
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        //シフトを押しているときは移動できなくする
        if (!Input.GetButton("Lock"))
        {
            _isMoving = judgeMove((int)x, (int)y);
            //移動先に障害物がないかどうか
            if (_isMoving && (x != 0 || y != 0))
            {
                //ここに岩や敵があった時移動できないという処理を追加する
                _nextPosition = transform.position + new Vector3(x, y, 0);

                //ゲームマネージャーにプレイヤーの場所を渡す
                _gameManagerIns.SetPlayerPosition((int)x, (int)y * -1);

                //移動処理
                transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);

                //行動が終わったのでターンフェーズを変える
                _gameManagerIns.TurnType = GameManager.TurnManager.Enemy;
            }
            else
            {

            }
        }
        else
        {
            Debug.Log("Shift");
        }

        _isMoving = false;
    }

    private void UIInputKey() 
    {
        if (Input.GetButtonDown("Cancel")) 
        {
            
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
            return false;

        }
        else if (value == 1)
        { 
           return true;
        }

        return false;

    }

   
}