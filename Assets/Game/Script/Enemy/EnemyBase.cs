using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using UniRx.Triggers;
using UniRx;
public abstract class EnemyBase : MonoBehaviour
{
    [Header("ダイクストラスクリプト")]
    [SerializeField] DaiksutoraCs _daiksutoraCs;

    [Header("移動にかける時間")]
    [SerializeField] float _moveTime;

    [SerializeField] Animator _anim;

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

    [Tooltip("進んだ方向を保存しておく変数")]
    protected Vector2 _dir;

    [Tooltip("DgGeneratorのインスタンス")]
    protected DgGenerator _generatorIns;

    [Tooltip("エネミーマネージャーのインスタンス")]
    protected EnemyManager _enemyManager;

    [Tooltip("ゲームマネージャーのインスタンス")]
    protected GameManager _gameManager;

    [Tooltip("EnemyStatusのScript")]
    [SerializeField] protected EnemyStatus _enemyStatus;

    [Tooltip("PlayerIDamageble")]
    protected IDamageble _playerBase;

    [Tooltip("自分がどこの部屋にいるか")]
    protected int _nowRoomNum;

    [Tooltip("監視するアニメーションを入れる")]
    protected AnimatorStateInfo stateInfo;

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
        stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
    }


    /// <summary>
    /// 敵の移動AI
    /// </summary>
    public virtual async void EnemyAction()
    {
        _startX = (int)transform.position.x;
        _startY = (int)transform.position.y;

        _goalX = _gameManager.PlayerX;
        _goalY = -1 * _gameManager.PlayerY;


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
                    //_playerBase.AddDamage(_enemyStatus.GetPower(), this.gameObject);
                    // _anim.SetTrigger("Attack");
                    _isAttack = true;
                    await EnemyAttack();
                    //_enemyManager.EnemyActionEnd = false;

                    //コルーチンでアニメーションの処理を書いてもいいかも
                }
            }
        }


        if (!_isMove && !_isAttack)
        {
            //var data = _daiksutoraCs.Dijkstra(_startX, _startY * -1);
            //Debug.Log($"{_startX - data.PlayerX}da{_startY - data.PlayerY * -1}");
            //_nextPosition = new Vector2(_startX - data.PlayerX, _startY  - data.PlayerY * -1);
            EnemyMove();
        }
        else if (_isAttack)
        {
            _isAttack = false;
        }

        //自分が今どこの部屋にいるか判定するi
        // _nowRoomNum = _gameManager.GetRoomNum((int)transform.position.x, (int)transform.position.y);

        if (_isMove)
        {
            _generatorIns.Layer.SetData((int)_nextPosition.x, (int)_nextPosition.y * -1, MapNum.EnemyNum);
            //移動する
            DOTween.To(() => transform.position,
                       x => transform.position = x,
                       _nextPosition, _moveTime)
                       .OnComplete(() => transform.position = _nextPosition);

            _anim.SetBool("Move", true);
            _anim.SetFloat("x", _dir.x);
            _anim.SetFloat("y", _dir.y);
            //transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);
            StartCoroutine(NextJudge());

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
        //??I?n??X????????????????
        else if (_goalX - _startX == 0)
        {
            _xBool = 0;
        }

        //??I?n??Y????????????????????
        if (_goalY - _startY > 0)
        {
            _yBool = 1;
        }
        //??I?n??Y????????????????
        else if (_goalY - _startY == 0)
        {
            _yBool = 0;
        }
        // _xBool = (int)Mathf.Sign(_goalX - _startX);

        //_yBool = (int)Mathf.Sign(_goalY - _startY);

        //通常の移動
        if ((GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 1 || GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 3) && !_isMove)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(_xBool, _yBool);
            _dir = new Vector2(_xBool, _yBool);
            _isMove = true;
        }
        //Y軸が同じときX軸方向にだけ動く
        else if ((GetMapData(_startX + _xBool, _startY * -1) == 1 || GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 3) && !_isMove && _yBool == 0)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(_xBool, 0);
            _dir = new Vector2(_xBool, 0);
            _isMove = true;
        }
        //X軸が同じときY方向にだけ動く
        else if ((GetMapData(_startX, _startY + _yBool * -1) == 1 || GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 3) && !_isMove && _xBool == 0)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(0, _yBool);
            _dir = new Vector2(0, _yBool);
            _isMove = true;
        }

        //どこにも動けなかった場合周りを見て動く
        if ((GetMapData(_startX + 1, _startY * -1) == 1 || GetMapData(_startX + 1, (_startY + _yBool) * -1) == 3) && !_isMove && _xBool == 1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(1, 0);
            _dir = new Vector2(1, 0);
            _isMove = true;
        }
        else if ((GetMapData(_startX - 1, _startY * -1) == 1 || GetMapData(_startX - 1, (_startY + _yBool) * -1) == 3) && !_isMove && _xBool == -1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(-1, 0);
            _dir = new Vector2(-1, 0);
            _isMove = true;
        }
        else if ((GetMapData(_startX, (_startY * -1) - 1) == 1 || GetMapData(_startX + _xBool, (_startY * -1) - 1) == 3) && !_isMove && _yBool == 1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(0, 1);
            _dir = new Vector2(0, 1);
            _isMove = true;
        }
        else if ((GetMapData(_startX, (_startY * -1) + 1) == 1 || GetMapData(_startX + _xBool, (_startY * -1) + 1) == 3) && !_isMove && _yBool == -1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(0, -1);
            _dir = new Vector2(0, -1);
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
            _enemyManager.EnemyActionEnd = false;
        }
        else
        {
            //ゲームマネージャーにプレイヤーの場所を渡す
            //EnemyManager.Instance.EnemyList[0].transform.position += new Vector3(_xBool, _yBool, 0);
            _generatorIns.Layer.SetData((int)transform.position.x, (int)transform.position.y * -1, MapNum.LoadNum);
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

    /// <summary>
    ///このスクリプトがついているGameObjectを返す
    /// </summary>
    /// <returns></returns>
    public GameObject GetThisScriptObj()
    {
        return this.gameObject;
    }


    /// <summary>
    /// 指定した目的地に着くまでループを回し続ける関数
    /// </summary>
    /// <returns></returns>
    IEnumerator NextJudge()
    {
        while (true)
        {
            if (transform.position == _nextPosition)
            {
                _anim.SetBool("Move", false);
                _isMove = false;
                _enemyManager.EnemyActionEnd = false;
                break;
            }
            yield return null;
        }
        yield return null;
    }


    async UniTask EnemyAttack()
    {
        _anim.SetTrigger("Attack");
        //攻撃実行前のステートを取得しないように１フレーム待つ
        await UniTask.DelayFrame(1);

        stateInfo = default;

        await UniTask.WaitUntil(() => 0.5f <= stateInfo.normalizedTime);
        //攻撃
        _playerBase.AddDamage(_enemyStatus.GetPower(), this.gameObject);
        Debug.Log("ダメージを与えた");

        await UniTask.WaitUntil(() => 1f <= stateInfo.normalizedTime);

        _enemyManager.EnemyActionEnd = false;

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