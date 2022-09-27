using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Astar�m�[�h
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

        //�X�e�[�^�X
        private eStatus _status = eStatus.None;

        //���R�X�g
        private int _cost = 0;

        //�q���[���X�e�B�b�N�E�R�X�g
        int _heuristic = 0;

        //�e�̃m�[�h
        ANode _parent = null;


        //���W
        private int _x = 0;
        private int _y = 0;

        //�J�v�Z����
        public int X => _x;
        public int Y => _y;
        public int Cost => _cost;

        //�R���X�g���N�^
        public ANode(int x, int y)
        {
            _x = x;
            _y = y;
        }
        //�X�R�A���v�Z����
        public int GetScore()
        {
            return _cost + _heuristic;
        }

        //�q���[���X�e�B�b�N�E�R�X�g�̌v�Z
        public void CalcHeuristic(int xGoal, int yGoal)
        {
            //�΂߈ړ�
            var dx = (int)Mathf.Abs(xGoal - X);
            var dy = (int)Mathf.Abs(yGoal - Y);

            //�傫���ق����R�X�g�ɂ���
            _heuristic = dx > dy ? dx : dy;

        }

        //�X�e�[�^�X��None���ǂ���
        public bool IsNone()
        {
            return _status == eStatus.None;
        }

        //�X�e�[�^�X���I�[�v���ɂ���
        public void Open(ANode parent, int cost)
        {
            _status = eStatus.Open;
            _cost = cost;
            _parent = parent;
        }

        //�X�e�[�^�X��Closed�ɂ���
        public void Close()
        {
            _status = eStatus.Closed;
        }

        //�p�X���擾����
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
        //�I�[�v�����X�g
        private List<ANode> _openList = null;
        //�m�[�h�C���X�^���X�Ǘ�
        Dictionary<int, ANode> _pool = null;
        //�S�[�����W
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
        /// �m�[�h�𐶐�����
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public ANode GetNode(int x, int y)
        {
            var idx = DgGenerator.Instance.Layer.GetMapData(x, y);

            if (_pool.ContainsKey(idx))
            {
                //���ɑ��݂��Ă���̂Ńv�[�����O����擾
                return _pool[idx];
            }

            //�Ȃ��̂ŐV�K�쐬
            var node = new ANode(x, y);
            _pool[idx] = node;
            //�q���[���X�e�B�b�N�E�R�X�g���v�Z����
            node.CalcHeuristic(_xGoal, _yGoal);
            return node;
        }

        /// <summary>
        /// �m�[�h���I�[�v�����X�g�ɒǉ�����
        /// </summary>
        /// <param name="node"></param>
        public void AddOepnList(ANode node)
        {
            _openList.Add(node);
        }

        /// <summary>
        /// �m�[�h���I�[�v�����X�g����폜����
        /// </summary>
        /// <param name="node"></param>
        public void RemoveOpenList(ANode node)
        {
            _openList.Remove(node);
        }

        public ANode OpenNode(int x, int y, int cost, ANode parent)
        {

            //���W���`�F�b�N
            if (DgGenerator.Instance.Layer.IsOutOfRange(x, y))
            {
                //�̈�O
                return null;
            }
            if (DgGenerator.Instance.Layer.GetMapData(x, y) == 0)
            {
                //�ǂ������Ƃ��ʉ߂ł��Ȃ�
                return null;
            }
    
            //�m�[�h���擾����
            var node = GetNode(x, y);
            if (node.IsNone() == false)
            {
                //����Open���Ă���̂ŉ������Ȃ�
                return null;
            }
           
            //Open����
            node.Open(parent, cost);
            AddOepnList(node);

            return node;
        }

        /// <summary>
        /// ������I�[�v������
        /// </summary>
        /// <param name="parent"></param>
        public void OpenAround(ANode parent)
        {
            var xBase = parent.X; //��{���W(X)
            var yBase = parent.Y; //��{���W(Y)
            var cost = parent.Cost; //�R�X�g
            cost += 1; //����i�ނ̂�+1����

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
            //�ŏ��X�R�A
            int min = 9999;
            //�ŏ����R�X�g
            int minCost = 9999;
            ANode minNode = null;

            foreach (ANode node in _openList)
            {
                int score = node.GetScore();
                if (score < min)
                {
                    //�X�R�A���傫��
                    continue;
                }
                if (score == min && node.Cost >= minCost)
                {
                    //�X�R�A�������Ƃ��͎��R�X�g����r����
                    continue;
                }

                //�ŏ��l�X�V
                min = score;
                minCost = node.Cost;
                minNode = node;
            }

            return minNode;
        }
    }

    enum eState
    {
        Exec, // ���s��.
        Walk, // �ړ���.
        End,  // �����܂�.
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

        //�X�^�[�g�n�_�̃m�[�h���擾
        //�X�^�[�g�n�_�Ȃ̂ŃR�X�g��0
        ANode node = mgr.OpenNode(_startX, _startY, 0, null);
       mgr.AddOepnList(node);

        //���s�񐔁B1000�񒴂����狭�����f
        int cnt = 0;
        while (cnt < 1000) 
        {
            mgr.RemoveOpenList(node);
            //���͂��J��
            mgr.OpenAround(node);
            //�ŏ��X�R�A�̃m�[�h��T��
            node = mgr.SearchMinScoreNodeFromOpenList();
            Debug.Log(node);
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
                mgr.RemoveOpenList(node);
                //�p�X���擾����
                node.GetPath(pList);
                Debug.Log(pList.Count);
                //���]����
                pList.Reverse();
                break;
            }

           

        }

        _state = eState.Walk;
        //foreach���g�킸�Ɉ�x������������
      



        _state = eState.End;
        
    }

    private void Update()
    {
        
    }


}
