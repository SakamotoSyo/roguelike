using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    [Tooltip("Enemyのプレハブ")]
    [SerializeField]private GameObject _enemyPrefab;

    //敵のリスト
    public List<GameObject> EnemyList = new List<GameObject>();

    //敵の総数のどこまで敵が行動したか
    public int EnemyActionCountNum = 0;

    //敵一体の攻撃が終わったかどうか
    public bool EnemyActionEnd = false;

    [Header("ダンジョンに湧かせたい敵の量")]
    [SerializeField]private int _totalEnemyNum;

    [Tooltip("ダンジョンの今の敵の総数")]
    private int _nowTotalEnemyNum;
    public int NowTotalEnemyNum => _nowTotalEnemyNum;

    private DgGenerator _generator;

    void Start()
    {
        _generator = DgGenerator.Instance;
    }

    void Update()
    {
        //敵の行動順を管理する
        EnemyActionMgr();

        //敵の生成を管理する
        EnemyGenerator();
        
    }

    /// <summary>
    /// ターンがEnemyに移った時に各Enemyの行動を始める
    /// </summary>
    private void EnemyActionMgr() 
    {
        if (GameManager.Instance.TurnType == GameManager.TurnManager.Enemy && !EnemyActionEnd && EnemyList.Count > EnemyActionCountNum)
        {
            if (EnemyList[EnemyActionCountNum].TryGetComponent(out IEnemyMove IM))
            {
                EnemyActionEnd = true;
                IM.Move();
                Debug.Log("敵が行動した");
                EnemyActionCountNum++;
            }
        }
        //Enemyの行動がすべて終わったらプレイヤーのターンに移す
        else if(EnemyList.Count <= EnemyActionCountNum && !EnemyActionEnd)
        {
            EnemyActionCountNum = 0;
            Debug.Log("敵の行動が終わった");
            GameManager.Instance.TurnType = GameManager.TurnManager.Player;
        }
    }

    /// <summary>
    /// Enemyの生成を管理するメソッド
    /// </summary>
    private void EnemyGenerator() 
    {
        if (_totalEnemyNum > _nowTotalEnemyNum  && _generator.MapGenerateEnd) 
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

}
