using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : SingletonBehaviour<GameManager>
{
    public enum TurnManager
    {
        Player,
        MenuOpen,
        Enemy,
    }

    public TurnManager TurnType;

    private GameObject _playerObj;
    public GameObject PlayerObj => _playerObj;

    private List<GameObject> _itemObjList = new List<GameObject>();

    public List<GameObject> ItemObjList => _itemObjList;


    //プレイヤーの座標
    private int _playerX;
    private int _playerY;
    //カプセル化
    public int PlayerX => _playerX;
    public int PlayerY => _playerY;



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
    /// アイテムをリストにセットする
    /// </summary>
    /// <param name="obj"></param>
    public void SetItemObjList(GameObject obj)
    {
        _itemObjList.Add(obj);
    }

    /// <summary>
    /// リストから指定したインデックスのアイテムをリムーブする
    /// </summary>
    /// <param name="a"></param>
    public void RemoveItemObjList(GameObject a)
    {
        _itemObjList.Remove(a);
    }

}
