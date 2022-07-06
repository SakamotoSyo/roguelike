using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IEnemyMove
{
    [SerializeField, Header("�̗�")] private float _enemyHp;
    [SerializeField, Header("�s����")] private int _actionNum;
    [SerializeField, Header("�U����")] private float _power;

    [SerializeField] private LayerMask _testLayerMask;

    [Tooltip("�������g�̍��W")]
    private int _startX;
    private int _startY;

    [Tooltip("�ǂ�������Ώۂ̍��W")]
    private int _goalX;
    private int _goalY;

    private int _yBool = -1;
    private int _xBool = -1;

    [Tooltip("�ړ��������ǂ���")]
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

    [Tooltip("PlayerBaseScript")]
    private IDamageble _playerBase;

    //[Tooltip("�������ǂ��̕����ɂ��邩")]
    //private int _nowRoomNum;

    private Vector2 _position;
    protected private void Start()
    {
        //�C���X�^���X���擾
        _gameManager = GameManager.Instance;
        _enemyManager = EnemyManager.Instance;
        _generatorIns = DgGenerator.Instance;
        //�G�l�~�[�}�l�[�W���[�Ɏ������g�̃I�u�W�F�N�g��n��
        _enemyManager.EnemyList.Add(this.gameObject);

        _playerBase = _gameManager.PlayerObj.GetComponent<IDamageble>();
    }

    protected virtual void Update()
    {
        _position = new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1) - new Vector2(this.transform.position.x, this.transform.position.y);
        Debug.DrawRay(transform.position, _position, Color.blue);
    }

    ///// <summary>
    ///// Player�̕�����Ray���΂�Player��IDamgage���擾���ă_���[�W��^����
    ///// </summary>
    //protected virtual IEnumerator Attack()
    //{
    //    //�G�l�~�[����v���C���[�΂�������x�N�g�����擾
    //    //_position = new Vector2(this.transform.position.x, this.transform.position.y) - new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1);
    //    _position = new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1) - new Vector2(this.transform.position.x, this.transform.position.y);


    //    //RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(_gameManager.PlayerX, _gameManager.PlayerY * -1), 10.0f, _testLayerMask);
    //    RaycastHit2D hit = Physics2D.Linecast(this.transform.position, _position, _testLayerMask);

    //    yield return new WaitForSeconds(0.1f);
 
    //    Debug.Log(hit);
    //    if (hit.collider.gameObject.TryGetComponent(out IDamageble ID))
    //    {
    //        ID.AddDamage(_power);
    //    }
     

    //    _enemyManager.EnemyActionEnd = false;
    //    _isAttack = false;

    //}

    /// <summary>
    /// �G�̈ړ�AI
    /// </summary>
    public virtual void Move()
    {
        _startX = (int)transform.position.x;
        _startY = (int)transform.position.y;

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
                    //�U������
                    _playerBase.AddDamage(_power);
                    _isAttack = true;
                    //�R���[�`���ŃA�j���[�V�����̏����������Ă���������
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
                Debug.Log("move");
                _isMove = true;
            }
            //Y���������Ƃ�X�������ɂ�������
            if (_generatorIns.Layer.GetMapData(_startX + _xBool, _startY * -1) == 1 && !_isMove && _yBool == 0)
            {
                _nextPosition = transform.position + new Vector3(_xBool, 0, 0);
                _isMove = true;
                Debug.Log("move1");
            }
            //X���������Ƃ�Y�����ɂ�������
            if (_generatorIns.Layer.GetMapData(_startX, _startY + _yBool * -1) == 1 && !_isMove && _xBool == 0)
            {
                _nextPosition = transform.position + new Vector3(0, _yBool, 0);
                _isMove = true;
                Debug.Log("move2");
            }

            //�ǂ��ɂ������Ȃ������ꍇ��������ē���
            if (_generatorIns.Layer.GetMapData(_startX + 1, _startY * -1) == 1 && !_isMove && _xBool == 1)
            {
                _nextPosition = transform.position + new Vector3(1, 0, 0);
                _isMove = true;
                Debug.Log("move3");
            }

            if (_generatorIns.Layer.GetMapData(_startX - 1, _startY * -1) == 1 && !_isMove && _xBool == -1)
            {
                _nextPosition = transform.position + new Vector3(-1, 0, 0);
                _isMove = true;
                Debug.Log("move4");
            }

            if (_generatorIns.Layer.GetMapData(_startX, (_startY * -1) - 1) == 1 && !_isMove && _yBool == 1)
            {
                _nextPosition = transform.position + new Vector3(0, 1, 0);
                _isMove = true;
                Debug.Log("move5");
            }

            if (_generatorIns.Layer.GetMapData(_startX, (_startY * -1) + 1) == 1 && !_isMove && _yBool == -1)
            {
                _nextPosition = transform.position + new Vector3(0, -1, 0);
                _isMove = true;
                Debug.Log("move6");
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
                //EnemyManager.Instance.EnemyList[0].transform.position += new Vector3(_xBool, _yBool, 0);
                _generatorIns.Layer.SetData((int)transform.position.x, (int)transform.position.y * -1, 1);
                _generatorIns.Layer.SetData((int)transform.position.x + _xBool, ((int)transform.position.y + _yBool) * -1 , 2);
                Debug.Log("���ݒn���X�V���܂���");
                
            }

        }
        else if (_isAttack)
        {
           _isAttack = false;
        }

        //���������ǂ��̕����ɂ��邩���肷��i
       // _nowRoomNum = _gameManager.GetRoomNum((int)transform.position.x, (int)transform.position.y);
        //�ړ�����
        transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);

        if (transform.position == _nextPosition)
        {
            
            _isMove = false;
            _enemyManager.EnemyActionEnd = false;
        }
    }

    /// <summary>
    /// �_���[�W���󂯂鏈��
    /// </summary>
    /// <param name="damage">�󂯂�_���[�W</param>
    public void AddDamage(float damage)
    {

    }

    ///// <summary>
    ///// Enemy�ɂǂ��̕����ɍ�����̂��l���Z�b�g����i
    ///// </summary>
    //public void SetRoomNum(int nowRoom) 
    //{
    //    _nowRoomNum = nowRoom;
    //}
}
