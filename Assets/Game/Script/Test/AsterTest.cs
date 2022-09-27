using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Astarノード
public class AsterTest : MonoBehaviour
{
    private void Start()
    {
        Astar();
    }

    struct Point2
    {
        public int x;
        public int y;
        public Point2(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public void Set(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class ANode
    {
        enum eStatus
        {
            None,
            Open,
            Closed,
        }

        //ステータス
        private eStatus _status = eStatus.None;

        //実コスト
        private int _cost = 0;

        //ヒューリスティック・コスト
        int _heuristic = 0;

        //親のノード
        ANode _parent = null;


        //座標
        private int _x = 0;
        private int _y = 0;

        //カプセル化
        public int X => _x;
        public int Y => _y;
        public int Cost => _cost;

        //コンストラクタ
        public ANode(int x, int y)
        {
            _x = x;
            _y = y;
        }
        //スコアを計算する
        public int GetScore()
        {
            return _cost + _heuristic;
        }

        //ヒューリスティック・コストの計算
        public void CalcHeuristic(int xGoal, int yGoal)
        {
            //斜め移動
            var dx = (int)Mathf.Abs(xGoal - X);
            var dy = (int)Mathf.Abs(yGoal - Y);

            //大きいほうをコストにする
            _heuristic = dx > dy ? dx : dy;

        }

        //ステータスがNoneかどうか
        public bool IsNone()
        {
            return _status == eStatus.None;
        }

        //ステータスをオープンにする
        public void Open(ANode parent, int cost)
        {
            _status = eStatus.Open;
            _cost = cost;
            _parent = parent;
        }

        //ステータスをClosedにする
        public void Close()
        {
            _status = eStatus.Closed;
        }

        //パスを取得する
        public void GetPath(List<Point2> pList)
        {
            pList.Add(new Point2(X, Y));
            if (_parent != null)
            {
                _parent.GetPath(pList);
            }
        }
    }

    class ANodeMgr
    {
        bool _diagonalShift= true;
        //オープンリスト
        private List<ANode> _openList = null;
        //ノードインスタンス管理
        Dictionary<int, ANode> _pool = null;
        //ゴール座標
        private int _xGoal;
        private int _yGoal;

        public ANodeMgr(int xgoal, int ygoal)
        {
            _openList = new List<ANode>();
            _pool = new Dictionary<int, ANode>();
            _xGoal = xgoal;
            _yGoal = ygoal;
        }

        /// <summary>
        /// ノードを生成する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public ANode GetNode(int x, int y)
        {
            var idx = DgGenerator.Instance.Layer.GetMapData(x, y);

            if (_pool.ContainsKey(idx))
            {
                //既に存在しておるのでプーリングから取得
                return _pool[idx];
            }

            //ないので新規作成
            var node = new ANode(x, y);
            _pool[idx] = node;
            //ヒューリスティック・コストを計算する
            node.CalcHeuristic(_xGoal, _yGoal);
            return node;
        }

        /// <summary>
        /// ノードをオープンリストに追加する
        /// </summary>
        /// <param name="node"></param>
        public void AddOepnList(ANode node)
        {
            _openList.Add(node);
        }

        /// <summary>
        /// ノードをオープンリストから削除する
        /// </summary>
        /// <param name="node"></param>
        public void RemoveOpenList(ANode node)
        {
            _openList.Remove(node);
        }

        public ANode OpenNode(int x, int y, int cost, ANode parent)
        {

            //座標をチェック
            if (DgGenerator.Instance.Layer.IsOutOfRange(x, y))
            {
                //領域外
                return null;
            }
            if (DgGenerator.Instance.Layer.GetMapData(x, y) == 0)
            {
                //壁だったとき通過できない
                return null;
            }
    
            //ノードを取得する
            var node = GetNode(x, y);
            if (node.IsNone() == false)
            {
                //既にOpenしているので何もしない
                return null;
            }
           
            //Openする
            node.Open(parent, cost);
            AddOepnList(node);

            return node;
        }

        /// <summary>
        /// 周りをオープンする
        /// </summary>
        /// <param name="parent"></param>
        public void OpenAround(ANode parent)
        {
            var xBase = parent.X; //基本座標(X)
            var yBase = parent.Y; //基本座標(Y)
            var cost = parent.Cost; //コスト
            cost += 1; //一歩進むので+1する

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var x = xBase + i - 1;
                    var y = yBase + j - 1;
                    OpenNode(x, y, cost, parent);
                }
            }
        }

        public ANode SearchMinScoreNodeFromOpenList()
        {
            //最小スコア
            int min = 9999;
            //最小実コスト
            int minCost = 9999;
            ANode minNode = null;

            foreach (ANode node in _openList)
            {
                int score = node.GetScore();
                if (score < min)
                {
                    //スコアが大きい
                    continue;
                }
                if (score == min && node.Cost >= minCost)
                {
                    //スコアが同じときは実コストも比較する
                    continue;
                }

                //最小値更新
                min = score;
                minCost = node.Cost;
                minNode = node;
            }

            return minNode;
        }
    }

    enum eState
    {
        Exec, // 実行中.
        Walk, // 移動中.
        End,  // おしまい.
    }

    eState _state = eState.Exec;

    private int _startX;
    private int _startY;

    private int _goalX;
    private int _goalY;


    public void Astar()
    {

        _startX = (int)transform.position.x;
        _startY = (int)transform.position.y *-1;

        _goalX = GameManager.Instance.PlayerX;
        _goalY = GameManager.Instance.PlayerY;

        var pList = new List<Point2>();

        var mgr = new ANodeMgr(_goalX, _goalY);

        //スタート地点のノードを取得
        //スタート地点なのでコストは0
        ANode node = mgr.OpenNode(_startX, _startY, 0, null);
       mgr.AddOepnList(node);

        //試行回数。1000回超えたら強制中断
        int cnt = 0;
        while (cnt < 1000) 
        {
            mgr.RemoveOpenList(node);
            //周囲を開く
            mgr.OpenAround(node);
            //最小スコアのノードを探す
            node = mgr.SearchMinScoreNodeFromOpenList();
            Debug.Log(node);
            if (node == null) 
            {
                //袋小路なのでおしまい
                Debug.Log("nononono");
                break;
            }
            if (node.X == _goalX && node.Y == _goalY) 
            {
                //ゴールにたどり着いた
                Debug.Log("Succes");
                mgr.RemoveOpenList(node);
                //パスを取得する
                node.GetPath(pList);
                Debug.Log(pList.Count);
                //反転する
                pList.Reverse();
                break;
            }

           

        }

        _state = eState.Walk;
        //foreachを使わずに一度だけ処理する
      



        _state = eState.End;
        
    }

    private void Update()
    {
        
    }


}
