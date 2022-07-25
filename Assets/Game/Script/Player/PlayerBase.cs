using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBase : MonoBehaviour
{

    [Header("PlayerStatus")]
    [SerializeField] private PlayerStatus _playerStatus;

    [Tooltip("ゲームマネージャー")]
    private GameManager _gameManagerIns;

    [Tooltip("エネミーマネージャー")]
    private EnemyManager _enemyManagerIns;

    [Tooltip("プレイヤーが次に移動する場所")]
    private Vector3 _nextPosition;

    [Tooltip("プレイヤーが動いた方向")]
    private Vector3 _playerDirection;
    public Vector3 PlayerDirection => _playerDirection;

    [Tooltip("動作中かどうか")]
    private bool _isMoving;


    //Test用
    private float _waitTime = 0.1f;
    private float _countTime = 0;

    private void Start()
    {
        _gameManagerIns = GameManager.Instance;
        _enemyManagerIns = EnemyManager.Instance;
        _gameManagerIns.SetPlayerObj(this.gameObject);

        //ゲームマネージャーにプレイヤーの場所を渡すi
        // _gameManagerIns.SetPlayerRoomNum((int)(transform.position.x), (int)transform.position.y * -1);

    }
    private void Update()
    {
        if (_gameManagerIns.TurnType == GameManager.TurnManager.Player)
        {
            Attack();
        }
        if (_gameManagerIns.TurnType == GameManager.TurnManager.Player && _waitTime < _countTime)
        {
            MoveInputKey();
        }
        if (_gameManagerIns.TurnType == GameManager.TurnManager.Result) 
        {
            
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
                //プレイヤーの方向を保存する
                _playerDirection = new Vector3(x, y, 0);

                //ゲームマネージャーでプレイヤーがどの部屋にいるか判定するi
                // _gameManagerIns.SetPlayerRoomNum((int)(transform.position.x + x)　, (int)(transform.position.y + y) * -1);

                //アイテムが足元に落ちていないかどうか
                ItemJudge(x, y);

                //ここに岩や敵があった時移動できないという処理を追加する
                _nextPosition = transform.position + new Vector3(x, y, 0);

                //ゲームマネージャーにプレイヤーの場所を渡す
                _gameManagerIns.SetPlayerPosition((int)x, (int)y * -1);

                //移動処理
                transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);

                //行動が終わったのでターンフェーズを変える
                _gameManagerIns.TurnType = GameManager.TurnManager.Enemy;
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
    ///プレイヤーの攻撃処理
    /// </summary>
    private void Attack()
    {
        if (Input.GetButtonDown("Submit"))
        {
            Debug.Log("攻撃がよばれた");
            //プレイヤーが向いている方向に敵がいた場合
            foreach (var i in _enemyManagerIns.EnemyBaseList)
            {
                if (transform.position.x + PlayerDirection.x == i.transform.position.x && transform.position.y + PlayerDirection.y == i.transform.position.y)
                {
                     LogScript.Instance.OutPutLog("攻撃の処理が成功した");
                    //アニメーションの処理を入れる
                    i.GetComponent<IDamageble>().AddDamage(_playerStatus.Power, this.gameObject);
                    
                }
            }

            //経験値をゲットしたかどうか確認する
            _gameManagerIns.TurnType = GameManager.TurnManager.Result;

           //元の設定
           //_gameManagerIns.TurnType = GameManager.TurnManager.Enemy;
        }
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
        foreach (var i in _gameManagerIns.ItemObjList)
        {
            //プレイヤーとアイテムの座標が重なっていた時
            if (i.transform.position == this.gameObject.transform.position + new Vector3(x, y, 0))
            {
                var objCs = i.GetComponent<ItemObjectScript>();
                var PlayerStatus = gameObject.GetComponent<PlayerStatus>();

                //アイテムをインベントリにセットする
                PlayerStatus.SetItem(objCs.ItemInfomation);
                //足元に置いたアイテムをリストからRemove
                _gameManagerIns.RemoveItemObjList(i);
                //落ちているアイテムを削除
                objCs.DestroyObj();

                return;

            }
        }
    }

    /// <summary>
    /// 経験獲得などリザルトの処理
    /// </summary>
    private void ResultProcess() 
    {
        
    }

}