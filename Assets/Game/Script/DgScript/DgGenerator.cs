using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����=>
//1. ������ (2�����z��쐬�E��惊�X�g�쐬)
//2.���ׂĂ�ǂɂ���
//3.�}�b�v�T�C�Y�ōŏ��̋������
//4.���𕪊����Ă���
//5.�����ɕ��������
//6.�������m���Ȃ���ʘH�����

/// <summary>
/// �_���W�����̎�������
/// </summary>
public class DgGenerator : SingletonBehaviour<DgGenerator>
{
    //2�����z����
    public Layer2D Layer = null;

    //��惊�X�g
    private List<DgDivision> _divList = null;

    public List<DgDivision> DivList => _divList;

    /// <summary>�}�b�v�̍Đ�����ʒm����</summary>
    public event System.Action MapNotice;

    [Header("������ݒ肷��")]
    [SerializeField] private int _height;

    [Header("����ݒ肷��")]
    [SerializeField] private int _width;

    [Header("Layer2D�ɓ����ǂ�\������")]
    [SerializeField] private int _chipWall;

    [Header("�������̕ǂƋ��̗]��")]
    [SerializeField] int _startMergin;

    [Header("�����̍ŏ��T�C�Y")]
    [SerializeField] private int _minRoom;

    [Header("��������̗]��")]
    [SerializeField] private int _outerMergin;

    [Header("�����̍ő�T�C�Y")]
    [SerializeField] private int _maxRoom;

    [Header("�敪���������̕����̗]��")]
    [SerializeField] private int _posMergin;
    #region �^�C���}�b�v�ϐ�
    //[Header("�n�ʂ̃^�C���}�b�v")]
    //[SerializeField] private Tilemap _groundTileMap;

    //[Header("�ǂ̃^�C���}�b�v")]
    //[SerializeField] private Tilemap _wallTileMap;

    //[Header("�n�ʂ̃^�C���`�b�v")]
    //[SerializeField] private Tile _groundTileChip;

    //[Header("�ǂ̃^�C���`�b�v")]
    //[SerializeField] private Tile _wallTileChip;

    //[Header("�K�i�̃^�C���`�b�v")]
    //[SerializeField]private Tile _stairTileChip;
    #endregion

    [Header("Object���܂Ƃ߂�e��Prefab")]
    [SerializeField] GameObject _mapParentPrefab;

    [Header("�O���E���h�̃I�u�W�F�N�g")]
    [SerializeField] private GameObject _kusanyaObject;

    [Header("��̃I�u�W�F�N�g")]
    [SerializeField] private GameObject _iwaObject;

    [Header("�K�i�̃I�u�W�F�N�g")]
    [SerializeField] private GameObject _stairSet;

    [Header("�v���C���[�̃I�u�W�F�N�g")]
    [SerializeField] private GameObject _playerObject;

    [Header("�ŏI�w�ɍs�����Ƃ��ɐ�������PlayerPrefab")]
    [SerializeField] GameObject _lastPlayer;

    [Header("�G�̃v���n�u")]
    [SerializeField] private GameObject _enemyPrefab;

    [Header("�{�X�̃v���n�u")]
    [SerializeField] GameObject _bossPrefab;

    [Header("�g���b�v�̃v���n�u")]
    [SerializeField] private GameObject _trapPrefab;

    [Header("ItemDataBase")]
    [SerializeField] private ItemDataBase _itemDataBase;

    [Header("Item�̃v���n�u")]
    [SerializeField] private GameObject _itemPrefab;

    [Tooltip("mapParent�𐶐��������̂�ۑ�����")]
    GameObject _mapParent;

    [Tooltip("�}�b�v�𐶐����I��������ǂ���")]
    private bool _mapGenerateEnd;
    public bool MapGenerateEnd => _mapGenerateEnd;

    private bool isVertical = false;

    GameManager _gameManager;
    //�c�ŕ������邩�ǂ���
    // Start is called before the first frame update
    protected override void OnAwake()
    {
        _gameManager = GameManager.Instance;
        MapNotice += MapInit;
        MapGeneration();
    }

