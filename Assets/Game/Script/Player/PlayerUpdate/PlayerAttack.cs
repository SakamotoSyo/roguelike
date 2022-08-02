using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    EnemyManager _enemyManager;
    GameManager _gameManager;

    [Tooltip("playerStatus�̃X�N���v�g")]
    [SerializeField] PlayerStatus _playerStatus;

    [Tooltip("playerMove�̃X�N���v�g")]
    [SerializeField] PlayerMove _playerMoveCs;


    private void Start()
    {
        _gameManager = GameManager.Instance;
        _enemyManager = EnemyManager.Instance;
    }

    /// <summary>
    ///�v���C���[�̍U������
    /// </summary>
    public void Attack()
    {
        if (Input.GetButtonDown("Submit"))
        {
            Debug.Log("�U������΂ꂽ");
            //�v���C���[�������Ă�������ɓG�������ꍇ
            foreach (var i in _enemyManager.EnemyBaseList)
            {
                if (transform.position.x + _playerMoveCs.PlayerDirection.x == i.EnemyPos.x && transform.position.y + _playerMoveCs.PlayerDirection.y == i.EnemyPos.y)
                {
                    LogScript.Instance.OutPutLog("�U���̏�������������");
                    //�A�j���[�V�����̏���������
                    i.GetComponent<IDamageble>().AddDamage(_playerStatus.Power, this.gameObject);
                    //�_���[�W��^���I������珈�����I����
                    break;

                }
            }

            //�o���l���Q�b�g�������ǂ����m�F����
            _gameManager.TurnType = GameManager.TurnManager.Result;

            //���̐ݒ�
            //_gameManager.TurnType = GameManager.TurnManager.Enemy;
        }
    }

}
