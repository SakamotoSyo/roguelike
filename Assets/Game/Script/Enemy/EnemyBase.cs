using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
  

    [Tooltip("自分自身の座標")]
    protected int _startX;
    protected int _startY;

    [Tooltip("追いかける対象の座標")]
    protected int _goalX;
    protected int _goalY;

    protected int _yBool = -1;
    protected int _xBool = -1;

    [Tooltip("移動したかどうか")]
    protected bool _isMove = false;

    [Tooltip("攻撃できるかどうか")]
    protected bool _isAttack = false;

    [Tooltip("次の目的地")]
    protected Vector3 _nextPosition;

    [Tooltip("DgGeneratorのインスタンス")]
    protected DgGenerator _generatorIns;

    [Tooltip("エネミーマネージャーのインスタンス")]
    protected EnemyManager _enemyManager;

    [Tooltip("ゲームマネージャーのインスタンス")]
    protected GameManager _gameManager;

    [Tooltip("EnemyStatusのScript")]
    [SerializeField]protected EnemyStatus _enemyStatus;

    [Tooltip("PlayerIDamageble")]
    protected IDamageble _playerBase;

    [Tooltip("自分がどこの部屋にいるか")]
    protected int _nowRoomNum;

    private Vector2 _position;

    public Vector3 EnemyPos => this.gameObject.transform.position; 


    protected void Start()
    {
        //インスタンスを取得
        _gameManager = GameManager.Instance;
        _enemyManager = EnemyManager.Instance;
        _generatorIns = DgGenerator.Instance;
        //エネミーマネージャーに自分自身のオブジェクトを渡す
        _enemyManager.SetEnemyBaseList(this.gameObject.GetComponent<EnemyBase>());

        _playerBase = _gameManager.PlayerObj.GetComponent<IDamageble>();
    }

    protected virtual void Update()
    {
      
    }


    /// <summary>
    /// 敵の移動AI
    /// </summary>
    public virtual void EnemyAction()
    {
        _startX = (int)transform.position.x;
        _startY = (int)transform.position.y;

        _goalX = (int)_gameManager.PlayerX;
        _goalY = -1 * (int)_gameManager.PlayerY;

        //攻撃できるかどうか確認する
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var a = _startX + i - 1;
                var b = _startY + j - 1;
                //周りを見渡して攻撃対象がいた場合フラグを上げる
                if (a == _goalX && b == _goalY)
                {
                    //攻撃処理
                    _playerBase.AddDamage(_enemyStatus.GetPower(), this.gameObject);
                    _isAttack = true;
                    //コルーチンでアニメーションの処理を書いてもいいかも
                }
            }
        }


        if (!_isMove && !_isAttack)
        {
            EnemyMove();
        }
        else if (_isAttack)
        {
            _isAttack = false;
        }

        //自分が今どこの部屋にいるか判定するi
       // _nowRoomNum = _gameManager.GetRoomNum((int)transform.position.x, (int)transform.position.y);
        //移動する
        transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);

        if (transform.position == _nextPosition)
        {
            Debug.Log("EnemyActionEnd");
            _generatorIns.Layer.SetData((int)Mathf.Floor(transform.position.x), (int)Mathf.Floor(transform.position.y) * -1, 2);
            _isMove = false;
            _enemyManager.EnemyActionEnd = false;
        }
    }

    /// <summary>
    /// どう移動するかの処理
    /// </summary>
    public virtual void EnemyMove() 
    {
        _xBool = -1;
        _yBool = -1;

        //目的地のX軸が右方向だった場合
        if (_goalX - _startX > 0)
        {
            _xBool = 1;
        }
        //目的地のX軸が同じだった場合
        else if (_goalX - _startX == 0)
        {
            _xBool = 0;
        }

        //目的地のY軸がした方向だった場合
        if (_goalY - _startY > 0)
        {
            _yBool = 1;
        }
        //目的地のY軸が同じだった場合
        else if (_goalY - _startY == 0)
        {
            _yBool = 0;
        }

        if ((GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 1 || GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 3) && !_isMove)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(_xBool, _yBool);
            _isMove = true;
        }
        //Y軸が同じときX軸方向にだけ動く
        if ((GetMapData(_startX + _xBool, _startY * -1) == 1 || GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 3) && !_isMove && _yBool == 0)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(_xBool, 0);
            _isMove = true;
        }
        //X軸が同じときY方向にだけ動く
        if ((GetMapData(_startX, _startY + _yBool * -1) == 1 || GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 3) && !_isMove && _xBool == 0)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(0, _yBool);
            _isMove = true;
        }

        //どこにも動けなかった場合周りを見て動く
        if ((GetMapData(_startX + 1, _startY * -1) == 1 || GetMapData(_startX + 1, (_startY + _yBool) * -1) == 3) && !_isMove && _xBool == 1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(1, 0);
            _isMove = true;
        }

        if ((GetMapData(_startX - 1, _startY * -1) == 1 || GetMapData(_startX - 1, (_startY + _yBool) * -1) == 3) && !_isMove && _xBool == -1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(-1, 0);
            _isMove = true;
        }

        if ((GetMapData(_startX, (_startY * -1) - 1) == 1 || GetMapData(_startX + _xBool, (_startY * -1) - 1) == 3) && !_isMove && _yBool == 1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(0, 1);
            _isMove = true;
        }

        if ((GetMapData(_startX, (_startY * -1) + 1) == 1 || GetMapData(_startX + _xBool, (_startY * -1) + 1) == 3) && !_isMove && _yBool == -1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(0, -1);
            _isMove = true;
        }
        else
        {
            Debug.Log("壁です");
        }


        if (!_isMove)
        {
            _xBool = 0;
            _yBool = 0;
        }
        else
        {
            //ゲームマネージャーにプレイヤーの場所を渡す
            //EnemyManager.Instance.EnemyList[0].transform.position += new Vector3(_xBool, _yBool, 0);
            _generatorIns.Layer.SetData((int)transform.position.x, (int)transform.position.y * -1, 1);
        }
    }

    /// <summary>
    /// マップデータの情報を取り出す
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int GetMapData(int x, int y) 
    {
        return _generatorIns.Layer.GetMapData(x, y);
    }

  

    /// <summary> Enemyにどこの部屋に今いるのか値をセットするi </summary>
    //public void SetRoomNum(int nowRoom) 
    //{
    //    _nowRoomNum = nowRoom;
    //}
}

//public struct EnemyStatusData 
//{
//    public float _enemyHp;
//    public float _power;
//    public float _exp;

//    public EnemyStatusData(float hp, float power, float exp) 
//    {
//        this._enemyHp = hp;
//        this._power = power;
//        this._exp = exp;
//    }
//}

    ///// <summary>
    ///// Playerの方向にRayを飛ばしPlayerのIDamgageを取得してダメージを与える
    ///// </summary>
    //protected virtual IEnumerator Attack()
    //{
    //    //エネミーからプレイヤー対する方向ベクトルを取得
    //    //_position = new Vector2(this.transform.position.x, this.transform.position.y) - new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1);
    //    _position = new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1) - new Vector2(this.transform.position.x, this.transform.position.y);


    //    //RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1), 10.0f, _testLayerMask);
    //    RaycastHit2D hit = Physics2D.Linecast(this.transform.position, _position, _testLayerMask);

    //    yield return new WaitForSeconds(0.1f);
 
    //    Debug.Log(hit);
    //    if (hit.collider.gameObject.TryGetComponent(out IDamageble ID))
    //    {
    //        ID.AddDamage(_power);
    //    }
     

    //    _enemyManager.EnemyActionEnd = false;
    //    _isAttack = false;

    //}