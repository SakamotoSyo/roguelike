using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Tooltip("GameManegerのインスタンス")]
    private GameManager _gameManager;

    [Tooltip("動作中かどうか")]
    private bool _isMoving;

    [Tooltip("プレイヤーが動いた方向")]
    private Vector3 _playerDirection;
    public Vector3 PlayerDirection => _playerDirection;

    [Tooltip("プレイヤーが次に移動する場所")]
    private Vector3 _nextPosition;



    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    /// <summary>
    /// 移動の入力処理
    /// </summary>
    public void MoveInputKey()
    {
        //_countTime = 0;
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        //シフトを押しているときは移動できなくする
        if (!Input.GetButton("Lock"))
        {
            _isMoving = judgeMove((int)x, (int)y);
            //移動先に障害物がないかどうか
            if (_isMoving && (x != 0 || y != 0))
            {
                //プレイヤーの方向を保存する
                _playerDirection = new Vector3(x, y, 0);

                //ゲームマネージャーでプレイヤーがどの部屋にいるか判定するi
                // _gameManagerIns.SetPlayerRoomNum((int)(transform.position.x + x)　, (int)(transform.position.y + y) * -1);

                //アイテムが足元に落ちていないかどうか
                ItemJudge(x, y);

                //ここに岩や敵があった時移動できないという処理を追加する
                _nextPosition = transform.position + new Vector3(x, y, 0);

                //ゲームマネージャーにプレイヤーの場所を渡す
                _gameManager.SetPlayerPosition((int)x, (int)y * -1);

                //移動処理
                transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);

                //行動が終わったのでターンフェーズを変える
                _gameManager.TurnType = GameManager.TurnManager.Enemy;
                Debug.Log("味方の行動が終わりました");
            }
            else
            {

            }
        }
        else
        {
            if (x != 0 || y != 0)
            {
                _playerDirection = new Vector2(x, y);
                Debug.Log($"{_playerDirection}プレイヤーの方向を決めました");
            }
        }



        _isMoving = false;
    }

    /// <summary>
    /// ダンジョンの区画にアクセスして次の場所が移動可能か調べる
    /// </summary>
    /// <param name="x">移動するｘ座標</param>
    /// <param name="y">移動するy座標</param>
    /// <returns></returns>
    private bool judgeMove(int x, int y)
    {
        //マップデータにアクセス
        int value = DgGenerator.Instance.Layer.GetMapData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1);

        // Debug.Log(value);

        //0は壁、1は道
        if (value == 0)
        {
            return false;

        }
        else if (value == 1 || value == 3)
        {
            return true;
        }

        return false;

    }

    /// <summary>
    /// 足元にアイテムがないかどうか判定する
    /// </summary>
    public void ItemJudge(float x, float y)
    {
        foreach (var i in _gameManager.ItemObjList)
        {
            //プレイヤーとアイテムの座標が重なっていた時
            if (i.transform.position == this.gameObject.transform.position + new Vector3(x, y, 0))
            {
                var objCs = i.GetComponent<ItemObjectScript>();
                var PlayerStatus = gameObject.GetComponent<PlayerStatus>();

                //アイテムをインベントリにセットする
                PlayerStatus.SetItem(objCs.ItemInfomation);
                //足元に置いたアイテムをリストからRemove
                _gameManager.RemoveItemObjList(i);
                //落ちているアイテムを削除
                objCs.DestroyObj();

                return;

            }
        }
    }
}