    /// <summary>
    /// �}�b�v�𐶐����鏈��
    /// </summary>
    public void MapGeneration()
    {
        //�ŏI�w�ɍs�����琶�����镔���̐ݒ��ς���
        LastFloorCheck();

        //�}�b�v�̍Đ����ɂ��ݒ�̏�����
        MapNotice();

        _mapGenerateEnd = false;

        //�񎟌��z��ɒl������
        Layer = new Layer2D(_width, _height);

        //��惊�X�g�쐬
        _divList = new List<DgDivision>();

        //���ׂĂ�ǂɂ���
        Layer.Fill(MapNum.WallNum);

        //�ŏ��̋������
        CreateDivision(_startMergin, _startMergin, _width - _startMergin, _height - _startMergin);

        //���𕪊�����
        SplitDivison(isVertical);

        //���ɕ��������
        CreateRoom();

        //�������m���Ȃ�
        ConnectRooms();

        //�^�C�����Z�b�g����
        SetTile();
    }


    /// <summary>
    /// ���������X�g�ɕۑ�����
    /// </summary>
    /// <param name="left"></param>
    /// <param name="top"></param>
    /// <param name="right"></param>
    /// <param name="bottom"></param>
    void CreateDivision(int left, int top, int right, int bottom)
    {
        //��`�̏����Z�b�g����
        DgDivision div = new DgDivision();
        div.Outer.Set(left, top, right, bottom);
        _divList.Add(div);
    }

    /// <summary>
    /// ���𕪊�����
    /// </summary>
    private void SplitDivison(bool isVertical)
    {
        //�����v�f�̎��o��
        DgDivision parent = _divList[_divList.Count - 1];
        _divList.Remove(parent);
        //�q���̗v�f�����
        DgDivision child = new DgDivision();

        if (parent.Outer.Left + _minRoom + _outerMergin >= parent.Outer.Right - _minRoom - _outerMergin || parent.Outer.Top + _minRoom + _outerMergin >= parent.Outer.Bottom - _minRoom - _outerMergin)
        {
            //�ǂ��炩�̐��̒���������Ȃ��ꍇ�e�̋������ɖ߂��Ă����܂�
            //�ǂ�����ő吔�܂ŕ�����ƌ`�����ɂȂ�̂Ń����_���v�f�����邽�߂ɂǂ��炩�̐��̒���������Ȃ��ꍇ�ɂ���
            _divList.Add(parent);
            return;
        }

        //�c�������ɕ�������
        if (isVertical)
        {
            //�����|�C���g�����߂�*Top�̏����l�͂O
            int a = parent.Outer.Left + (_minRoom + _outerMergin);
            int b = parent.Outer.Right - (_minRoom + _outerMergin);

            //AB�Ԃ̋��������߂�
            int ab = b - a;
            //�ő�̕����T�C�Y�𒴂��Ȃ��悤�ɂ���
            ab = Mathf.Min(ab, _maxRoom);

            //�����_�����߂�
            int p = a + Random.Range(0, ab + 1);

            //�q���ɏ���ݒ�
            child.Outer.Set(p, parent.Outer.Top, parent.Outer.Right, parent.Outer.Bottom);

            //�e��Left���q���̉E�[�܂łɈړ�������
            parent.Outer.Right = child.Outer.Left;
        }
        else
        {
            //�����|�C���g�����߂�*Top�̏����l�͂O
            int a = parent.Outer.Top + (_minRoom + _outerMergin);
            int b = parent.Outer.Bottom - (_minRoom + _outerMergin);

            //AB�Ԃ̋��������߂�
            int ab = b - a;
            //�ő�̕����T�C�Y�𒴂��Ȃ��悤�ɂ���
            ab = Mathf.Min(ab, _maxRoom);

            //�����_�����߂�
            int p = a + Random.Range(0, ab + 1);

            //�q���ɏ���ݒ�
            child.Outer.Set(parent.Outer.Left, p, parent.Outer.Right, parent.Outer.Bottom);

            //�e��Top���q���̉E�[�܂łɈړ�������
            parent.Outer.Bottom = child.Outer.Top;
        }


        _divList.Add(parent);
        _divList.Add(child);
        SplitDivison(!isVertical);


    }

