using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class PlayerPresenter : MonoBehaviour
{
    [Header("�v���C���[��UI�\�����Ǘ�����")]
    [SerializeField] PlayerUiView _playerUiView;
    [Tooltip("�v���C���̃X�e�[�^�X")]
    PlayerStatus _playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        _playerStatus = GameManager.Instance.PlayerObj.GetComponent<PlayerStatus>();
        //�ŏ��ɍő�l�̐ݒ�
        _playerStatus.MaxChanged.Subscribe(_ => _playerUiView.SetHp(_playerStatus.MaxHp, _playerStatus.CurrentHp));
        //�ύX������������View�ɒʒm����
        _playerStatus.CurrentChanged.Subscribe(_ => _playerUiView.SetHp(_playerStatus.MaxHp, _playerStatus.CurrentHp)); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
