using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//メモ=>
//1. 初期化 (2次元配列作成・区画リスト作成)
//2.すべてを壁にする
//3.マップサイズで最初の区画を作る
//4.区画を分割していく
//5.区画内に部屋を作る
//6.部屋同士をつなげる通路を作る

/// <summary>
/// ダンジョンの自動生成
/// </summary>
public class DgGenerator : SingletonBehaviour<DgGenerator>
{
    //2次元配列情報
    public Layer2D Layer = null;

    //区画リスト
    private List<DgDivision> _divList = null;

    public List<DgDivision> DivList => _divList;

    /// <summary>マップの再生成を通知する</summary>
    public event System.Action MapNotice;

    [Header("高さを設定する")]
    [SerializeField] private int _height;

    [Header("幅を設定する")]
    [SerializeField] private int _width;

    [Header("Layer2Dに入れる壁を表す数字")]
    [SerializeField] private int _chipWall;

    [Header("生成時の壁と区画の余白")]
    [SerializeField] int _startMergin;

    [Header("部屋の最小サイズ")]
    [SerializeField] private int _minRoom;

    [Header("部屋周りの余白")]
    [SerializeField] private int _outerMergin;

    [Header("部屋の最大サイズ")]
    [SerializeField] private int _maxRoom;

    [Header("区分けした中の部屋の余白")]
    [SerializeField] private int _posMergin;
    #region タイルマップ変数
    //[Header("地面のタイルマップ")]
    //[SerializeField] private Tilemap _groundTileMap;

    //[Header("壁のタイルマップ")]
    //[SerializeField] private Tilemap _wallTileMap;

    //[Header("地面のタイルチップ")]
    //[SerializeField] private Tile _groundTileChip;

    //[Header("壁のタイルチップ")]
    //[SerializeField] private Tile _wallTileChip;

    //[Header("階段のタイルチップ")]
    //[SerializeField]private Tile _stairTileChip;
    #endregion

    [Header("Objectをまとめる親のPrefab")]
    [SerializeField] GameObject _mapParentPrefab;

    [Header("グラウンドのオブジェクト")]
    [SerializeField] private GameObject _kusanyaObject;

    [Header("岩のオブジェクト")]
    [SerializeField] private GameObject _iwaObject;

    [Header("階段のオブジェクト")]
    [SerializeField] private GameObject _stairSet;

    [Header("プレイヤーのオブジェクト")]
    [SerializeField] private GameObject _playerObject;

    [Header("最終層に行ったときに生成するPlayerPrefab")]
    [SerializeField] GameObject _lastPlayer;

    [Header("敵のプレハブ")]
    [SerializeField] private GameObject _enemyPrefab;

    [Header("ボスのプレハブ")]
    [SerializeField] GameObject _bossPrefab;

    [Header("トラップのプレハブ")]
    [SerializeField] private GameObject _trapPrefab;

    [Header("ItemDataBase")]
    [SerializeField] private ItemDataBase _itemDataBase;

    [Header("Itemのプレハブ")]
    [SerializeField] private GameObject _itemPrefab;

    [Tooltip("mapParentを生成したものを保存する")]
    GameObject _mapParent;

    [Tooltip("マップを生成し終わったかどうか")]
    private bool _mapGenerateEnd;
    public bool MapGenerateEnd => _mapGenerateEnd;

    private bool isVertical = false;

    GameManager _gameManager;
    //縦で分割するかどうか
    // Start is called before the first frame update
    protected override void OnAwake()
    {
        _gameManager = GameManager.Instance;
        MapNotice += MapInit;
        MapGeneration();
    }

    /// <summary>
    /// マップを生成する処理
    /// </summary>
    public void MapGeneration()
    {
        //最終層に行ったら生成する部屋の設定を変える
        LastFloorCheck();

        //マップの再生成による設定の初期化
        MapNotice();

        _mapGenerateEnd = false;

        //二次元配列に値を入れる
        Layer = new Layer2D(_width, _height);

        //区画リスト作成
        _divList = new List<DgDivision>();

        //すべてを壁にする
        Layer.Fill(MapNum.WallNum);

        //最初の区画を作る
        CreateDivision(_startMergin, _startMergin, _width - _startMergin, _height - _startMergin);

        //区画を分割する
        SplitDivison(isVertical);

        //区画に部屋を作る
        CreateRoom();

        //部屋同士をつなぐ
        ConnectRooms();

        //タイルをセットする
        SetTile();
    }


