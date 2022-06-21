using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Tooltip("�������g�̍��W")]
    private int _startX;
    private int _startY;

    [Tooltip("�ǂ�������Ώۂ̍��W")]
    private int _goalX;
    private int _goalY;

    private int _yBool = -1;
    private int _xBool = -1;

    [Tooltip("���������ǂ���")]
    private bool _isMove = false;
    [Tooltip("�U���ł��邩�ǂ���")]
    private bool _isAttack = false;

    private Vector3 _nextPosition;
    private DgGenerator _generatorIns;


    private void Start()
    {
        _generatorIns = DgGenerator.Instance;

       

    }

    private void Update()
    {
        if (GameManager.Instance.TurnType == GameManager.TurnManager.Enemy)
        {
            _startX = (int)EnemyManager.Instance.EnemyList[0].transform.position.x;
            _startY = (int)EnemyManager.Instance.EnemyList[0].transform.position.y;
            _goalX = (int)GameManager.Instance.PlayerX;
            _goalY = -1 * (int)GameManager.Instance.PlayerY;
            Move();
            MoveTest();
        }

    }



    private void MoveTest()
    {
       //�ړ�����
        transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);

        if (transform.position == _nextPosition)
        {
            _isMove = false;
            GameManager.Instance.TurnType = GameManager.TurnManager.Player;
        }
    }

    public virtual void Move()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var a = _startX + i - 1;
                var b = _startY + j - 1;
                //��������n���čU���Ώۂ������ꍇ�t���O���グ��
                if (a == _goalX && b == _goalY) 
                {
                    _isAttack = true;
                }
            }
        }


        if (!_isMove && !_isAttack)
        {


            _xBool = -1;
            _yBool = -1;

            //�ړI�n��X�����E�����������ꍇ
            if (_goalX - _startX > 0)
            {
                _xBool = 1;
            }
            //�ړI�n��X���������������ꍇ
            else if (_goalX - _startX == 0)
            {
                _xBool = 0;
            }

            //�ړI�n��Y�������������������ꍇ
            if (_goalY - _startY > 0)
            {
                _yBool = 1;
            }
            //�ړI�n��Y���������������ꍇ
            else if (_goalY - _startY == 0)
            {
                _yBool = 0;
            }

            if (_generatorIns.Layer.GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 1 && !_isMove)
            {
                _nextPosition = transform.position + new Vector3(_xBool, _yBool, 0);
                _isMove = true;
            }
            //Y���������Ƃ�X�������ɂ�������
            if (_generatorIns.Layer.GetMapData(_startX + _xBool, _startY * -1) == 1 && !_isMove && _xBool == 0)
            {
                _nextPosition = transform.position + new Vector3(_xBool, 0, 0);
                _isMove = true;
            }
            //X���������Ƃ�Y�����ɂ�������
            if (_generatorIns.Layer.GetMapData(_startX, _startY + _yBool * -1) == 1 && !_isMove && _yBool == 0)
            {
                _nextPosition = transform.position + new Vector3(0, _yBool, 0);
                _isMove = true;
            }

            //�ǂ��ɂ������Ȃ������ꍇ��������ē���
            if (_generatorIns.Layer.GetMapData(_startX + 1, _startY * -1) == 1 && !_isMove && _xBool == 1)
            {
                _nextPosition = transform.position + new Vector3(1, 0, 0);
                _isMove = true;
            }

            if (_generatorIns.Layer.GetMapData(_startX - 1, _startY * -1) == 1 && !_isMove && _xBool == -1)
            {
                _nextPosition = transform.position + new Vector3(-1, 0, 0);
                _isMove = true;
            }

            if (_generatorIns.Layer.GetMapData(_startX, (_startY * -1) - 1) == 1 && !_isMove && _yBool == 1)
            {
                _nextPosition = transform.position + new Vector3(0, 1, 0);
                _isMove = true;
            }

            if (_generatorIns.Layer.GetMapData(_startX, (_startY * -1) + 1) == 1 && !_isMove && _yBool == -1)
            {
                _nextPosition = transform.position + new Vector3(0, -1, 0);
                _isMove = true;
            }
            else
            {
                Debug.Log("�ǂł�");
            }


            if (!_isMove)
            {
                _xBool = 0;
                _yBool = 0;
            }
            else
            {
                //�Q�[���}�l�[�W���[�Ƀv���C���[�̏ꏊ��n��
                EnemyManager.Instance.EnemyList[0].transform.position += new Vector3(_xBool, _yBool, 0);


            }

        }
        else if (_isAttack) 
        {
            _isAttack = false;
            Debug.Log("�U������");
        }
    }
}

