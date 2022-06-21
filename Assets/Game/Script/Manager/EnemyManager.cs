using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    public enum EnemyState 
    {
        Wait,
        Move,
    }

    //“G‚ÌƒŠƒXƒg
    public List<GameObject> EnemyList = new List<GameObject>();

    private int EnemyCountNum = 0;

    public EnemyState _enemyState = EnemyState.Wait;

    private bool _isMoveing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyState == EnemyState.Move) 
        {
            //EnemyList[EnemyCountNum];
            _isMoveing = true;
        }
    }


}
