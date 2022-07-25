using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    [Tooltip("GameManagerのインスタンス")]
    GameManager _gameManager;

    [Tooltip("Enemyのプレハブ")]
    [SerializeField] private GameObject _enemyPrefab;

    [Tooltip("EnemyBaseのリスト")]
    private List<EnemyBase> _enemyBaseList = new List<EnemyBase>();
    public List<EnemyBase> EnemyBaseList => _enemyBaseList;

    [Tooltip("EnemyStatusのリスト")]
    private List<EnemyStatusData> _enemyStatusDataList = new List<EnemyStatusData>();
    public List<EnemyStatusData> EnemyStatusList => _enemyStatusDataList;
    [Tooltip("PlayerのStatus")]
    private PlayerStatus _playerStatus;

    //敵の総数のどこまで敵が行動したか
    public int EnemyActionCountNum = 0;

    //敵一体の攻撃が終わったかどうか
    public bool EnemyActionEnd = false;

    [Header("ダンジョンに湧かせたい敵の量")]
    [SerializeField] private int _totalEnemyNum;

    [Tooltip("ダンジョンの今の敵の総数")]
    private int _nowTotalEnemyNum;
    public int NowTotalEnemyNum => _nowTotalEnemyNum;

    private DgGenerator _generator;

    [Tooltip("現在の総EXP")]
    private float _totalEnemyExp;

    [Tooltip("何回levelアップするか")]
    private int _levelUpNum;


    void Start()
    {
        _generator = DgGenerator.Instance;
        _gameManager = GameManager.Instance;
    }

    void Update()
    {
        //敵の行動順を管理する
        EnemyActionMgr();

        //敵の生成を管理する
        EnemyGenerator();

        //倒されたEnemy分処理をする
        if (_gameManager.TurnType == GameManager.TurnManager.Result)
        {
            PlayerGetExp();

            //levelアップしない場合ターンを変更する
            if (_levelUpNum == 0)
            {
                _gameManager.TurnType = GameManager.TurnManager.Enemy;
            }
        }

    }

    /// <summary>
    /// ターンがEnemyに移った時に各Enemyの行動を始める
    /// </summary>
    private void EnemyActionMgr()
    {
        if (GameManager.Instance.TurnType == GameManager.TurnManager.Enemy && !EnemyActionEnd && _enemyBaseList.Count > EnemyActionCountNum)
        {
            EnemyActionEnd = true;
            _enemyBaseList[EnemyActionCountNum].EnemyAction();
            EnemyActionCountNum++;
        }
        //Enemyの行動がすべて終わったらプレイヤーのターンに移す
        else if (_enemyBaseList.Count <= EnemyActionCountNum && !EnemyActionEnd)
        {
            EnemyActionCountNum = 0;
            GameManager.Instance.TurnType = GameManager.TurnManager.Player;
        }
    }

    /// <summary>
    /// Playerに経験値を獲得させる処理
    /// </summary>
    private void PlayerGetExp()
    {

        foreach (var i in _enemyStatusDataList)
        {

            Debug.Log($"{i.Exp}経験値を手に入れた");
            var remainingExp = i.Exp;

            //レベルアップができなくなるまでループする
            while (_playerStatus.EXP - i.Exp < 0)
            {
                remainingExp -= _playerStatus.EXP;
                //レベルアップさせるための処理
                //プレイヤーを1LevelUpさせる
                _playerStatus.SetLevel(_playerStatus.Level + 1);
            }

            _playerStatus.SetExp(i.Exp);
        }

        _enemyStatusDataList.Clear();
        _gameManager.TurnType = GameManager.TurnManager.Enemy;
        //_gameManager.TurnType = GameManager.TurnManager.Enemy;
    }

    /// <summary>
    /// Enemyの生成を管理するメソッド
    /// </summary>
    private void EnemyGenerator()
    {
        if (_totalEnemyNum > _nowTotalEnemyNum && _generator.MapGenerateEnd)
        {
            _generator.Generatesomething(_enemyPrefab);
        }
    }

    /// <summary>
    /// 敵の総数に変更があった時に使う
    /// </summary>
    public void SetTotalEnemyNum(int num)
    {
        _nowTotalEnemyNum += num;
    }


    /// <summary>
    /// 総獲得EXPをセットする
    /// </summary>
    /// <param name="exp"></param>
    public void SetTotalExp(float exp)
    {
        _totalEnemyExp = exp;
    }

    /// <summary>
    /// リストに値をセットする関数
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
