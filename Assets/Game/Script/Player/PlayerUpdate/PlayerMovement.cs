using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("GameManager�̃C���X�^���X")]
    GameManager _gameManager;

    [Header("PlayerMove�̃X�N���v�g")]
    [SerializeField] PlayerMove _playerMoveCs;
    [Header("PlayerAttack�̃X�N���v�g")]
    [SerializeField] PlayerAttack _playerAttackCs;
 
    void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.SetPlayerObj(this.gameObject);
    }

    /// <summary>
    /// �v���C�����ǂ��s�����邩���肷��
    /// </summary>
    void Update()
    {
        if (_gameManager.TurnType == GameManager.TurnManager.Player)
        {
            //�ړ��̓��͏���
            _playerMoveCs.MoveInputKey();
            //�v���C���[�̍U������
            _playerAttackCs.Attack();
        }
        else if (_gameManager.TurnType == GameManager.TurnManager.LogOpen)
        {

        }
        else if (_gameManager.TurnType == GameManager.TurnManager.Result)
        {

        }
        else if (_gameManager.TurnType == GameManager.TurnManager.Enemy) 
        {

        }
             
    }

}
