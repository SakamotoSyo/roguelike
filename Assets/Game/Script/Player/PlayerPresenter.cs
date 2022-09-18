using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class PlayerPresenter : MonoBehaviour
{
    [Header("プレイヤーのUI表示を管理する")]
    [SerializeField] PlayerUiView _playerUiView;
    [Tooltip("プレイヤのステータス")]
    PlayerStatus _playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        _playerStatus = GameManager.Instance.PlayerObj.GetComponent<PlayerStatus>();
        //最初に最大値の設定
        _playerStatus.MaxChanged.Subscribe(value => _playerUiView.SetMax((int)value));
        //変更があった時にViewに通知する
        _playerStatus.CurrentChanged.Subscribe(value => _playerUiView.SetCurrent((int)value)); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
