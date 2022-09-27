using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using UniRx.Triggers;
using UniRx;
public abstract class EnemyBase : MonoBehaviour, IDirection
{
    [Header("�_�C�N�X�g���X�N���v�g")]
    [SerializeField] DaiksutoraCs _daiksutoraCs;

    [Header("�ړ��ɂ����鎞��")]
    [SerializeField] float _moveTime;

    [Header("�ړ��ł����")]
    [SerializeField] int _moveCount = 1;

    [Header("AudioSource")]
    [SerializeField] AudioSource _audioSource;

    [Header("�U���̉�")]
    [SerializeField] AudioClip _attackClip;

    [SerializeField] protected Animator _anim;

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

    [Tooltip("�i�񂾕�����ۑ����Ă����ϐ�")]
    protected Vector2 _dir;

    [Tooltip("DgGenerator�̃C���X�^���X")]
    protected DgGenerator _generatorIns;

    [Tooltip("�G�l�~�[�}�l�[�W���[�̃C���X�^���X")]
    protected EnemyManager _enemyManager;

    [Tooltip("�Q�[���}�l�[�W���[�̃C���X�^���X")]
    protected GameManager _gameManager;

    [Tooltip("EnemyStatus��Script")]
    [SerializeField] protected EnemyStatus _enemyStatus;

    [Tooltip("PlayerIDamageble")]
    protected IDamageble _playerBase;

    [Tooltip("�������ǂ��̕����ɂ��邩")]
    protected int _nowRoomNum;

