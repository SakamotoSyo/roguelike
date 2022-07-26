using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
  

    [Tooltip("�������g�̍��W")]
    protected int _startX;
    protected int _startY;

    [Tooltip("�ǂ�������Ώۂ̍��W")]
    protected int _goalX;
    protected int _goalY;

    protected int _yBool = -1;
    protected int _xBool = -1;

    [Tooltip("�ړ��������ǂ���")]
    protected bool _isMove = false;

    [Tooltip("�U���ł��邩�ǂ���")]
    protected bool _isAttack = false;

    [Tooltip("���̖ړI�n")]
    protected Vector3 _nextPosition;

    [Tooltip("DgGenerator�̃C���X�^���X")]
    protected DgGenerator _generatorIns;

    [Tooltip("�G�l�~�[�}�l�[�W���[�̃C���X�^���X")]
    protected EnemyManager _enemyManager;

    [Tooltip("�Q�[���}�l�[�W���[�̃C���X�^���X")]
    protected GameManager _gameManager;

    [Tooltip("EnemyStatus��Script")]
    [SerializeField]protected EnemyStatus _enemyStatus;

    [Tooltip("PlayerIDamageble")]
    protected IDamageble _playerBase;

    [Tooltip("�������ǂ��̕����ɂ��邩")]
    protected int _nowRoomNum;

    private Vector2 _position;

    public Vector3 EnemyPos => this.gameObject.transform.position; 


    protected void Start()
    {
        //�C���X�^���X���擾
        _gameManager = GameManager.Instance;
        _enemyManager = EnemyManager.Instance;
        _generatorIns = DgGenerator.Instance;
        //�G�l�~�[�}�l�[�W���[�Ɏ������g�̃I�u�W�F�N�g��n��
        _enemyManager.SetEnemyBaseList(this.gameObject.GetComponent<EnemyBase>());

        _playerBase = _gameManager.PlayerObj.GetComponent<IDamageble>();
    }

    protected virtual void Update()
    {
      
    }


    /// <summary>
    /// �G�̈ړ�AI
    /// </summary>
    public virtual void EnemyAction()
    {
        _startX = (int)transform.position.x;
        _startY = (int)transform.position.y;

        _goalX = (int)_gameManager.PlayerX;
        _goalY = -1 * (int)_gameManager.PlayerY;

        //�U���ł��邩�ǂ����m�F����
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
                    _playerBase.AddDamage(_enemyStatus.GetPower(), this.gameObject);
                    _isAttack = true;
                    //�R���[�`���ŃA�j���[�V�����̏����������Ă���������
                }
            }
        }


        if (!_isMove && !_isAttack)
        {
            EnemyMove();
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
            Debug.Log("EnemyActionEnd");
            _generatorIns.Layer.SetData((int)Mathf.Floor(transform.position.x), (int)Mathf.Floor(transform.position.y) * -1, 2);
            _isMove = false;
            _enemyManager.EnemyActionEnd = false;
        }
    }

    /// <summary>
    /// �ǂ��ړ����邩�̏���
    /// </summary>
    public virtual void EnemyMove() 
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

        if ((GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 1 || GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 3) && !_isMove)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(_xBool, _yBool);
            _isMove = true;
        }
        //Y���������Ƃ�X�������ɂ�������
        if ((GetMapData(_startX + _xBool, _startY * -1) == 1 || GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 3) && !_isMove && _yBool == 0)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(_xBool, 0);
            _isMove = true;
        }
        //X���������Ƃ�Y�����ɂ�������
        if ((GetMapData(_startX, _startY + _yBool * -1) == 1 || GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 3) && !_isMove && _xBool == 0)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(0, _yBool);
            _isMove = true;
        }

        //�ǂ��ɂ������Ȃ������ꍇ��������ē���
        if ((GetMapData(_startX + 1, _startY * -1) == 1 || GetMapData(_startX + 1, (_startY + _yBool) * -1) == 3) && !_isMove && _xBool == 1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(1, 0);
            _isMove = true;
        }

        if ((GetMapData(_startX - 1, _startY * -1) == 1 || GetMapData(_startX - 1, (_startY + _yBool) * -1) == 3) && !_isMove && _xBool == -1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(-1, 0);
            _isMove = true;
        }

        if ((GetMapData(_startX, (_startY * -1) - 1) == 1 || GetMapData(_startX + _xBool, (_startY * -1) - 1) == 3) && !_isMove && _yBool == 1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(0, 1);
            _isMove = true;
        }

        if ((GetMapData(_startX, (_startY * -1) + 1) == 1 || GetMapData(_startX + _xBool, (_startY * -1) + 1) == 3) && !_isMove && _yBool == -1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(0, -1);
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
            //EnemyManager.Instance.EnemyList[0].transform.position += new Vector3(_xBool, _yBool, 0);
            _generatorIns.Layer.SetData((int)transform.position.x, (int)transform.position.y * -1, 1);
        }
    }

    /// <summary>
    /// �}�b�v�f�[�^�̏������o��
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int GetMapData(int x, int y) 
    {
        return _generatorIns.Layer.GetMapData(x, y);
    }

  

    /// <summary> Enemy�ɂǂ��̕����ɍ�����̂��l���Z�b�g����i </summary>
    //public void SetRoomNum(int nowRoom) 
    //{
    //    _nowRoomNum = nowRoom;
    //}
}

//public struct EnemyStatusData 
//{
//    public float _enemyHp;
//    public float _power;
//    public float _exp;

//    public EnemyStatusData(float hp, float power, float exp) 
//    {
//        this._enemyHp = hp;
//        this._power = power;
//        this._exp = exp;
//    }
//}

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