    private void CreateRoom()
    {
        foreach (var div in _divList)
        {
            //�����̊�̃T�C�Y�����߂�
            int dw = div.Outer.Width() - _outerMergin;
            int dh = div.Outer.Height() - _outerMergin;

            //�����̑傫���������_���Ɍ��߂�
            int sw = Random.Range(_minRoom, dw);
            int sh = Random.Range(_minRoom, dh);

            //�����̍ő�T�C�Y�𒴂��Ȃ��悤�ɂ���
            sw = Mathf.Min(sw, _maxRoom);
            sh = Mathf.Min(sh, _maxRoom);

            //�󂫃T�C�Y���v�Z�B��悩�畔���̃T�C�Y�������ċ��߂Ă���
            int rw = (dw - sw);
            int rh = (dh - sh);

            //�����̍���̈ʒu�����߂�
            int rx = Random.Range(0, rw) + _posMergin;
            int ry = Random.Range(0, rh) + _posMergin;

            int left = div.Outer.Left + rx;
            int right = left + sw;
            int top = div.Outer.Top + ry;
            int bottom = top + sh;

            //�����̃T�C�Y��ݒ�
            div.Room.Set(left, top, right, bottom);

            //������ʘH�ɂ���
            FillRoom(div.Room);
        }

    }


    //�͈͓��𖄂߂�
    private void FillRoom(DgDivision.DgRect room)
    {

        for (int j = room.Left; j <= room.Right; j++)
        {
            for (int i = room.Top; i <= room.Bottom; i++)
            {
                Layer.SetData(j, i, MapNum.LoadNum);
            }
        }

    }

    /// <summary>
    /// �w�肵���������Ⴉ������false����������Ture
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    private bool CheckDivisionSize(int a)
    {
        //�w��̍������Ⴉ�����ꍇfalse
        return a <= _minRoom;
    }

    private void ConnectRooms()
    {

        for (int i = 0; i < _divList.Count - 1; i++)
        {
            DgDivision a = _divList[i];
            DgDivision b = _divList[i + 1];

            CreateRoad(a, b);

        }
    }

    /// <summary>
    /// �ǂ����ɓ���L�΂���
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    private void CreateRoad(DgDivision a, DgDivision b)
    {
        if (a.Outer.Right == b.Outer.Left)
        {
            //���ɐL�΂�
            CreateHorizontalRoad(a, b);
        }
        else
        {
            //�c�ɐL�΂�
            CreateVerticalRoad(a, b);
        }
    }

    /// <summary>
    /// ���ɂȂ��������
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    private void CreateHorizontalRoad(DgDivision a, DgDivision b)
    {
        int y1 = Random.Range(a.Room.Top, a.Room.Bottom);
        int y2 = Random.Range(b.Room.Top, b.Room.Bottom);

        for (int x = a.Room.Right; x < a.Outer.Right; x++)
        {
            Layer.SetData(x, y1, MapNum.LoadNum);
        }
        for (int x = b.Room.Left; x > b.Outer.Left; x--)
        {
            Layer.SetData(x, y2, MapNum.LoadNum);
        }
        for (int y = Mathf.Min(y1, y2), end = Mathf.Max(y1, y2); y <= end; y++)
        {
            Layer.SetData(a.Outer.Right, y, MapNum.LoadNum);
        }
    }


    /// <summary>
    /// �c�ɂȂ��������
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    private void CreateVerticalRoad(DgDivision a, DgDivision b)
    {
        int x1 = Random.Range(a.Room.Left, a.Room.Right);
        int x2 = Random.Range(b.Room.Left, b.Room.Right);

        for (int y = a.Room.Bottom; y < a.Outer.Bottom; y++)
        {
            Layer.SetData(x1, y, MapNum.LoadNum);
        }
        for (int y = b.Room.Top; y > b.Outer.Top; y--)
        {
            Layer.SetData(x2, y, MapNum.LoadNum);
        }
        for (int x = Mathf.Min(x1, x2), end = Mathf.Max(x1, x2); x <= end; x++)
        {
            Layer.SetData(x, a.Outer.Bottom, MapNum.LoadNum);
        }
    }

