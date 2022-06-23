using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IEnemyMove
{
    [SerializeField, Header("体力")] private float _enemyHp;
    [SerializeField, Header("行動回数")] private int _actionNum;
    [SerializeField, Header("攻撃力")] private float _power;


    [Tooltip("自分自身の座標")]
    private int _startX;
    private int _startY;

    [Tooltip("追いかける対象の座標")]
    private int _goalX;
    private int _goalY;

    private int _yBool = -1;
    private int _xBool = -1;

    [Tooltip("動いたかどうか")]
    private bool _isMove = false;

    [Tooltip("攻撃できるかどうか")]
    private bool _isAttack = false;

    [Tooltip("次の目的地")]
    private Vector3 _nextPosition;

    [Tooltip("DgGeneratorのインスタンス")]
    private DgGenerator _generatorIns;

    [Tooltip("エネミーマネージャーのインスタンス")]
    private EnemyManager _enemyManager;

    [Tooltip("ゲームマネージャーのインスタンス")]
    private GameManager _gameManager;

    protected private void Start()
    {
        //インスタンスを取得
        _gameManager = GameManager.Instance;
        _enemyManager = EnemyManager.Instance;
        _generatorIns = DgGenerator.Instance;
        //エネミーマネージャーに自分自身のオブジェクトを渡す
        _enemyManager.EnemyList.Add(this.gameObject);
    }

    /// <summary>
    /// Playerの方向にRayを飛ばしPlayerのIDamgageを取得してダメージを与える
    /// </summary>
    protected virtual void Attack()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1), 10.0f);
        Debug.Log(hit.collider.gameObject);
        if (hit.collider)
        {
            Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1), Color.red);
        }

        if (hit.collider.gameObject.TryGetComponent(out IDamageble ID))
        {
            ID.AddDamage(_power);
        }

    }

    /// <summary>
    /// 敵の移動AI
    /// </summary>
    public virtual void Move()
    {
        _startX = (int)_enemyManager.EnemyList[0].transform.position.x;
        _startY = (int)_enemyManager.EnemyList[0].transform.position.y;

        _goalX = (int)_gameManager.PlayerX;
        _goalY = -1 * (int)_gameManager.PlayerY;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var a = _startX + i - 1;
                var b = _startY + j - 1;
                //周りを見渡して攻撃対象がいた場合フラグを上げる
                if (a == _goalX && b == _goalY)
                {
                    _isAttack = true;
                }
            }
        }


        if (!_isMove && !_isAttack)
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

            if (_generatorIns.Layer.GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 1 && !_isMove)
            {
                _nextPosition = transform.position + new Vector3(_xBool, _yBool, 0);
                _isMove = true;
            }
            //Y軸が同じときX軸方向にだけ動く
            if (_generatorIns.Layer.GetMapData(_startX + _xBool, _startY * -1) == 1 && !_isMove && _xBool == 0)
            {
                _nextPosition = transform.position + new Vector3(_xBool, 0, 0);
                _isMove = true;
            }
            //X軸が同じときY方向にだけ動く
            if (_generatorIns.Layer.GetMapData(_startX, _startY + _yBool * -1) == 1 && !_isMove && _yBool == 0)
            {
                _nextPosition = transform.position + new Vector3(0, _yBool, 0);
                _isMove = true;
            }

            //どこにも動けなかった場合周りを見て動く
            if (_generatorIns.Layer.GetMapData(_startX + 1, _startY * -1) == 1 && !_isMove && _xBool == 1)
            {
                _nextPosition = transform.position + new Vector3(1, 0, 0);
                _isMove = true;
            }

            if (_generatorIns.Layer.GetMapData(_startX - 1, _startY * -1) == 1 && !_isMove && _xBool == -1)
            {
                _nextPosition = transform.position + new Vector3(-1, 0, 0);
                _isMove = true;
            }

            if (_generatorIns.Layer.GetMapData(_startX, (_startY * -1) - 1) == 1 && !_isMove && _yBool == 1)
            {
                _nextPosition = transform.position + new Vector3(0, 1, 0);
                _isMove = true;
            }

            if (_generatorIns.Layer.GetMapData(_startX, (_startY * -1) + 1) == 1 && !_isMove && _yBool == -1)
            {
                _nextPosition = transform.position + new Vector3(0, -1, 0);
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
                EnemyManager.Instance.EnemyList[0].transform.position += new Vector3(_xBool, _yBool, 0);


            }

        }
        else if (_isAttack)
        {
            _isAttack = false;

            Attack();
        }

        //移動する
        transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);

        if (transform.position == _nextPosition)
        {
            _isMove = false;
            GameManager.Instance.TurnType = GameManager.TurnManager.Player;
        }
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damage">受けるダメージ</param>
    public void AddDamage(float damage) 
    {
        
    }
}