    [Tooltip("�Ď�����A�j���[�V����������")]
    protected AnimatorStateInfo _stateInfo;

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
        _stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        _anim.SetFloat("x", _dir.x);
        _anim.SetFloat("y", _dir.y);
    }


    /// <summary>
    /// �G�̈ړ�AI
    /// </summary>
    public virtual async void EnemyAction()
    {
        for (int k = 0; k < _moveCount; k++)
        {
            //���������ǂ��̕����ɂ��邩���肷��i
            _nowRoomNum = _generatorIns.GetDivNum((int)transform.position.x, (int)transform.position.y);

            _startX = (int)transform.position.x;
            _startY = (int)transform.position.y;

            _goalX = _gameManager.PlayerX;
            _goalY = -1 * _gameManager.PlayerY;

            AddAttack();

            //�U���ł��邩�ǂ����m�F����
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var a = _startX + i - 1;
                    var b = _startY + j - 1;
                    //��������n���čU���Ώۂ������ꍇ�t���O���グ��
                    if (a == _goalX && b == _goalY && !_isAttack)
                    {
                        //�U������
                        await EnemyAttack();
                    }
                }
            }


            if (!_isMove && !_isAttack)
            {
                Debug.Log(_nowRoomNum == _generatorIns.GetDivNum(_gameManager.PlayerX, _gameManager.PlayerY * -1));
                //�v���C���[�Ɠ����������������ǐՂ���
                if (_nowRoomNum == _generatorIns.GetDivNum(_gameManager.PlayerX, _gameManager.PlayerY * -1) &&
                    DgGenerator.Instance.GetDivList((int)transform.position.x, (int)transform.position.y) != null)
                {
                    Debug.Log("��H");
                    //�_�C�N�X�g���J�n
                    var data = _daiksutoraCs.Dijkstra(_startX, _startY * -1);
                    //�ړ�
                    _dir = new Vector2(data.PlayerX - transform.position.x, transform.position.y * -1 - data.PlayerY);
                    _nextPosition = new Vector2(data.PlayerX, data.PlayerY * -1);
                    _isMove = true;
                }
                // �ʘH�ɂ���Ƃ��̓e�X�g�p�Ɏg���Ă������̂ŒǐՂ���
                if (_generatorIns.GetDivNum((int)transform.position.x, (int)transform.position.y) == -1
                   || (Mathf.Abs(transform.position.x - _gameManager.PlayerX) < 4 && Mathf.Abs(transform.position.y - _gameManager.PlayerY * -1) < 4))
                {
                    EnemyMove();
                }
                ////�ǂ���ł��Ȃ��Ƃ��̓����_���Ɉړ�����
                //else
                //{
                //    Debug.Log("soreigai");
                //    RandomMove();
                //}

            }
            else if (_isAttack)
            {
                _isAttack = false;
                break;
            }

            if (_isMove)
            {
                _generatorIns.Layer.SetData((int)transform.position.x, (int)transform.position.y * -1, MapNum.LoadNum);
                _generatorIns.Layer.SetData((int)_nextPosition.x, (int)_nextPosition.y * -1, MapNum.EnemyNum);
                //�ړ�����
                DOTween.To(() => transform.position,
                           x => transform.position = x,
                           _nextPosition, _moveTime)
                           .OnComplete(() => transform.position = _nextPosition);

                _anim.SetBool("Move", true);
                //transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);
                await StartCoroutine(NextJudge());

            }
        }

        _enemyManager.EnemyActionEnd();
    }

    /// <summary>
    /// �p����Œǉ������������������ɏ���
    /// </summary>
    protected virtual void AddAttack() { }

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
        //X����������������
        else if (_goalX - _startX == 0)
        {
            _xBool = 0;
        }

        //�ړI�n��Y�����E�����������ꍇ
        if (_goalY - _startY > 0)
        {
            _yBool = 1;
        }
        //Y����������������
        else if (_goalY - _startY == 0)
        {
            _yBool = 0;
        }
        // _xBool = (int)Mathf.Sign(_goalX - _startX);
        //_yBool = (int)Mathf.Sign(_goalY - _startY);

        //�ʏ�̈ړ�
        if ((GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 1 || GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 3) && !_isMove)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(_xBool, _yBool);
            _dir = new Vector2(_xBool, _yBool);
            _isMove = true;
        }
        //Y���������Ƃ�X�������ɂ�������
        else if ((GetMapData(_startX + _xBool, _startY * -1) == 1 || GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 3) && !_isMove && _yBool == 0)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(_xBool, 0);
            _dir = new Vector2(_xBool, 0);
            _isMove = true;
        }
        //X���������Ƃ�Y�����ɂ�������
        else if ((GetMapData(_startX, _startY + _yBool * -1) == 1 || GetMapData(_startX + _xBool, (_startY + _yBool) * -1) == 3) && !_isMove && _xBool == 0)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(0, _yBool);
            _dir = new Vector2(0, _yBool);
            _isMove = true;
        }

        //�ǂ��ɂ������Ȃ������ꍇ��������ē���
        if ((GetMapData(_startX + 1, _startY * -1) == 1 || GetMapData(_startX + 1, (_startY + _yBool) * -1) == 3) && !_isMove && _xBool == 1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(1, 0);
            _dir = new Vector2(1, 0);
            _isMove = true;
        }
        else if ((GetMapData(_startX - 1, _startY * -1) == 1 || GetMapData(_startX - 1, (_startY + _yBool) * -1) == 3) && !_isMove && _xBool == -1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(-1, 0);
            _dir = new Vector2(-1, 0);
            _isMove = true;
        }
        else if ((GetMapData(_startX, (_startY * -1) - 1) == 1 || GetMapData(_startX + _xBool, (_startY * -1) - 1) == 3) && !_isMove && _yBool == 1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(0, 1);
            _dir = new Vector2(0, 1);
            _isMove = true;
        }
        else if ((GetMapData(_startX, (_startY * -1) + 1) == 1 || GetMapData(_startX + _xBool, (_startY * -1) + 1) == 3) && !_isMove && _yBool == -1)
        {
            _nextPosition = (Vector2)transform.position + new Vector2(0, -1);
            _dir = new Vector2(0, -1);
            _isMove = true;
        }
        else
        {
            // Debug.Log("�ǂł�");
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
        }
    }

    /// <summary>
    /// �����_���Ɉړ�����
    /// </summary>
    void RandomMove()
    {
        List<Vector2> moveList = new List<Vector2>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var x = i - 1;
                var y = j - 1;
                Debug.Log(GetMapData((int)transform.position.x, (int)transform.position.y * -1));
                if (GetMapData((int)transform.position.x + x, (int)transform.position.y * -1 + y * -1) == MapNum.LoadNum)
                {
                    moveList.Add(new Vector2((int)transform.position.x + x, (int)transform.position.y + y));
                    _nextPosition = new Vector2((int)transform.position.x + x, (int)transform.position.y + y);
                    _isMove = true;
                    Debug.Log("�����_����");
                    return;
                }
            }
        }

        _nextPosition = moveList[UnityEngine.Random.Range(0, moveList.Count)];
        _dir = new Vector2(transform.position.x - _nextPosition.x, transform.position.y - _nextPosition.y);
        _isMove = true;
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

    /// <summary>
    ///���̃X�N���v�g�����Ă���GameObject��Ԃ�
    /// </summary>
    /// <returns></returns>
    public GameObject GetThisScriptObj()
    {
        return this.gameObject;
    }


    /// <summary>
    /// �w�肵���ړI�n�ɒ����܂Ń��[�v���񂵑�����֐�
    /// </summary>
    /// <returns></returns>
    IEnumerator NextJudge()
    {
        while (true)
        {
            if (transform.position == _nextPosition)
            {
                _anim.SetBool("Move", false);
                _isMove = false;
                break;
            }
            yield return null;
        }
        yield return null;
    }

    /// <summary>
    /// �G�̍U������
    /// </summary>
    /// <param name="count">�U���̎��</param>
    /// <returns></returns>
    protected virtual async UniTask EnemyAttack(int count = 1)
    {
        _isAttack = true;
        var ramCount = count;
        _anim.SetTrigger($"Attack{ramCount}");
        if (count == 1)
        {
            //�U�����s�O�̃X�e�[�g���擾���Ȃ��悤�ɂP�t���[���҂�
            await UniTask.DelayFrame(1);

            _audioSource.PlayOneShot(_attackClip);
            _stateInfo = default;
            _dir = new Vector2(_gameManager.PlayerX - (int)transform.position.x, _gameManager.PlayerY * -1 - (int)transform.position.y);

            await UniTask.WaitUntil(() => 0.5f <= _stateInfo.normalizedTime);
            //�U��
            _playerBase.AddDamage(_enemyStatus.GetPower, this.gameObject);
            Debug.Log("�_���[�W��^����");

            await UniTask.WaitUntil(() => 1f <= _stateInfo.normalizedTime);
        }

    }

    public Vector2 GetDirection() => _dir;
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