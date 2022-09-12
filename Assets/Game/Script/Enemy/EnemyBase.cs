using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using UniRx.Triggers;
using UniRx;
public abstract class EnemyBase : MonoBehaviour
{
    [Header("�_�C�N�X�g���X�N���v�g")]
    [SerializeField] DaiksutoraCs _daiksutoraCs;

    [Header("�ړ��ɂ����鎞��")]
    [SerializeField] float _moveTime;

    [SerializeField] Animator _anim;

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
    protected AnimatorStateInfo stateInfo;

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
        stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
    }


    /// <summary>
    /// �G�̈ړ�AI
    /// </summary>
    public virtual async void EnemyAction()
    {
        _startX = (int)transform.position.x;
        _startY = (int)transform.position.y;

        _goalX = _gameManager.PlayerX;
        _goalY = -1 * _gameManager.PlayerY;


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
                    //_playerBase.AddDamage(_enemyStatus.GetPower(), this.gameObject);
                    // _anim.SetTrigger("Attack");
                    _isAttack = true;
                    await EnemyAttack();
                    //_enemyManager.EnemyActionEnd = false;

                    //�R���[�`���ŃA�j���[�V�����̏����������Ă���������
                }
            }
        }


        if (!_isMove && !_isAttack)
        {
            //var data = _daiksutoraCs.Dijkstra(_startX, _startY * -1);
            //Debug.Log($"{_startX - data.PlayerX}da{_startY - data.PlayerY * -1}");
            //_nextPosition = new Vector2(_startX - data.PlayerX, _startY  - data.PlayerY * -1);
            EnemyMove();
        }
        else if (_isAttack)
        {
            _isAttack = false;
        }

        //���������ǂ��̕����ɂ��邩���肷��i
        // _nowRoomNum = _gameManager.GetRoomNum((int)transform.position.x, (int)transform.position.y);

        if (_isMove)
        {
            _generatorIns.Layer.SetData((int)_nextPosition.x, (int)_nextPosition.y * -1, MapNum.EnemyNum);
            //�ړ�����
            DOTween.To(() => transform.position,
                       x => transform.position = x,
                       _nextPosition, _moveTime)
                       .OnComplete(() => transform.position = _nextPosition);

            _anim.SetBool("Move", true);
            _anim.SetFloat("x", _dir.x);
            _anim.SetFloat("y", _dir.y);
            //transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);
            StartCoroutine(NextJudge());

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
        //??I?n??X????????????????
        else if (_goalX - _startX == 0)
        {
            _xBool = 0;
        }

        //??I?n??Y????????????????????
        if (_goalY - _startY > 0)
        {
            _yBool = 1;
        }
        //??I?n??Y????????????????
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
            Debug.Log("�ǂł�");
        }


        if (!_isMove)
        {
            _xBool = 0;
            _yBool = 0;
            _enemyManager.EnemyActionEnd = false;
        }
        else
        {
            //�Q�[���}�l�[�W���[�Ƀv���C���[�̏ꏊ��n��
            //EnemyManager.Instance.EnemyList[0].transform.position += new Vector3(_xBool, _yBool, 0);
            _generatorIns.Layer.SetData((int)transform.position.x, (int)transform.position.y * -1, MapNum.LoadNum);
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
                _enemyManager.EnemyActionEnd = false;
                break;
            }
            yield return null;
        }
        yield return null;
    }


    async UniTask EnemyAttack()
    {
        _anim.SetTrigger("Attack");
        //�U�����s�O�̃X�e�[�g���擾���Ȃ��悤�ɂP�t���[���҂�
        await UniTask.DelayFrame(1);

        stateInfo = default;

        await UniTask.WaitUntil(() => 0.5f <= stateInfo.normalizedTime);
        //�U��
        _playerBase.AddDamage(_enemyStatus.GetPower(), this.gameObject);
        Debug.Log("�_���[�W��^����");

        await UniTask.WaitUntil(() => 1f <= stateInfo.normalizedTime);

        _enemyManager.EnemyActionEnd = false;

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