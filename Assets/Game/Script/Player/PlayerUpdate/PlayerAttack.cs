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

    [Header("playerStatus�̃X�N���v�g")]
    [SerializeField] PlayerStatus _playerStatus;

    [Header("playerMove�̃X�N���v�g")]
    [SerializeField] PlayerMove _playerMoveCs;

    [Header("Thunder��Effect")]
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
    ///�v���C���[�̍U������
    /// </summary>
    public async UniTask Attack()
    {
        if (Input.GetButtonDown("Submit"))
        {
            //�v���C���[�������Ă�������ɓG�������ꍇ
            foreach (var i in _enemyManager.EnemyBaseList)
            {
                if (transform.position.x + _playerMoveCs.GetDirection().x == i.EnemyPos.x && transform.position.y + _playerMoveCs.GetDirection().y == i.EnemyPos.y)
                {
                    _audioSource.Play();
                   // LogScript.Instance.OutPutLog("�U���̏�������������");
                    _anim.SetTrigger("Attack");
                    _gameManager.TurnType = GameManager.TurnManager.WaitTurn;
                    //�U�����s�O�̃X�e�[�g���擾���Ȃ��悤�ɂP�t���[���҂�
                    await UniTask.DelayFrame(1);

                    _stateInfo = default;
                    //_dir = new Vector2(_gameManager.PlayerX - (int)transform.position.x, _gameManager.PlayerY * -1 - (int)transform.position.y);

                    await UniTask.WaitUntil(() => 0.5f <= _stateInfo.normalizedTime);
                    Instantiate(_thunderPrefab, transform.position, transform.rotation);
                    //�U��
                    i.GetComponent<IDamageble>().AddDamage(_playerStatus.Power, this.gameObject);

                    await UniTask.WaitUntil(() => 1f <= _stateInfo.normalizedTime);

                    //�_���[�W��^���I������珈�����I����
                    break;

                }
            }

            //�o���l���Q�b�g�������ǂ����m�F����
            _gameManager.TurnType = GameManager.TurnManager.Result;
        }
    }

}
