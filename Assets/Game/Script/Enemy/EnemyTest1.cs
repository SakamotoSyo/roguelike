using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest1 : EnemyBase
{
    
    public override void EnemyMove() 
    {
        _xBool = Random.Range(-1, 2);
        _yBool = Random.Range(-1, 2);

        _nextPosition = (Vector2)transform.position + new Vector2(_xBool, _yBool);
        _isMove = true;
        _generatorIns.Layer.SetData((int)transform.position.x, (int)transform.position.y * -1, 1);

    }
}
