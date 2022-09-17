using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
public class GameManager : SingletonBehaviour<GameManager>
{
    public enum TurnManager
    {
        Player,
        MenuOpen,
        Enemy,
        LogOpen,
        Result,
        WaitTurn,
        Story,
    }

    public TurnManager TurnType;

    public GameObject PlayerObj => _playerObj;

    public int TotalEnemyNum => _totalEnemyNum;
    public int PlayerX => _playerX;
    public int PlayerY => _playerY;
    public int NowFloor => _nowFloor;
    public int FinalFloor => _finalStratum;
    public List<GameObject> ItemObjList => _itemObjList;

    [Header("現在の階層を表示するテキスト")]
    [SerializeField] Text _nowFloorText;

    [Header("最終階層")]
    [SerializeField] int _finalStratum;

    [Header("フェードにかかる時間")]
    [SerializeField] int _fadeTime;

    [Header("マップの光")]
    [SerializeField] GameObject _mapLight;

    [Tooltip("PlayerのObject")]
    private GameObject _playerObj;

    [Tooltip("アイテムのゲームオブジェクトをリストで管理する")]
    private List<GameObject> _itemObjList = new List<GameObject>();

    [Tooltip("ダンジョンにいる敵の総数")]
    private int _totalEnemyNum;
    //プレイヤーのいる部屋i
    // private int _playerRoomNum;
    //public int PlayerRoomNum => _playerRoomNum; S

    [Tooltip("現在の階層")]
    int _nowFloor = 1;

    //プレイヤーの座標
    private int _playerX;
    private int _playerY;

    private DgGenerator _dgGenerator;

    protected override void OnAwake()
    {

    }

    private void Start()
    {
        _dgGenerator = DgGenerator.Instance;
        _dgGenerator.MapNotice += MapInit;
    }

    /// <summary>マップの再生成による初期化</summary>
    void MapInit()
    {
        _itemObjList.Clear();
    }

    /// <summary>
    /// プレイヤーのポジションをセットする
    /// </summary>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    public void SetPlayerPosition(int x, int y)
    {
        _playerX = x;
        _playerY = y;
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

    /// <summary>リストから指定したインデックスのアイテムをリムーブする</summary>
    public void RemoveItemObjList(GameObject ItemObj)
    {
        _itemObjList.Remove(ItemObj);
    }

    /// <summary>
    /// Objectのアクティブ非アクティブを入れ替える
    /// </summary>
    public void SetLight(bool Setbool) 
    {
        _mapLight.SetActive(Setbool);
    }

    /// <summary>次の階層に移動する時に呼ぶメゾット</summary>
    public async void NextFloor()
    {
        TurnType = TurnManager.WaitTurn;
        _nowFloor++;

        //フェードが終わるまで待つ
        if (_nowFloor != _finalStratum)
        {
            //マップの生成
            _dgGenerator.MapGeneration();
            _nowFloorText.text = _nowFloor.ToString() + "F";
            await FadeWait();
            TurnType = TurnManager.Player;
        }
        else 
        {
            _nowFloorText.text = "最終層";
            _dgGenerator.MapGeneration();
            await FadeWait();
            Debug.Log("最終層");
            //マップの生成
            TurnType = TurnManager.Player;
        }
        

    }

    /// <summary>
    /// レベルアップしたときに呼ばれる
    /// </summary>
    private void PlayerLevelUpProcess()
    {
        //OutPutLog($"プレイヤーは{_playerStatus.Level + 1}にアップした");
    }

    async UniTask FadeWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_fadeTime));
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
