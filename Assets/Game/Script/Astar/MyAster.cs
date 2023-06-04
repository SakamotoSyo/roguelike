using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAster : MonoBehaviour
{
    public List<Node> ParentNodeList => _parentNodeList;
    [SerializeField] private int _mapHeight;
    [SerializeField] private int _mapWidth;
    [SerializeField] private int _startX = 0;
    [SerializeField] private int _startY = 0;
    [SerializeField] private int _goalX = 0;
    [SerializeField] private int _goalY = 0;
    [SerializeField] private bool _isDiagonal;
    private List<Node> _openNodeList = new List<Node>();
    private List<Node> _parentNodeList = new List<Node>();
    [Tooltip("生成したNodeを保存しておく変数")]
    private Dictionary<int, Node> _nodePool = new Dictionary<int, Node>();
    private Layer2D _layer2D;

    public class Node
    {
        public int X;
        public int Y;
        public int Heuristic;
        public int Cost;
        public Node NodeParent;
        public NodeState Status = NodeState.None;

        public Node(int x, int y, int heuristic, int cost)
        {
            X = x;
            Y = y;
            Heuristic = heuristic;
            Cost = cost;
        }

        public int GetScore()
        {
            return Heuristic + Cost;
        }

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Open()
        {
            if (Status == NodeState.None)
            {
                Status = NodeState.Open;
            }
        }

        public void GetPath(List<Node> nodeList)
        {
            nodeList.Add(this);
            if (NodeParent != null)
            {
                NodeParent.GetPath(nodeList);
            }
        }

        public enum NodeState
        {
            None,
            Close,
            Open,
        }

    }

    private void Start()
    {
       // StartAlgorithm();
    }

    public void StartAlgorithm(Layer2D layer)
    {
        _parentNodeList.Clear();
        _openNodeList.Clear();
        _nodePool.Clear();
        _layer2D = layer;
        AStar();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    private void AStar()
    {
        var count = 0;
        var node = new Node(_startX, _startY, GetHeuristicCost(_startX, _startY), 0);
        _openNodeList.Add(node);
        while (count < 10000)
        {
            _openNodeList.Remove(node);
            OpenAround(node);
            node = SearchMinimumCostNode();
            count++;
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
                _openNodeList.Remove(node);
                //パスを取得する
                node.GetPath(_parentNodeList);
                Debug.Log(_parentNodeList.Count);
                //反転する
                _parentNodeList.Reverse();
                break;
            }
        }
    }

    private void OpenAround(Node node)
    {
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                var x = node.X + i - 1;
                var y = node.Y + j - 1;
                OpenNode(x, y, node.Cost + _layer2D.GetMapData(x, y), node);
            }
        }
    }

    private void OpenNode(int x, int y, int cost, Node parent)
    {
        //領域外だったらReturn
        if (_layer2D.IsOutOfRange(x, y))
        {
            Debug.Log("領域外です");
            return;
        }

        var openNode = GetNode(x, y);
        if (openNode.Status != Node.NodeState.None)
        {
            return;
        }
        openNode.Open();
        _openNodeList.Add(openNode);
        openNode.NodeParent = parent;
        openNode.Heuristic = GetHeuristicCost(x, y);
        openNode.Cost = cost;
    }

    /// <summary>
    /// 座標に対してのNodeを取得する
    /// </summary>
    private Node GetNode(int x, int y)
    {
        var idx = _layer2D.ToIdx(x, y);
        if (_nodePool.ContainsKey(idx))
        {
            return _nodePool[idx];
        }

        var node = new Node(x, y, 0, 0);
        _nodePool.Add(idx, node);
        return node;

    }


    /// <summary>
    /// 推定コストを求める
    /// </summary>
    /// <param name="x">現在のx座標</param>
    /// <param name="y">現在のｙ座標</param>
    /// <returns>推定コスト</returns>
    private int GetHeuristicCost(int x, int y)
    {
        //推定コストは斜め移動がある場合はゴールの位置から現在の位置を引き算し、
        //コストが高いほうを推定コストにする
        var a = _goalX - x;
        var b = _goalY - y;

        return a > b ? a : b;
    }

    private Node SearchMinimumCostNode()
    {
        int min = 9999;
        // 最小実コスト
        int minCost = 9999;
        Node minNode = null;
        foreach (Node node in _openNodeList)
        {
            int score = node.GetScore();
            if (score > min)
            {
                // スコアが大きい
                continue;
            }
            if (score == min && node.Cost >= minCost)
            {
                // スコアが同じときは実コストも比較する
                continue;
            }

            // 最小値更新.
            min = score;
            minCost = node.Cost;
            minNode = node;
        }
        return minNode;
    }

    public void SetStartGoalPos(Vector2 start, Vector2 goal) 
    {
        _startX = (int)start.x;
        _startY = (int)start.y;
        _goalX = (int)goal.x;
        _startY = (int)goal.y;
    }

    public void SetStartGoalPos(int startX, int startY, int goalX, int goalY)
    {
        _startX = startX;
        _startY = startY;
        _goalX = goalX;
        _goalY = goalY;
    }
}