    /// <summary>
    /// 区画情報をリストに保存する
    /// </summary>
    /// <param name="left"></param>
    /// <param name="top"></param>
    /// <param name="right"></param>
    /// <param name="bottom"></param>
    void CreateDivision(int left, int top, int right, int bottom)
    {
        //矩形の情報をセットする
        DgDivision div = new DgDivision();
        div.Outer.Set(left, top, right, bottom);
        _divList.Add(div);
    }

    /// <summary>
    /// 区画を分割する
    /// </summary>
    private void SplitDivison(bool isVertical)
    {
        //末尾要素の取り出し
        DgDivision parent = _divList[_divList.Count - 1];
        _divList.Remove(parent);
        //子区画の要素を作る
        DgDivision child = new DgDivision();

        if (parent.Outer.Left + _minRoom + _outerMergin >= parent.Outer.Right - _minRoom - _outerMergin || parent.Outer.Top + _minRoom + _outerMergin >= parent.Outer.Bottom - _minRoom - _outerMergin)
        {
            //どちらかの線の長さが足りない場合親の区画を元に戻しておしまい
            //どちらも最大数まで分けると形が一定になるのでランダム要素をつけるためにどちらかの線の長さが足りない場合にする
            _divList.Add(parent);
            return;
        }

        //縦か横かに分割する
        if (isVertical)
        {
            //分割ポイントを求める*Topの初期値は０
            int a = parent.Outer.Left + (_minRoom + _outerMergin);
            int b = parent.Outer.Right - (_minRoom + _outerMergin);

            //AB間の距離を求める
            int ab = b - a;
            //最大の部屋サイズを超えないようにする
            ab = Mathf.Min(ab, _maxRoom);

            //分割点を決める
            int p = a + Random.Range(0, ab + 1);

            //子区画に情報を設定
            child.Outer.Set(p, parent.Outer.Top, parent.Outer.Right, parent.Outer.Bottom);

            //親のLeftを子区画の右端までに移動させる
            parent.Outer.Right = child.Outer.Left;
        }
        else
        {
            //分割ポイントを求める*Topの初期値は０
            int a = parent.Outer.Top + (_minRoom + _outerMergin);
            int b = parent.Outer.Bottom - (_minRoom + _outerMergin);

            //AB間の距離を求める
            int ab = b - a;
            //最大の部屋サイズを超えないようにする
            ab = Mathf.Min(ab, _maxRoom);

            //分割点を決める
            int p = a + Random.Range(0, ab + 1);

            //子区画に情報を設定
            child.Outer.Set(parent.Outer.Left, p, parent.Outer.Right, parent.Outer.Bottom);

            //親のTopを子区画の右端までに移動させる
            parent.Outer.Bottom = child.Outer.Top;
        }


        _divList.Add(parent);
        _divList.Add(child);
        SplitDivison(!isVertical);


    }

    private void CreateRoom()
    {
        foreach (var div in _divList)
        {
            //部屋の基準のサイズを決める
            int dw = div.Outer.Width() - _outerMergin;
            int dh = div.Outer.Height() - _outerMergin;

            //部屋の大きさをランダムに決める
            int sw = Random.Range(_minRoom, dw);
            int sh = Random.Range(_minRoom, dh);

            //部屋の最大サイズを超えないようにする
            sw = Mathf.Min(sw, _maxRoom);
            sh = Mathf.Min(sh, _maxRoom);

            //空きサイズを計算。区画から部屋のサイズを引いて求めている
            int rw = (dw - sw);
            int rh = (dh - sh);

            //部屋の左上の位置を決める
            int rx = Random.Range(0, rw) + _posMergin;
            int ry = Random.Range(0, rh) + _posMergin;

            int left = div.Outer.Left + rx;
            int right = left + sw;
            int top = div.Outer.Top + ry;
            int bottom = top + sh;

            //部屋のサイズを設定
            div.Room.Set(left, top, right, bottom);

            //部屋を通路にする
            FillRoom(div.Room);
        }

    }


    //範囲内を埋める
    private void FillRoom(DgDivision.DgRect room)
    {

        for (int j = room.Left; j <= room.Right; j++)
        {
            for (int i = room.Top; i <= room.Bottom; i++)
            {
                Layer.SetData(j, i, MapNum.LoadNum);
            }
        }

    }

    /// <summary>
    /// 指定した高さより低かったらfalse高かったらTure
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    private bool CheckDivisionSize(int a)
    {
        //指定の高さより低かった場合false
        return a <= _minRoom;
    }

    private void ConnectRooms()
    {

        for (int i = 0; i < _divList.Count - 1; i++)
        {
            DgDivision a = _divList[i];
            DgDivision b = _divList[i + 1];

            CreateRoad(a, b);

        }
    }

