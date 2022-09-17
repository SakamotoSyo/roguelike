using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
public class GameManager : SingletonBehaviour<GameManager>
{
    public enum TurnManager
    {
        Player,
        MenuOpen,
        Enemy,
        LogOpen,
        Result,
        WaitTurn,
        Story,
    }

    public TurnManager TurnType;

    public GameObject PlayerObj => _playerObj;

    public int TotalEnemyNum => _totalEnemyNum;
    public int PlayerX => _playerX;
    public int PlayerY => _playerY;
    public int NowFloor => _nowFloor;
    public int FinalFloor => _finalStratum;
    public List<GameObject> ItemObjList => _itemObjList;

    [Header("���݂̊K�w��\������e�L�X�g")]
    [SerializeField] Text _nowFloorText;

    [Header("�ŏI�K�w")]
    [SerializeField] int _finalStratum;

    [Header("�t�F�[�h�ɂ����鎞��")]
    [SerializeField] int _fadeTime;

    [Header("�}�b�v�̌�")]
    [SerializeField] GameObject _mapLight;

    [Tooltip("Player��Object")]
    private GameObject _playerObj;

    [Tooltip("�A�C�e���̃Q�[���I�u�W�F�N�g�����X�g�ŊǗ�����")]
    private List<GameObject> _itemObjList = new List<GameObject>();

    [Tooltip("�_���W�����ɂ���G�̑���")]
    private int _totalEnemyNum;
    //�v���C���[�̂��镔��i
    // private int _playerRoomNum;
    //public int PlayerRoomNum => _playerRoomNum; S

    [Tooltip("���݂̊K�w")]
    int _nowFloor = 1;

    //�v���C���[�̍��W
    private int _playerX;
    private int _playerY;

    private DgGenerator _dgGenerator;

    protected override void OnAwake()
    {

    }

    private void Start()
    {
        _dgGenerator = DgGenerator.Instance;
        _dgGenerator.MapNotice += MapInit;
    }

    /// <summary>�}�b�v�̍Đ����ɂ�鏉����</summary>
    void MapInit()
    {
        _itemObjList.Clear();
    }

    /// <summary>
    /// �v���C���[�̃|�W�V�������Z�b�g����
    /// </summary>
    /// <param name="x">x���W</param>
    /// <param name="y">y���W</param>
    public void SetPlayerPosition(int x, int y)
    {
        _playerX = x;
        _playerY = y;
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

    /// <summary>���X�g����w�肵���C���f�b�N�X�̃A�C�e���������[�u����</summary>
    public void RemoveItemObjList(GameObject ItemObj)
    {
        _itemObjList.Remove(ItemObj);
    }

    /// <summary>
    /// Object�̃A�N�e�B�u��A�N�e�B�u�����ւ���
    /// </summary>
    public void SetLight(bool Setbool) 
    {
        _mapLight.SetActive(Setbool);
    }

    /// <summary>���̊K�w�Ɉړ����鎞�ɌĂԃ��]�b�g</summary>
    public async void NextFloor()
    {
        TurnType = TurnManager.WaitTurn;
        _nowFloor++;

        //�t�F�[�h���I���܂ő҂�
        if (_nowFloor != _finalStratum)
        {
            //�}�b�v�̐���
            _dgGenerator.MapGeneration();
            _nowFloorText.text = _nowFloor.ToString() + "F";
            await FadeWait();
            TurnType = TurnManager.Player;
        }
        else 
        {
            _nowFloorText.text = "�ŏI�w";
            _dgGenerator.MapGeneration();
            await FadeWait();
            Debug.Log("�ŏI�w");
            //�}�b�v�̐���
            TurnType = TurnManager.Player;
        }
        

    }

    /// <summary>
    /// ���x���A�b�v�����Ƃ��ɌĂ΂��
    /// </summary>
    private void PlayerLevelUpProcess()
    {
        //OutPutLog($"�v���C���[��{_playerStatus.Level + 1}�ɃA�b�v����");
    }

    async UniTask FadeWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_fadeTime));
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
