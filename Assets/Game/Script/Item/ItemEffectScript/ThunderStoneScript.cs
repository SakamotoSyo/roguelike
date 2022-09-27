using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ThunderStoneScript : MonoBehaviour
{
    [Header("AnimatorのComponent")]
    [SerializeField] Animator _anim;

    [Header("このアイテムのダメージ")]
    [SerializeField] int _damage;

    [Header("どのくらいの範囲で攻撃するか")]
    [SerializeField] int _attackRange = 0;

    [Header("雷のエフェクト")]
    [SerializeField] GameObject ThunderObj;

    AnimatorStateInfo _stateInfo;

    GameManager _gameManager;

    async void Start()
    {
        _gameManager = GameManager.Instance;
       await AnimationWait();
        //アニメーションを再生
    }

    void Update()
    {
       _stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Colliderの範囲に入っている敵にダメージを与える
        if (collision.gameObject.tag == "Enemy" && collision.TryGetComponent(out IDamageble damage))
        {
            damage.AddDamage(_damage, _gameManager.PlayerObj);
        }
    }

    async UniTask AnimationWait()
    {
        //攻撃実行前のステートを取得しないように１フレーム待つ
        await UniTask.DelayFrame(1);
        _stateInfo = default;
        await UniTask.WaitUntil(() => 1f <= _stateInfo.normalizedTime);
        _gameManager.TurnType = GameManager.TurnManager.Enemy;
        Destroy(gameObject);
    }
}
