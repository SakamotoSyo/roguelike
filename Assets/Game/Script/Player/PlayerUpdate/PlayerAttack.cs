using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerAttack : MonoBehaviour
{
    EnemyManager _enemyManager;
    GameManager _gameManager;

    [Header("Animator")]
    [SerializeField] Animator _anim;

    [Header("playerStatusのスクリプト")]
    [SerializeField] PlayerStatus _playerStatus;

    [Header("playerMoveのスクリプト")]
    [SerializeField] PlayerMove _playerMoveCs;

    [Header("ThunderのEffect")]
    [SerializeField] GameObject _thunderPrefab;

    [Header("AudioSource")]
    [SerializeField] AudioSource _audioSource;

     AnimatorStateInfo _stateInfo;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _enemyManager = EnemyManager.Instance;
    }

    void Update()
    {
        _stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
        
    }

    /// <summary>
    ///プレイヤーの攻撃処理
    /// </summary>
    public async UniTask Attack()
    {
        if (Input.GetButtonDown("Submit"))
        {
            //プレイヤーが向いている方向に敵がいた場合
            foreach (var i in _enemyManager.EnemyBaseList)
            {
                if (transform.position.x + _playerMoveCs.GetDirection().x == i.EnemyPos.x && transform.position.y + _playerMoveCs.GetDirection().y == i.EnemyPos.y)
                {
                    _audioSource.Play();
                   // LogScript.Instance.OutPutLog("攻撃の処理が成功した");
                    _anim.SetTrigger("Attack");
                    _gameManager.TurnType = GameManager.TurnManager.WaitTurn;
                    //攻撃実行前のステートを取得しないように１フレーム待つ
                    await UniTask.DelayFrame(1);

                    _stateInfo = default;
                    //_dir = new Vector2(_gameManager.PlayerX - (int)transform.position.x, _gameManager.PlayerY * -1 - (int)transform.position.y);

                    await UniTask.WaitUntil(() => 0.5f <= _stateInfo.normalizedTime);
                    Instantiate(_thunderPrefab, transform.position, transform.rotation);
                    //攻撃
                    i.GetComponent<IDamageble>().AddDamage(_playerStatus.Power, this.gameObject);

                    await UniTask.WaitUntil(() => 1f <= _stateInfo.normalizedTime);

                    //ダメージを与え終わったら処理を終える
                    break;

                }
            }

            //経験値をゲットしたかどうか確認する
            _gameManager.TurnType = GameManager.TurnManager.Result;
        }
    }

}