    /// <summary>
    /// どっちに道を伸ばすか
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    private void CreateRoad(DgDivision a, DgDivision b)
    {
        if (a.Outer.Right == b.Outer.Left)
        {
            //横に伸ばす
            CreateHorizontalRoad(a, b);
        }
        else
        {
            //縦に伸ばす
            CreateVerticalRoad(a, b);
        }
    }

    /// <summary>
    /// 横につなぐ道を作る
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    private void CreateHorizontalRoad(DgDivision a, DgDivision b)
    {
        int y1 = Random.Range(a.Room.Top, a.Room.Bottom);
        int y2 = Random.Range(b.Room.Top, b.Room.Bottom);

        for (int x = a.Room.Right; x < a.Outer.Right; x++)
        {
            Layer.SetData(x, y1, MapNum.LoadNum);
        }
        for (int x = b.Room.Left; x > b.Outer.Left; x--)
        {
            Layer.SetData(x, y2, MapNum.LoadNum);
        }
        for (int y = Mathf.Min(y1, y2), end = Mathf.Max(y1, y2); y <= end; y++)
        {
            Layer.SetData(a.Outer.Right, y, MapNum.LoadNum);
        }
    }


    /// <summary>
    /// 縦につなぐ道を作る
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    private void CreateVerticalRoad(DgDivision a, DgDivision b)
    {
        int x1 = Random.Range(a.Room.Left, a.Room.Right);
        int x2 = Random.Range(b.Room.Left, b.Room.Right);

        for (int y = a.Room.Bottom; y < a.Outer.Bottom; y++)
        {
            Layer.SetData(x1, y, MapNum.LoadNum);
        }
        for (int y = b.Room.Top; y > b.Outer.Top; y--)
        {
            Layer.SetData(x2, y, MapNum.LoadNum);
        }
        for (int x = Mathf.Min(x1, x2), end = Mathf.Max(x1, x2); x <= end; x++)
        {
            Layer.SetData(x, a.Outer.Bottom, MapNum.LoadNum);
        }
    }

    /// <summary>
    /// タイルチップをセットする
    /// </summary>
    private void SetTile()
    {
        for (int y = 0; y < Layer.Height; ++y)
        {
            for (int x = 0; x < Layer.Width; ++x)
            {
                //XYの値を入れて配列の中身の数字を持ってくる
                int a = Layer.GetMapData(x, y);

                //配列の中身の数字によってマップチップを入れてる。ここに罠などのギミックを生成する処理を追加してもいいかも
                if (a == 1)
                {
                    var v = Instantiate(_kusanyaObject, new Vector3(x, -1 * y, 0), _kusanyaObject.transform.rotation);
                    v.transform.parent = _mapParent.transform;
                    //TrapGenerator(x, y);
                }
                else if (a == 0)
                {
                    var t = Instantiate(_iwaObject, new Vector3(x, -1 * y, 0), _iwaObject.transform.rotation);
                    t.transform.parent = _mapParent.transform;
                }
            }
        }

        ///テスト用
        if (GameManager.Instance.PlayerObj == null)
        {
            Generatesomething(_playerObject);
        }
        else
        {
            PlayerRespawn();
        }

        //プレイヤーの生成
        if (_gameManager.NowFloor == _gameManager.FinalFloor)
        {
            Instantiate(_lastPlayer, new Vector3(24, -38, 0), transform.rotation);
            Instantiate(_bossPrefab, new Vector3(24, -29, 0), transform.rotation);
        }
        else 
        {
            //アイテムの生成
            ItemGeneratesomething();
            //階段をランダムに生成
            Generatesomething(_stairSet);
        }
        //敵の生成
        //Generatesomething(_enemyPrefab);
        _mapGenerateEnd = true;

    }


    /// <summary>
    /// オブジェクトを入れるとダンジョン内にランダムに生成する
    /// </summary>
    /// <param name="Iobject">インスタンスするオブジェクト</param>
    public void Generatesomething(GameObject Iobject)
    {
        //ランダムな区画を選択する
        int suffix = Random.Range(0, _divList.Count);

        //区画の中のランダムな場所を選択する
        int x = Random.Range(_divList[suffix].Room.Left, _divList[suffix].Room.Right);
        int y = Random.Range(_divList[suffix].Room.Top, _divList[suffix].Room.Bottom);

        var Object = Instantiate(Iobject, new Vector3(x, -1 * y, 0), _stairSet.transform.rotation);



        if (Iobject == _enemyPrefab)
        {
            Object.transform.parent = _mapParent.transform;
            //エネミーに今自分がどの部屋にいるか教えてあげる
            var EnemyScript = Object.GetComponent<EnemyBase>();
            //EnemyScript.SetRoomNum(suffix);
            //EnemyManager.Instance.SetTotalEnemyNum(1);
        }
        else if (Iobject == _stairSet)
        {
            Object.transform.parent = _mapParent.transform;
            Layer.SetData(x, y, MapNum.StairNum);
        }
    }

