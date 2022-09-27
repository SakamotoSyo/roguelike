using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ThunderStoneScript : MonoBehaviour
{
    [Header("Animator��Component")]
    [SerializeField] Animator _anim;

    [Header("���̃A�C�e���̃_���[�W")]
    [SerializeField] int _damage;

    [Header("�ǂ̂��炢�͈̔͂ōU�����邩")]
    [SerializeField] int _attackRange = 0;

    [Header("���̃G�t�F�N�g")]
    [SerializeField] GameObject ThunderObj;

    AnimatorStateInfo _stateInfo;

    GameManager _gameManager;

    async void Start()
    {
        _gameManager = GameManager.Instance;
       await AnimationWait();
        //�A�j���[�V�������Đ�
    }

    void Update()
    {
       _stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Collider�͈̔͂ɓ����Ă���G�Ƀ_���[�W��^����
        if (collision.gameObject.tag == "Enemy" && collision.TryGetComponent(out IDamageble damage))
        {
            damage.AddDamage(_damage, _gameManager.PlayerObj);
        }
    }

    async UniTask AnimationWait()
    {
        //�U�����s�O�̃X�e�[�g���擾���Ȃ��悤�ɂP�t���[���҂�
        await UniTask.DelayFrame(1);
        _stateInfo = default;
        await UniTask.WaitUntil(() => 1f <= _stateInfo.normalizedTime);
        _gameManager.TurnType = GameManager.TurnManager.Enemy;
        Destroy(gameObject);
    }
}
