using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBase : MonoBehaviour, IDamageble
{
    public ChType GetType => ChType.Player;

    [SerializeField, Header("最大HP")] private int _maxHp;
    [SerializeField, Header("HP")] private float _playerHp;
    [SerializeField, Header("攻撃力")] private float _power;
    [SerializeField, Header("行動の回数")] private int _actionNum;
    [SerializeField, Header("アイテムの所持リスト")] private List<Item> _playerItemList;

    [SerializeField, Header("ItemDateBase")] private ItemDataBase _itemDataBase;

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
        _gameManagerIns.PlayerBase = this.gameObject;
        _gameManagerIns.PlayerX = (int)transform.position.x;
        _gameManagerIns.PlayerY = -1 * (int)transform.position.y;

    }
    private void Update()
    {
        if (_gameManagerIns.TurnType == GameManager.TurnManager.Player && _waitTime < _countTime)
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
        }

        _isMoving = false;
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

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damage">食らうダメージ</param>
    public void AddDamage(float damage)
    {
        Debug.Log($"プレイヤーは{damage}のダメージを受けた");
        _playerHp -= damage;
    }

    /// <summary>
    /// アイテム名からアイテムを取得する
    /// </summary>
    /// <param name="searchName">アイテム名</param>
    /// <returns></returns>
    public Item GetItem(string searchName) 
    {
        return _itemDataBase.GetItemLists().Find(itemName => itemName.GetItemName == searchName);
    }
}