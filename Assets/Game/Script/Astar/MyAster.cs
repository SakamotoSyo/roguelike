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
    [Tooltip("��������Node��ۑ����Ă����ϐ�")]
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
                //�܏��H�Ȃ̂ł����܂�
                Debug.Log("nononono");
                break;
            }
            if (node.X == _goalX && node.Y == _goalY)
            {
                //�S�[���ɂ��ǂ蒅����
                Debug.Log("Succes");
                _openNodeList.Remove(node);
                //�p�X���擾����
                node.GetPath(_parentNodeList);
                Debug.Log(_parentNodeList.Count);
                //���]����
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
        //�̈�O��������Return
        if (_layer2D.IsOutOfRange(x, y))
        {
            Debug.Log("�̈�O�ł�");
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
    /// ���W�ɑ΂��Ă�Node���擾����
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
    /// ����R�X�g�����߂�
    /// </summary>
    /// <param name="x">���݂�x���W</param>
    /// <param name="y">���݂̂����W</param>
    /// <returns>����R�X�g</returns>
    private int GetHeuristicCost(int x, int y)
    {
        //����R�X�g�͎΂߈ړ�������ꍇ�̓S�[���̈ʒu���猻�݂̈ʒu�������Z���A
        //�R�X�g�������ق��𐄒�R�X�g�ɂ���
        var a = _goalX - x;
        var b = _goalY - y;

        return a > b ? a : b;
    }

    private Node SearchMinimumCostNode()
    {
        int min = 9999;
        // �ŏ����R�X�g
        int minCost = 9999;
        Node minNode = null;
        foreach (Node node in _openNodeList)
        {
            int score = node.GetScore();
            if (score > min)
            {
                // �X�R�A���傫��
                continue;
            }
            if (score == min && node.Cost >= minCost)
            {
                // �X�R�A�������Ƃ��͎��R�X�g����r����
                continue;
            }

            // �ŏ��l�X�V.
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

