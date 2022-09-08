using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStoneScript : MonoBehaviour
{
    [Header("AnimatorのComponent")]
    [SerializeField] Animator _animation;

    [Header("このアイテムのダメージ")]
    [SerializeField] int _damage;

    [Header("どのくらいの範囲で攻撃するか")]
    [SerializeField] int _attackRange = 0;

    GameManager _gameManager;

    void Start()
    {
        _gameManager = GameManager.Instance;

        //アニメーションを再生
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Colliderの範囲に入っている敵にダメージを与える
        if (collision.gameObject.tag == "Enemy" && collision.TryGetComponent(out IDamageble damage)) 
        {
            damage.AddDamage(_damage, _gameManager.PlayerObj);
        }
    }


}
