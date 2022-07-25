using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    [Tooltip("GameManager�̃C���X�^���X")]
    GameManager _gameManager;

    [Tooltip("Enemy�̃v���n�u")]
    [SerializeField] private GameObject _enemyPrefab;

    [Tooltip("EnemyBase�̃��X�g")]
    private List<EnemyBase> _enemyBaseList = new List<EnemyBase>();
    public List<EnemyBase> EnemyBaseList => _enemyBaseList;

    [Tooltip("EnemyStatus�̃��X�g")]
    private List<EnemyStatusData> _enemyStatusDataList = new List<EnemyStatusData>();
    public List<EnemyStatusData> EnemyStatusList => _enemyStatusDataList;
    [Tooltip("Player��Status")]
    private PlayerStatus _playerStatus;

    //�G�̑����̂ǂ��܂œG���s��������
    public int EnemyActionCountNum = 0;

    //�G��̂̍U�����I��������ǂ���
    public bool EnemyActionEnd = false;

    [Header("�_���W�����ɗN���������G�̗�")]
    [SerializeField] private int _totalEnemyNum;

    [Tooltip("�_���W�����̍��̓G�̑���")]
    private int _nowTotalEnemyNum;
    public int NowTotalEnemyNum => _nowTotalEnemyNum;

    private DgGenerator _generator;

    [Tooltip("���݂̑�EXP")]
    private float _totalEnemyExp;

    [Tooltip("����level�A�b�v���邩")]
    private int _levelUpNum;


    void Start()
    {
        _generator = DgGenerator.Instance;
        _gameManager = GameManager.Instance;
    }

    void Update()
    {
        //�G�̍s�������Ǘ�����
        EnemyActionMgr();

        //�G�̐������Ǘ�����
        EnemyGenerator();

        //�|���ꂽEnemy������������
        if (_gameManager.TurnType == GameManager.TurnManager.Result)
        {
            PlayerGetExp();

            //level�A�b�v���Ȃ��ꍇ�^�[����ύX����
            if (_levelUpNum == 0)
            {
                _gameManager.TurnType = GameManager.TurnManager.Enemy;
            }
        }

    }

    /// <summary>
    /// �^�[����Enemy�Ɉڂ������ɊeEnemy�̍s�����n�߂�
    /// </summary>
    private void EnemyActionMgr()
    {
        if (GameManager.Instance.TurnType == GameManager.TurnManager.Enemy && !EnemyActionEnd && _enemyBaseList.Count > EnemyActionCountNum)
        {
            EnemyActionEnd = true;
            _enemyBaseList[EnemyActionCountNum].EnemyAction();
            EnemyActionCountNum++;
        }
        //Enemy�̍s�������ׂďI�������v���C���[�̃^�[���Ɉڂ�
        else if (_enemyBaseList.Count <= EnemyActionCountNum && !EnemyActionEnd)
        {
            EnemyActionCountNum = 0;
            GameManager.Instance.TurnType = GameManager.TurnManager.Player;
        }
    }

    /// <summary>
    /// Player�Ɍo���l���l�������鏈��
    /// </summary>
    private void PlayerGetExp()
    {

        foreach (var i in _enemyStatusDataList)
        {

            Debug.Log($"{i.Exp}�o���l����ɓ��ꂽ");
            var remainingExp = i.Exp;

            //���x���A�b�v���ł��Ȃ��Ȃ�܂Ń��[�v����
            while (_playerStatus.EXP - i.Exp < 0)
            {
                remainingExp -= _playerStatus.EXP;
                //���x���A�b�v�����邽�߂̏���
                //�v���C���[��1LevelUp������
                _playerStatus.SetLevel(_playerStatus.Level + 1);
            }

            _playerStatus.SetExp(i.Exp);
        }

        _enemyStatusDataList.Clear();
        _gameManager.TurnType = GameManager.TurnManager.Enemy;
        //_gameManager.TurnType = GameManager.TurnManager.Enemy;
    }

    /// <summary>
    /// Enemy�̐������Ǘ����郁�\�b�h
    /// </summary>
    private void EnemyGenerator()
    {
        if (_totalEnemyNum > _nowTotalEnemyNum && _generator.MapGenerateEnd)
        {
            _generator.Generatesomething(_enemyPrefab);
        }
    }

    /// <summary>
    /// �G�̑����ɕύX�����������Ɏg��
    /// </summary>
    public void SetTotalEnemyNum(int num)
    {
        _nowTotalEnemyNum += num;
    }


    /// <summary>
    /// ���l��EXP���Z�b�g����
    /// </summary>
    /// <param name="exp"></param>
    public void SetTotalExp(float exp)
    {
        _totalEnemyExp = exp;
    }

    /// <summary>
    /// ���X�g�ɒl���Z�b�g����֐�
    /// </summary>
    /// <param name="enemyBase">EnemyBaseScript</param>
    public void SetEnemyBaseList(EnemyBase enemyBase)
    {
        _enemyBaseList.Add(enemyBase);
    }

    public void SetEnemyStatusList(EnemyStatusData enemyStatus)
    {
        _enemyStatusDataList.Add(enemyStatus);
    }
}