    /// <summary>
    /// �^�C���`�b�v���Z�b�g����
    /// </summary>
    private void SetTile()
    {
        for (int y = 0; y < Layer.Height; ++y)
        {
            for (int x = 0; x < Layer.Width; ++x)
            {
                //XY�̒l�����Ĕz��̒��g�̐����������Ă���
                int a = Layer.GetMapData(x, y);

                //�z��̒��g�̐����ɂ���ă}�b�v�`�b�v�����Ă�B������㩂Ȃǂ̃M�~�b�N�𐶐����鏈����ǉ����Ă���������
                if (a == 1)
                {
                    var v = Instantiate(_kusanyaObject, new Vector3(x, -1 * y, 0), _kusanyaObject.transform.rotation);
                    v.transform.parent = _mapParent.transform;
                    //TrapGenerator(x, y);
                }
                else if (a == 0)
                {
                    var t = Instantiate(_iwaObject, new Vector3(x, -1 * y, 0), _iwaObject.transform.rotation);
                    t.transform.parent = _mapParent.transform;
                }
            }
        }

        ///�e�X�g�p
        if (GameManager.Instance.PlayerObj == null)
        {
            Generatesomething(_playerObject);
        }
        else
        {
            PlayerRespawn();
        }

        //�v���C���[�̐���
        if (_gameManager.NowFloor == _gameManager.FinalFloor)
        {
            Instantiate(_lastPlayer, new Vector3(24, -38, 0), transform.rotation);
            Instantiate(_bossPrefab, new Vector3(24, -29, 0), transform.rotation);
        }
        else 
        {
            //�A�C�e���̐���
            ItemGeneratesomething();
            //�K�i�������_���ɐ���
            Generatesomething(_stairSet);
        }
        //�G�̐���
        //Generatesomething(_enemyPrefab);
        _mapGenerateEnd = true;

    }


    /// <summary>
    /// �I�u�W�F�N�g������ƃ_���W�������Ƀ����_���ɐ�������
    /// </summary>
    /// <param name="Iobject">�C���X�^���X����I�u�W�F�N�g</param>
    public void Generatesomething(GameObject Iobject)
    {
        //�����_���ȋ���I������
        int suffix = Random.Range(0, _divList.Count);

        //���̒��̃����_���ȏꏊ��I������
        int x = Random.Range(_divList[suffix].Room.Left, _divList[suffix].Room.Right);
        int y = Random.Range(_divList[suffix].Room.Top, _divList[suffix].Room.Bottom);

        var Object = Instantiate(Iobject, new Vector3(x, -1 * y, 0), _stairSet.transform.rotation);



        if (Iobject == _enemyPrefab)
        {
            Object.transform.parent = _mapParent.transform;
            //�G�l�~�[�ɍ��������ǂ̕����ɂ��邩�����Ă�����
            var EnemyScript = Object.GetComponent<EnemyBase>();
            //EnemyScript.SetRoomNum(suffix);
            //EnemyManager.Instance.SetTotalEnemyNum(1);
        }
        else if (Iobject == _stairSet)
        {
            Object.transform.parent = _mapParent.transform;
            Layer.SetData(x, y, MapNum.StairNum);
        }
    }

    /// <summary>Player�����X�|�[�������鏈��</summary>
    public void PlayerRespawn()
    {
        //�����_���ȋ���I������
        int suffix = Random.Range(0, _divList.Count);

        //���̒��̃����_���ȏꏊ��I������
        int x = Random.Range(_divList[suffix].Room.Left, _divList[suffix].Room.Right);
        int y = Random.Range(_divList[suffix].Room.Top, _divList[suffix].Room.Bottom);

        GameManager.Instance.PlayerObj.transform.position = new Vector2(x, y * -1);
        GameManager.Instance.SetPlayerPosition(x, y);
    }

