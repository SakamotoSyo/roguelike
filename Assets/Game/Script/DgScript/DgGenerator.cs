using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
public class DgGenerator : SingletonBehaviour<DgGenerator>, IGeneratesomething
{

    //2�����z����
    private Layer2D _layer = null;

    //�J�v�Z����
    public Layer2D Layer => _layer;

    //��惊�X�g
    private List<DgDivision> _divList = null;

    public List<DgDivision> DivList => _divList;

    [Header("������ݒ肷��")]
    [SerializeField] private int _height;

    [Header("����ݒ肷��")]
    [SerializeField] private int _width;

    [Header("Layer2D�ɓ����ǂ�\������")]
    [SerializeField] private int _chipWall;

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

    [Header("�O���E���h�̃I�u�W�F�N�g")]
    [SerializeField]private GameObject _kusanyaObject;

    [Header("��̃I�u�W�F�N�g")]
    [SerializeField] private GameObject _iwaObject;

    [Header("�K�i�̃I�u�W�F�N�g")]
    [SerializeField] private GameObject _stairSet;

    [Header("�v���C���[�̃I�u�W�F�N�g")]
    [SerializeField] private GameObject _playerObject;

    [Header("�G�̃v���n�u")]
    [SerializeField] private GameObject _enemyPrefab;

    private bool isVertical = false;
    //�c�ŕ������邩�ǂ���
    // Start is called before the first frame update
    void Start()
    {
        //�񎟌��z��ɒl������
        _layer = new Layer2D(_width, _height);

        //��惊�X�g�쐬
        _divList = new List<DgDivision>();

        //���ׂĂ�ǂɂ���
        _layer.Fill(_chipWall);

        //�ŏ��̋������
        CreateDivision(0, 0, _width - 1, _height - 1);

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

        if (parent.Outer.Left + _minRoom + _outerMergin >= parent.Outer.Right - _minRoom - _outerMergin || parent.Outer.Top + _minRoom + _outerMergin >= parent.Outer.Bottom - _minRoom -_outerMergin) 
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
                _layer.SetData(j, i, 1);
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
            _layer.SetData(x, y1, 1);
        }
        for (int x = b.Room.Left; x > b.Outer.Left; x--) 
        {
            _layer.SetData(x, y2, 1);
        }
        for (int y = Mathf.Min(y1, y2), end = Mathf.Max(y1, y2); y <= end; y++) 
        {
            _layer.SetData(a.Outer.Right, y, 1);
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
            _layer.SetData(x1, y, 1);
        }
        for (int y = b.Room.Top; y > b.Outer.Top; y--) 
        {
            _layer.SetData(x2, y, 1);
        }
        for (int x = Mathf.Min(x1, x2), end = Mathf.Max(x1, x2); x <= end; x++) 
        {
            _layer.SetData(x, a.Outer.Bottom, 1);
        }
    }

    /// <summary>
    /// �^�C���`�b�v���Z�b�g����
    /// </summary>
    private void SetTile() 
    {
        for (int y = 0; y < _layer.Height; ++y) 
        {
            for (int x = 0; x < _layer.Width; ++x) 
            {
                //XY�̒l�����Ĕz��̒��g�̐����������Ă���
                int a = _layer.GetMapData(x, y);

                //�z��̒��g�̐����ɂ���ă}�b�v�`�b�v�����Ă�B������㩂Ȃǂ̃M�~�b�N�𐶐����鏈����ǉ����Ă���������
                if (a == 1)
                {
                    //_groundTileMap.SetTile(new Vector3Int(x, y, 0), _groundTileChip);
                    var v = Instantiate(_kusanyaObject, new Vector3(x, -1 * y, 0), _kusanyaObject.transform.rotation);

                }
                else if(a == 0) 
                {
                    //_wallTileMap.SetTile(new Vector3Int(x, y, 0), _wallTileChip);
                    var t = Instantiate(_iwaObject, new Vector3(x, -1 * y, 0), _iwaObject.transform.rotation);
                }
            }
        }

        ///�e�X�g�p
        //�K�i�������_���ɐ���
        Generatesomething(_stairSet);
        //�v���C���[�̐���
        Generatesomething(_playerObject);
        //�G�̐���
        Generatesomething(_enemyPrefab);
    }


    /// <summary>
    /// �I�u�W�F�N�g������ƃ_���W�������Ƀ����_���ɐ�������
    /// </summary>
    /// <param name="Iobject"></param>
    public void Generatesomething(GameObject Iobject) 
    {
        //�����_���ȋ���I������
        int suffix = Random.Range(0, _divList.Count);

        //���̒��̃����_���ȏꏊ��I������
        int x = Random.Range(_divList[suffix].Room.Left, _divList[suffix].Room.Right);
        int y = Random.Range(_divList[suffix].Room.Top, _divList[suffix].Room.Bottom);

        var Object = Instantiate(Iobject, new Vector3(x, -1 * y, 0), _stairSet.transform.rotation);
    }

    /// <summary>
    /// 㩂𐶐�����
    /// </summary>
    private void TrapGenerator() 
    {

    }
}
