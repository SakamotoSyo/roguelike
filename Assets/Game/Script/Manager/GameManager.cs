using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class GameManager : SingletonBehaviour<GameManager>
{
    public enum TurnManager
    {
        Player,
        MenuOpen,
        Enemy,
        LogOpen,
        Result,
    }

    [Header("ログを出力するパネル")]
    [SerializeField] private GameObject _logPanel;

    [Header("ログを出力するテキスト")]
    [SerializeField] private Text _logText;

    [Header("メッセージをどの程度表示するか")]
    [SerializeField] private float _messageTime;

    [Header("メッセージのスクロールにかける時間")]
    [SerializeField] private float _messageScroll;

    [Header("メッセージのスクロールバー")]
    [SerializeField] private Scrollbar _scrollbar;

    public TurnManager TurnType;

    public GameObject PlayerObj => _playerObj;

    public int TotalEnemyNum => _totalEnemyNum;
    public int PlayerX => _playerX;
    public int PlayerY => _playerY;

    public List<GameObject> ItemObjList => _itemObjList;


    private GameObject _playerObj;

    [Tooltip("アイテムのゲームオブジェクトをリストで管理する")]
    private List<GameObject> _itemObjList = new List<GameObject>();
    [Tooltip("TextLogを格納するList")]
    private List<String> _logList = new List<string>();

    [Tooltip("ダンジョンにいる敵の総数")]
    private int _totalEnemyNum;
    //プレイヤーのいる部屋i
    // private int _playerRoomNum;
    //public int PlayerRoomNum => _playerRoomNum; S
    //プレイヤーの座標
    private int _playerX;
    private int _playerY;

    private float _countTime;
    private float _messageCountTime;

    private Coroutine _textCoroutine;
    private Coroutine _messageCoroutine;

    private DgGenerator _dgGenerator;

    private void Start()
    {
        _dgGenerator = DgGenerator.Instance;
    }

    private void Update()
    {
        LogActive();

        if (_scrollbar.value != 0)
        {
            LogScroll();
        }
    }

    /// <summary>
    /// プレイヤーのポジションをセットする
    /// </summary>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    public void SetPlayerPosition(int x, int y)
    {
        _playerX += x;
        _playerY += y;
    }

    /// <summary>
    /// プレイヤーの座標をセットする
    /// </summary>
    /// <param name="player">playerのオブジェクト</param>
    public void SetPlayerObj(GameObject player)
    {
        _playerObj = player;
    }

    /// <summary>
    /// アイテムオブジェクトをリストにセットする
    /// </summary>
    /// <param name="obj"></param>
    public void SetItemObjList(GameObject obj)
    {
        _itemObjList.Add(obj);
    }

    /// <summary>
    /// 敵の総数を増やしたり減らしたりする
    /// </summary>
    /// <param name="num"></param>
    public void SetTotalEnemy(int num)
    {
        _totalEnemyNum += num;
    }

    /// <summary>
    /// リストから指定したインデックスのアイテムをリムーブする
    /// </summary>
    /// <param name="a"></param>
    public void RemoveItemObjList(GameObject ItemObj)
    {
        _itemObjList.Remove(ItemObj);
    }

    /// <summary>
    /// Logにメッセージを出力する
    /// </summary>
    /// <param name="message">メッセージ</param>
    public void OutPutLog(string message)
    {

        //既に前のテキストが残っていた場合改行してTextを出力する
        if (_logList.Count == 0)
        {
            _logList.Add(message);
            _logText.text = message;
        }
        else
        {
            var St = "\n" + message;
            _logList.Add(St);
            _logText.text += St;
        }

        //既にパネルが表示されていたらパネルの表示時間を元に戻す
        if (_textCoroutine == null)
        {
            _textCoroutine = StartCoroutine(LogActive());

        }
        else
        {
            StopCoroutine(_textCoroutine);
            _textCoroutine = null;
            _textCoroutine = StartCoroutine(LogActive());
        }
    }

    /// <summary>
    /// パネルをアクティブ非アクティブ切り替える
    /// </summary>
    private IEnumerator LogActive()
    {
        _logPanel.SetActive(true);
        yield return new WaitForSeconds(3);
        _logPanel.SetActive(false);

        StopCoroutine(_textCoroutine);
        _textCoroutine = null;
    }

    /// <summary>文字が見切れた時にTextをScrollする処理</summary>
    private void LogScroll()
    {
        _messageCountTime += Time.deltaTime;
        if (_messageTime > _messageCountTime) 
        {
            return;
        }

        _scrollbar.value -= 0.01f;

        if (_scrollbar.value <= 0) 
        {
            _scrollbar.value = 0;   
        }

        _messageCountTime = 0;
    }

    /// <summary>
    /// プレイヤーのいる部屋を判定して変数に入れるi
    /// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    //public void SetPlayerRoomNum(int x, int y) 
    //{
    //    for (int i = 0; i < _dgGenerator.GetDivList().Count; i++)
    //    {
    //        DgDivision a = _dgGenerator.GetDivList()[i];

    //        if (a.Room.Left <= x && a.Room.Right >= x && a.Room.Top >= y && a.Room.Bottom <= y)
    //        {
    //            _playerRoomNum = i;
    //        }

    //    }
    //}

    ///// <summary>自分が現在どの部屋にいるか始めるi</summary>
    //public int GetRoomNum(int x, int y) 
    //{

    //    for (int i = 0; i < _dgGenerator.GetDivList().Count; i++) 
    //    {
    //       DgDivision a = DgGenerator.Instance.GetDivList()[i];

    //        if (a.Room.Left <= x && a.Room.Right >= x && a.Room.Top >= y && a.Room.Bottom <= y) 
    //        {
    //            return i;  
    //        }

    //    }

    //    return -1;
    //}

}