    /// <summary>Playerをリスポーンさせる処理</summary>
    public void PlayerRespawn()
    {
        //ランダムな区画を選択する
        int suffix = Random.Range(0, _divList.Count);

        //区画の中のランダムな場所を選択する
        int x = Random.Range(_divList[suffix].Room.Left, _divList[suffix].Room.Right);
        int y = Random.Range(_divList[suffix].Room.Top, _divList[suffix].Room.Bottom);

        GameManager.Instance.PlayerObj.transform.position = new Vector2(x, y * -1);
        GameManager.Instance.SetPlayerPosition(x, y);
    }

    /// <summary>
    /// ランダムな区画にアイテムを生成する
    /// </summary>
    public void ItemGeneratesomething()
    {
        foreach (var i in _divList)
        {
            //部屋の中にランダムな数のアイテムを生成する
            int ItemNum = Random.Range(0, 3);
            for (int j = 0; j < ItemNum; j++)
            {
                //アイテムの座標をかぶらせないために一度生成したものをリストに保存
                List<int> PosList = new List<int>();
                //部屋の中のランダムな座標を指定
                int x = Random.Range(i.Room.Left, i.Room.Right);
                int y = Random.Range(i.Room.Top, i.Room.Bottom);
                //被らない座標が出るまで回し続ける
                while (PosList.Contains(x + y))
                {
                    x = Random.Range(i.Room.Left, i.Room.Right);
                    y = Random.Range(i.Room.Top, i.Room.Bottom);
                }

                //生成した座標の保存
                PosList.Add(x + y);

                var ItemObject = Instantiate(_itemPrefab, new Vector3(x, -1 * y, 0), _itemPrefab.transform.rotation);
                ItemObject.transform.parent = _mapParent.transform;
                //インスタンス化したプレハブからスクリプトを取得する
                var ItemObjectCs = ItemObject.GetComponent<ItemObjectScript>();
                //データーベースの中からランダムなアイテムを取ってくる
                var ItemRan = _itemDataBase.GetRandamItemLists();

                //スクリプトに情報をセットする
                ItemObjectCs.SetItemInfor(ItemRan);
                ItemObjectCs.SetItemSprite(ItemRan.GetItemImage);

                //リストにアイテムのオブジェクトをセットする
                GameManager.Instance.SetItemObjList(ItemObject);
            }
        }
    }

    /// <summary>
    /// 罠を生成する
    /// </summary>
    private void TrapGenerator(int x, int y)
    {
        if (Random.Range(0, 101) > 95)
        {
            var v = Instantiate(_trapPrefab, new Vector3(x, -1 * y, 0), _kusanyaObject.transform.rotation);
        }
    }

    /// <summary>
    /// ランダムな部屋の情報を返す
    /// </summary>
    public DgDivision GetDivList()
    {
        int suffix = Random.Range(0, _divList.Count);
        return _divList[suffix];
    }

    /// <summary>設定を初期化する</summary>
    private void MapInit()
    {
        Layer = null;
        Destroy(_mapParent);
        _mapParent = Instantiate(_mapParentPrefab, new Vector3(0, 0, 0), transform.rotation);
    }

    /// <summary>
    ///Positionを使ってMapDataにアクセスする用の関数
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int LayerGetPos(int x, int y)
    {
        return Layer.GetMapData(x, y * -1);
    }

    /// <summary>
    ///Positionを使ってMapDataにアクセスする用の関数
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public void LayerSetPos(int x, int y, int SetNum)
    {
        Layer.SetData(x, y * -1, SetNum);
    }

    /// <summary>
    /// 最終階層かどうかチェックする
    /// </summary>
    void LastFloorCheck()
    {
        //最終層だったら
        if (_gameManager.NowFloor == _gameManager.FinalFloor)
        {
            #region 部屋の設定
            _height = 55;
            _width = 55;
            _startMergin = 10;
            _minRoom = 30;
            _outerMergin = 4;
            _maxRoom = 30;
            _posMergin = 2;
            #endregion
        }
    }

}

public class MapNum
{
    /// <summary>壁</summary>
    public const int WallNum = 0;
    /// <summary>道</summary>
    public const int LoadNum = 1;
    /// <summary>敵</summary>
    public const int EnemyNum = 2;
    /// <summary>アイテム</summary>
    public const int ItemNum = 3;
    /// <summary>階段</summary>
    public const int StairNum = 4;
}