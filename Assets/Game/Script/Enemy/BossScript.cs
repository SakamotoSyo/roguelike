using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : EnemyBase
{
    [Header("範囲攻撃を出す確率")]
    [SerializeField] int _rangeAttack;

    [Header("吹き飛ばし攻撃を出す確率")]
    [SerializeField] int _blowOffAttack;

    [Header("範囲攻撃のエフェクト")]
    [SerializeField] GameObject _rangeEffect;

    [Header("ItemEffect")]
    [SerializeField] ItemEffect _effectCs;

    protected override async void AddAttack()
    {
        //周囲３マス以内に主人公がいたときランダムに呼ばれる
        if (Mathf.Abs(_gameManager.PlayerX - transform.position.x) < 4 && Mathf.Abs((_gameManager.PlayerY * -1) - transform.position.y) < 4)
        {
            var ram = Random.Range(0, 101);

            if (ram < _rangeAttack)
            {
                //_isAttack = true;
                await EnemyAttack(2);
                if (_gameManager.PlayerY * -1 == transform.position.y)
                {
                    //エフェクトを生成する
                    Instantiate(_rangeEffect, _gameManager.PlayerObj.transform.position, new Quaternion(0, 0, 90, 90));
                }
                else 
                {
                    //エフェクトを生成する
                    Instantiate(_rangeEffect, _gameManager.PlayerObj.transform.position, transform.rotation);
                }                
            }
        }
        //周囲１マス以内に主人公がいたときランダムに呼ばれる
        if (Mathf.Abs(_gameManager.PlayerX - transform.position.x) < 2 && Mathf.Abs((_gameManager.PlayerY * -1) - transform.position.y) < 2)
        {
            var ram = Random.Range(0, 101);

            if (ram < _blowOffAttack)
            {
                //_isAttack = true;
                await EnemyAttack(3);
            }
        }

        else if ((_gameManager.PlayerX == transform.position.x || _gameManager.PlayerY * -1 == transform.position.y)
                 && Mathf.Abs(_gameManager.PlayerX - transform.position.x) < 4 && Mathf.Abs((_gameManager.PlayerY * -1) - transform.position.y) < 4) 
        {
            
        }

        
    }

    protected override async UniTask EnemyAttack(int count = 1)
    {
        await base.EnemyAttack(count);

        if (count == 2)
        {
            //攻撃実行前のステートを取得しないように１フレーム待つ
            await UniTask.DelayFrame(1);

            _stateInfo = default;
            _dir = new Vector2(_gameManager.PlayerX - (int)transform.position.x, _gameManager.PlayerY * -1 - (int)transform.position.y);

            await UniTask.WaitUntil(() => 0.5f <= _stateInfo.normalizedTime);

            //攻撃
            _playerBase.AddDamage(_enemyStatus.GetPower, this.gameObject);

            await UniTask.WaitUntil(() => 1f <= _stateInfo.normalizedTime);
        }
        else if (count == 3)
        {
            //PlayerについてるItemEffectから処理を呼ぶ
            _gameManager.PlayerObj.GetComponent<ItemEffect>().BlowAwayEffect(_dir);
            Debug.Log("吹き飛ばし攻撃");
        }
        else if (count == 4) 
        {
           
        }
    }
}
