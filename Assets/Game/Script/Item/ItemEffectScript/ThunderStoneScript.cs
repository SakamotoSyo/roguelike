using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStoneScript : MonoBehaviour
{
    [Header("Animator��Component")]
    [SerializeField] Animator _animation;

    [Header("���̃A�C�e���̃_���[�W")]
    [SerializeField] int _damage;

    [Header("�ǂ̂��炢�͈̔͂ōU�����邩")]
    [SerializeField] int _attackRange = 0;

    GameManager _gameManager;

    void Start()
    {
        _gameManager = GameManager.Instance;

        //�A�j���[�V�������Đ�
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collider�͈̔͂ɓ����Ă���G�Ƀ_���[�W��^����
        if (collision.gameObject.tag == "Enemy" && collision.TryGetComponent(out IDamageble damage)) 
        {
            damage.AddDamage(_damage, _gameManager.PlayerObj);
        }
    }


}
