using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    [Tooltip("GameManagerのインスタンス")]
    GameManager _gameManager;

    [Tooltip("生成するEnemyのList")]
    [SerializeField] private List<GameObject> _enemyList;

    [Tooltip("EnemyBaseのリスト")]
    private List<EnemyBase> _enemyBaseList = new List<EnemyBase>();
    public List<EnemyBase> EnemyBaseList => _enemyBaseList;

    [Tooltip("EnemyのGameObjectリスト")]
    private List<GameObject> _enemyGameObjList = new List<GameObject>();
    public List<GameObject> EnemyGameObjList => _enemyGameObjList;

    [Tooltip("EnemyStatusのリスト")]
    private List<EnemyStatusData> _enemyStatusDataList = new List<EnemyStatusData>();
    public List<EnemyStatusData> EnemyStatusList => _enemyStatusDataList;
    [Tooltip("PlayerのStatus")]
    private PlayerStatus _playerStatus;

    //敵の総数のどこまで敵が行動したか
    bool _enemyAction;

    //敵一体の攻撃が終わったかどうか
    int _actionEnd = 0;

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
        _generator.MapNotice += MapInit;
    }

    void Update()
    {
        //敵の生成を管理する
        EnemyGenerator();

        //獲得した経験値を処理する
        ExpResult();

        //敵の行動順を管理する
        EnemyActionMgr();


    }

    void FixedUpdate()
    {
       
    }

    /// <summary>マップの再生成による初期化</summary>
    void MapInit() 
    {
        Debug.Log("削除する");
        Debug.Log(_enemyGameObjList.Count);
        _enemyBaseList.Clear();
        foreach (var i in _enemyGameObjList) 
        {
            Destroy(i);
        }
        _enemyGameObjList.Clear();
        _nowTotalEnemyNum = 0;
    }

    /// <summary>
    /// ターンがEnemyに移った時に各Enemyの行動を始める
    /// </summary>
    private async void EnemyActionMgr()
    {
        if (GameManager.Instance.TurnType == GameManager.TurnManager.Enemy) 
        {
            if (!_enemyAction)
            {
                _enemyAction = true;
                for (int i = 0; i < _enemyBaseList.Count; i++)
                {
                    await TestWait();
                    _enemyBaseList[i].EnemyAction();
                }
            }
            //Enemyの行動がすべて終わったらプレイヤーのターンに移す
            else if (_enemyAction && _actionEnd >= _enemyBaseList.Count)
            {
                _enemyAction = false;
                _actionEnd = 0;
                GameManager.Instance.TurnType = GameManager.TurnManager.Player;
            }
        }
        
    }

    public void EnemyActionEnd() 
    {
        _actionEnd++;
    }

    /// <summary>獲得した経験値を処理する</summary>
    void ExpResult() 
    {
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
    /// Playerに経験値を獲得させる処理
    /// </summary>
    private void PlayerGetExp()
    {
        if (_playerStatus == null)
        {
            _playerStatus = GameManager.Instance.PlayerObj.GetComponent<PlayerStatus>();
        }

        foreach (var i in _enemyStatusDataList)
        {

            Debug.Log($"{i.Exp}経験値を手に入れた");
            var remainingExp = i.Exp;

            //レベルアップができなくなるまでループする
            while (_playerStatus.EXP - remainingExp < 0)
            {
                remainingExp -= _playerStatus.EXP;
                //レベルアップさせるための処理
                //プレイヤーを1LevelUpさせる
                _playerStatus.LevelUpSetData(_playerStatus.Level + 1);

                //_playerStatus.SetExp(50);
            }

            _playerStatus.SetExp(_playerStatus.EXP - remainingExp);
        }

        //経験値を獲得し終わったのでListを初期化
        _enemyStatusDataList.Clear();
        _gameManager.TurnType = GameManager.TurnManager.Enemy;
    }

    /// <summary>
    /// Enemyの生成を管理するメソッド
    /// </summary>
    private void EnemyGenerator()
    {
        if (_totalEnemyNum > _nowTotalEnemyNum && _generator.MapGenerateEnd)
        {
            var a = UnityEngine.Random.Range(0, _enemyList.Count);
            Debug.Log("敵を生成しました");
            _nowTotalEnemyNum++;
            _generator.Generatesomething(_enemyList[a]);
        }
    }

    /// <summary>
    /// 敵の総数に変更があった時に使う
    /// </summary>
    public void SetNowEnemyNum(int num)
    {
        _nowTotalEnemyNum += num;
    }

    /// <summary>
    /// EnemyListにGameObjectをAddするためのメソッド
    /// </summary>
    public void SetEnemyObject(GameObject enemy) 
    {
        _enemyGameObjList.Add(enemy);
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

    /// <summary>
    /// プレイヤーがのリザルトが行われているときに使うデータ
    /// </summary>
    /// <param name="enemyStatus"></param>
    public void SetEnemyStatusList(EnemyStatusData enemyStatus)
    {
        _enemyStatusDataList.Add(enemyStatus);
    }

    /// <summary>
    /// 指定したEnemyに関するデータを削除する
    /// </summary>
    /// <param name="gameObject">指定するGameObject</param>
    public void RemoveEnemyData(GameObject gameObject)
    {
        foreach (var i in _enemyBaseList)
        {
            var EnemyObj = i.GetThisScriptObj();

            if (gameObject == EnemyObj)
            {
                _generator.Layer.SetData((int)EnemyObj.transform.position.x, (int)EnemyObj.transform.position.y * -1, 1);
                _enemyBaseList.Remove(i);
                break;
            }
        }
    }

    /// <summary>
    /// ダンジョンに湧く敵の総数を変化させることができる
    /// </summary>
    /// <param name="num"></param>
    public void SetTotalEnemyNum(int num) 
    {
        _totalEnemyNum = num;
    }

    async UniTask TestWait()
    {
        var t = 0.001f;
        if (Input.GetButton("Dash"))
        {
            
        }
        await UniTask.Delay(TimeSpan.FromSeconds(t));
    }
}
