using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    [SerializeField] private AsterTest _aster;
    [Tooltip("ゲームマネージャーのインスタンス")]
    private GameManager _gameManagerIns;
    [Tooltip("エネミーマネージャーのインスタンス")]
    private EnemyManager _enemyManagerIns;
    // Start is called before the first frame update
    void Start()
    {
        _gameManagerIns = GameManager.Instance;
        _enemyManagerIns = EnemyManager.Instance;
        //エネミーマネージャーに自分自身のオブジェクトを渡す
        _enemyManagerIns.EnemyList.Add(this.gameObject);
        //_aster.Aster();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Move()
    {
        //int value = DgGenerator.Instance.Layer.GetMapData();

    }
}
