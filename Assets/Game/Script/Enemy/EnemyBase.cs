using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IEnemyMove
{
    [SerializeField, Header("�̗�")] private float _enemyHp;
    [SerializeField, Header("�s����")] private int _actionNum;
    [SerializeField, Header("�U����")] private float _power;


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

    [Tooltip("���̖ړI�n")]
    private Vector3 _nextPosition;

    [Tooltip("DgGenerator�̃C���X�^���X")]
    private DgGenerator _generatorIns;

    [Tooltip("�G�l�~�[�}�l�[�W���[�̃C���X�^���X")]
    private EnemyManager _enemyManager;

    [Tooltip("�Q�[���}�l�[�W���[�̃C���X�^���X")]
    private GameManager _gameManager;

    protected private void Start()
    {
        //�C���X�^���X���擾
        _gameManager = GameManager.Instance;
        _enemyManager = EnemyManager.Instance;
        _generatorIns = DgGenerator.Instance;
        //�G�l�~�[�}�l�[�W���[�Ɏ������g�̃I�u�W�F�N�g��n��
        _enemyManager.EnemyList.Add(this.gameObject);
    }

    /// <summary>
    /// Player�̕�����Ray���΂�Player��IDamgage���擾���ă_���[�W��^����
    /// </summary>
    protected virtual void Attack()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1), 10.0f);
        Debug.Log(hit.collider.gameObject);
        if (hit.collider)
        {
            Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1), Color.red);
        }

        if (hit.collider.gameObject.TryGetComponent(out IDamageble ID))
        {
            ID.AddDamage(_power);
        }

    }

    /// <summary>
    /// �G�̈ړ�AI
    /// </summary>
    public virtual void Move()
    {
        _startX = (int)_enemyManager.EnemyList[0].transform.position.x;
        _startY = (int)_enemyManager.EnemyList[0].transform.position.y;

        _goalX = (int)_gameManager.PlayerX;
        _goalY = -1 * (int)_gameManager.PlayerY;

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

            Attack();
        }

        //�ړ�����
        transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);

        if (transform.position == _nextPosition)
        {
            _isMove = false;
            GameManager.Instance.TurnType = GameManager.TurnManager.Player;
        }
    }

    /// <summary>
    /// �_���[�W���󂯂鏈��
    /// </summary>
    /// <param name="damage">�󂯂�_���[�W</param>
    public void AddDamage(float damage) 
    {
        
    }
}