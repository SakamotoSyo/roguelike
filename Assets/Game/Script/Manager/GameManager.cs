using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class GameManager : SingletonBehaviour<GameManager>
{
    public enum TurnManager
    {
        Player,
        MenuOpen,
        Enemy,
        LogOpen,
    }

    public TurnManager TurnType;

    public GameObject PlayerObj => _playerObj;
    [Header("���O���o�͂���p�l��")]
    [SerializeField] private GameObject _logPanel;

    private GameObject _playerObj;

    [Tooltip("�A�C�e���̃Q�[���I�u�W�F�N�g�����X�g�ŊǗ�����")]
    private List<GameObject> _itemObjList = new List<GameObject>();
    public List<GameObject> ItemObjList => _itemObjList;
    private List<String> _logList = new List<string>();

    public int TotalEnemyNum => _totalEnemyNum;

    [Tooltip("�_���W�����ɂ���G�̑���")]
    private int _totalEnemyNum;

    //�v���C���[�̂��镔��i
    // private int _playerRoomNum;
    //public int PlayerRoomNum => _playerRoomNum; S

    //�v���C���[�̍��W
    private int _playerX;
    private int _playerY;
    //�J�v�Z����
    public int PlayerX => _playerX;
    public int PlayerY => _playerY;

    [Header("���b�Z�[�W���ǂ̒��x�\�����邩")]
    [SerializeField] private float _messageTime;

    private float _countTime;

    [Header("���O���o�͂���e�L�X�g")]
    [SerializeField] private Text _logText;

    private Coroutine _textCoroutine;

    private DgGenerator _dgGenerator;

    private void Start()
    {
        _dgGenerator = DgGenerator.Instance;
    }

    private void Update()
    {
        LogActive();
    }

    /// <summary>
    /// �v���C���[�̃|�W�V�������Z�b�g����
    /// </summary>
    /// <param name="x">x���W</param>
    /// <param name="y">y���W</param>
    public void SetPlayerPosition(int x, int y)
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

    /// <summary>
    /// �A�C�e���I�u�W�F�N�g�����X�g�ɃZ�b�g����
    /// </summary>
    /// <param name="obj"></param>
    public void SetItemObjList(GameObject obj)
    {
        _itemObjList.Add(obj);
    }

    /// <summary>
    /// �G�̑����𑝂₵���茸�炵���肷��
    /// </summary>
    /// <param name="num"></param>
    public void SetTotalEnemy(int num)
    {
        _totalEnemyNum += num;
    }

    /// <summary>
    /// ���X�g����w�肵���C���f�b�N�X�̃A�C�e���������[�u����
    /// </summary>
    /// <param name="a"></param>
    public void RemoveItemObjList(GameObject ItemObj)
    {
        _itemObjList.Remove(ItemObj);
    }

    /// <summary>
    /// Log�Ƀ��b�Z�[�W���o�͂���
    /// </summary>
    /// <param name="message">���b�Z�[�W</param>
    public void OutPutLog(string message)
    {

        //���ɑO�̃e�L�X�g���c���Ă����ꍇ���s����Text���o�͂���
        if (_logList.Count == 0)
        {
            _logList.Add(message);
            _logText.text = message;
        }
        else
        {
            var St = "\n" + message;
            _logList.Add(St);
            _logText.text += St;
        }

        //���Ƀp�l�����\������Ă�����p�l���̕\�����Ԃ����ɖ߂�
        if (_textCoroutine == null)
        {
            _textCoroutine = StartCoroutine(LogActive());

        }
        else 
        {
            StopCoroutine(_textCoroutine);
            _textCoroutine = null;
            _textCoroutine = StartCoroutine(LogActive());
        }
    }

    /// <summary>
    /// �p�l�����A�N�e�B�u��A�N�e�B�u�؂�ւ���
    /// </summary>
    private IEnumerator LogActive()
    {
        _logPanel.SetActive(true);
        yield return new WaitForSeconds(3);
        _logPanel.SetActive(false);

        StopCoroutine(_textCoroutine);
        _textCoroutine = null;
    }

    /// <summary>
    /// �v���C���[�̂��镔���𔻒肵�ĕϐ��ɓ����i
    /// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    //public void SetPlayerRoomNum(int x, int y) 
    //{
    //    for (int i = 0; i < _dgGenerator.GetDivList().Count; i++)
    //    {
    //        DgDivision a = _dgGenerator.GetDivList()[i];

    //        if (a.Room.Left <= x && a.Room.Right >= x && a.Room.Top >= y && a.Room.Bottom <= y)
    //        {
    //            _playerRoomNum = i;
    //        }

    //    }
    //}

    ///// <summary>���������݂ǂ̕����ɂ��邩�n�߂�i</summary>
    //public int GetRoomNum(int x, int y) 
    //{

    //    for (int i = 0; i < _dgGenerator.GetDivList().Count; i++) 
    //    {
    //       DgDivision a = DgGenerator.Instance.GetDivList()[i];

    //        if (a.Room.Left <= x && a.Room.Right >= x && a.Room.Top >= y && a.Room.Bottom <= y) 
    //        {
    //            return i;  
    //        }

    //    }

    //    return -1;
    //}

}
