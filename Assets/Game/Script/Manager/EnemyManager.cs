using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    [Tooltip("Enemy�̃v���n�u")]
    [SerializeField]private GameObject _enemyPrefab;

    //�G�̃��X�g
    public List<GameObject> EnemyList = new List<GameObject>();

    //�G�̑����̂ǂ��܂œG���s��������
    public int EnemyActionCountNum = 0;

    //�G��̂̍U�����I��������ǂ���
    public bool EnemyActionEnd = false;

    [Header("�_���W�����ɗN���������G�̗�")]
    [SerializeField]private int _totalEnemyNum;

    [Tooltip("�_���W�����̍��̓G�̑���")]
    private int _nowTotalEnemyNum;
    public int NowTotalEnemyNum => _nowTotalEnemyNum;

    private DgGenerator _generator;

    void Start()
    {
        _generator = DgGenerator.Instance;
    }

    void Update()
    {
        //�G�̍s�������Ǘ�����
        EnemyActionMgr();

        //�G�̐������Ǘ�����
        EnemyGenerator();
        
    }

    /// <summary>
    /// �^�[����Enemy�Ɉڂ������ɊeEnemy�̍s�����n�߂�
    /// </summary>
    private void EnemyActionMgr() 
    {
        if (GameManager.Instance.TurnType == GameManager.TurnManager.Enemy && !EnemyActionEnd && EnemyList.Count > EnemyActionCountNum)
        {
            if (EnemyList[EnemyActionCountNum].TryGetComponent(out IEnemyMove IM))
            {
                EnemyActionEnd = true;
                IM.Move();
                Debug.Log("�G���s������");
                EnemyActionCountNum++;
            }
        }
        //Enemy�̍s�������ׂďI�������v���C���[�̃^�[���Ɉڂ�
        else if(EnemyList.Count <= EnemyActionCountNum && !EnemyActionEnd)
        {
            EnemyActionCountNum = 0;
            Debug.Log("�G�̍s�����I�����");
            GameManager.Instance.TurnType = GameManager.TurnManager.Player;
        }
    }

    /// <summary>
    /// Enemy�̐������Ǘ����郁�\�b�h
    /// </summary>
    private void EnemyGenerator() 
    {
        if (_totalEnemyNum > _nowTotalEnemyNum  && _generator.MapGenerateEnd) 
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

}