    /// <summary>
    /// �����_���ȋ��ɃA�C�e���𐶐�����
    /// </summary>
    public void ItemGeneratesomething()
    {
        foreach (var i in _divList)
        {
            //�����̒��Ƀ����_���Ȑ��̃A�C�e���𐶐�����
            int ItemNum = Random.Range(0, 3);
            for (int j = 0; j < ItemNum; j++)
            {
                //�A�C�e���̍��W�����Ԃ点�Ȃ����߂Ɉ�x�����������̂����X�g�ɕۑ�
                List<int> PosList = new List<int>();
                //�����̒��̃����_���ȍ��W���w��
                int x = Random.Range(i.Room.Left, i.Room.Right);
                int y = Random.Range(i.Room.Top, i.Room.Bottom);
                //���Ȃ����W���o��܂ŉ񂵑�����
                while (PosList.Contains(x + y))
                {
                    x = Random.Range(i.Room.Left, i.Room.Right);
                    y = Random.Range(i.Room.Top, i.Room.Bottom);
                }

                //�����������W�̕ۑ�
                PosList.Add(x + y);

                var ItemObject = Instantiate(_itemPrefab, new Vector3(x, -1 * y, 0), _itemPrefab.transform.rotation);
                ItemObject.transform.parent = _mapParent.transform;
                //�C���X�^���X�������v���n�u����X�N���v�g���擾����
                var ItemObjectCs = ItemObject.GetComponent<ItemObjectScript>();
                //�f�[�^�[�x�[�X�̒����烉���_���ȃA�C�e��������Ă���
                var ItemRan = _itemDataBase.GetRandamItemLists();

                //�X�N���v�g�ɏ����Z�b�g����
                ItemObjectCs.SetItemInfor(ItemRan);
                ItemObjectCs.SetItemSprite(ItemRan.GetItemImage);

                //���X�g�ɃA�C�e���̃I�u�W�F�N�g���Z�b�g����
                GameManager.Instance.SetItemObjList(ItemObject);
            }
        }
    }

    /// <summary>
    /// 㩂𐶐�����
    /// </summary>
    private void TrapGenerator(int x, int y)
    {
        if (Random.Range(0, 101) > 95)
        {
            var v = Instantiate(_trapPrefab, new Vector3(x, -1 * y, 0), _kusanyaObject.transform.rotation);
        }
    }

    /// <summary>
    /// �����_���ȕ����̏���Ԃ�
    /// </summary>
    public DgDivision GetDivList()
    {
        int suffix = Random.Range(0, _divList.Count);
        return _divList[suffix];
    }

    /// <summary>�ݒ������������</summary>
    private void MapInit()
    {
        Layer = null;
        Destroy(_mapParent);
        _mapParent = Instantiate(_mapParentPrefab, new Vector3(0, 0, 0), transform.rotation);
    }

    /// <summary>
    ///Position���g����MapData�ɃA�N�Z�X����p�̊֐�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int LayerGetPos(int x, int y)
    {
        return Layer.GetMapData(x, y * -1);
    }

    /// <summary>
    ///Position���g����MapData�ɃA�N�Z�X����p�̊֐�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public void LayerSetPos(int x, int y, int SetNum)
    {
        Layer.SetData(x, y * -1, SetNum);
    }

    /// <summary>
    /// �ŏI�K�w���ǂ����`�F�b�N����
    /// </summary>
    void LastFloorCheck()
    {
        //�ŏI�w��������
        if (_gameManager.NowFloor == _gameManager.FinalFloor)
        {
            #region �����̐ݒ�
            _height = 55;
            _width = 55;
            _startMergin = 10;
            _minRoom = 30;
            _outerMergin = 4;
            _maxRoom = 30;
            _posMergin = 2;
            #endregion
        }
    }

}

public class MapNum
{
    /// <summary>��</summary>
    public const int WallNum = 0;
    /// <summary>��</summary>
    public const int LoadNum = 1;
    /// <summary>�G</summary>
    public const int EnemyNum = 2;
    /// <summary>�A�C�e��</summary>
    public const int ItemNum = 3;
    /// <summary>�K�i</summary>
    public const int StairNum = 4;
}