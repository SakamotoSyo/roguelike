using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    //敵のリスト
    public List<GameObject> EnemyList = new List<GameObject>();

    //敵の総数のどこまで敵が行動したか
    public int EnemyActionCountNum = 0;

    //敵一体の攻撃が終わったかどうか
    public bool EnemyActionEnd = false;

    private bool _isMoveing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        EnemyActionMgr();
        
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


}
