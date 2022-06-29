using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : SingletonBehaviour<GameManager>
{
    public enum TurnManager
    {
        Player,
        MenuOpen,
        Enemy,
    }

    public TurnManager TurnType;

    private GameObject _playerObj;
    public GameObject PlayerObj => _playerObj;


    //�v���C���[�̍��W
    private int _playerX;
    private int _playerY;
    //�J�v�Z����
    public int PlayerX => _playerX;
    public int PlayerY => _playerY;
    


    /// <summary>
    /// �v���C���[�̃|�W�V�������Z�b�g����
    /// </summary>
    /// <param name="x">x���W</param>
    /// <param name="y">y���W</param>
    public void SetPlayerPosition(int x ,int y)
    {
        _playerX += x;
        _playerY += y;
    }

    /// <summary>
    /// �v���C���[�̍��W���Z�b�g����
    /// </summary>
    /// <param name="player">player�̃I�u�W�F�N�g</param>
    public void SetPlayerObj(GameObject player) 
    {
        _playerObj = player;
    }

}
