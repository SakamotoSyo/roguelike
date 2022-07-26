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

    public TurnManager TurnType;

    public GameObject PlayerObj => _playerObj;

    public int TotalEnemyNum => _totalEnemyNum;
    public int PlayerX => _playerX;
    public int PlayerY => _playerY;

    public List<GameObject> ItemObjList => _itemObjList;

    [Tooltip("PlayerのObject")]
    private GameObject _playerObj;

    [Tooltip("アイテムのゲームオブジェクトをリストで管理する")]
    private List<GameObject> _itemObjList = new List<GameObject>();

    [Tooltip("ダンジョンにいる敵の総数")]
    private int _totalEnemyNum;
    //プレイヤーのいる部屋i
    // private int _playerRoomNum;
    //public int PlayerRoomNum => _playerRoomNum; S
    //プレイヤーの座標
    private int _playerX;
    private int _playerY;

    private DgGenerator _dgGenerator;

    private void Start()
    {
        _dgGenerator = DgGenerator.Instance;
    }

    private void Update()
    {
       
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
    /// レベルアップしたときに呼ばれる
    /// </summary>
    private void PlayerLevelUpProcess()
    {
            //OutPutLog($"プレイヤーは{_playerStatus.Level + 1}にアップした");
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
