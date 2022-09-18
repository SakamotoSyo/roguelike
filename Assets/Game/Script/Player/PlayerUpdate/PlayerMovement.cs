using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("GameManager�̃C���X�^���X")]
    GameManager _gameManager;

    [Header("PlayerMove�̃X�N���v�g")]
    [SerializeField] PlayerMove _playerMoveCs;
    [Header("PlayerAttack�̃X�N���v�g")]
    [SerializeField] PlayerAttack _playerAttackCs;

    UIManager _uiManager;

    public bool FindStairs = false;

    void Awake()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _gameManager = GameManager.Instance;
        _gameManager.SetPlayerPosition((int)transform.position.x, (int)transform.position.y * -1);
        _gameManager.SetPlayerObj(this.gameObject);
    }

    /// <summary>
    /// �v���C�����ǂ��s�����邩���肷��
    /// </summary>
    async void Update()
    {
        if (_gameManager.TurnType == GameManager.TurnManager.Player)
        {
            //�����̑����ɊK�i������ǂ���
            StairCheck(false);
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
        else if (_gameManager.TurnType == GameManager.TurnManager.Story) 
        {

        }

    }

    async void FixedUpdate()
    {
        if (_gameManager.TurnType == GameManager.TurnManager.Player)
        {
            //�ړ��̓��͏���
            _playerMoveCs.MoveInputKey();
            //�v���C���[�̍U������
            await _playerAttackCs.Attack();
        }
    }

    public void StairCheck(bool find)
    {
        if (FindStairs) 
        {
            _gameManager.TurnType = GameManager.TurnManager.MenuOpen;
           // Debug.Log("�K�i������܂�");
            _uiManager.StairUI();
        }
        FindStairs = find;
    }
}
