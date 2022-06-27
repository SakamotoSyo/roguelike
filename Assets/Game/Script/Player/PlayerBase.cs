using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBase : MonoBehaviour, IDamageble
{
    public ChType GetType => ChType.Player;

    [SerializeField, Header("�ő�HP")] private int _maxHp;
    [SerializeField, Header("HP")] private float _playerHp;
    [SerializeField, Header("�U����")] private float _power;
    [SerializeField, Header("�s���̉�")] private int _actionNum;
    [SerializeField, Header("�A�C�e���̏������X�g")] private List<Item> _playerItemList;

    [SerializeField, Header("ItemDateBase")] private ItemDataBase _itemDataBase;

    [Tooltip("�Q�[���}�l�[�W���[")]
    private GameManager _gameManagerIns;

    [Tooltip("�v���C���[�����Ɉړ�����ꏊ")]
    private Vector3 _nextPosition;

    [Tooltip("���쒆���ǂ���")]
    private bool _isMoving;

  
    //Test�p
    private float _waitTime = 0.1f;
    private float _countTime = 0;

    private void Start()
    {
        _gameManagerIns = GameManager.Instance;
        _gameManagerIns.PlayerBase = this.gameObject;
        _gameManagerIns.PlayerX = (int)transform.position.x;
        _gameManagerIns.PlayerY = -1 * (int)transform.position.y;

    }
    private void Update()
    {
        if (_gameManagerIns.TurnType == GameManager.TurnManager.Player && _waitTime < _countTime)
        {
            InputKey();
        }
        _countTime += Time.deltaTime;
    }

    /// <summary>
    /// �ړ��̓��͏���
    /// </summary>
    private void InputKey()
    {
        _countTime = 0;
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        //�V�t�g�������Ă���Ƃ��͈ړ��ł��Ȃ�����
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            _isMoving = judgeMove((int)x, (int)y);
            //�ړ���ɏ�Q�����Ȃ����ǂ���
            if (_isMoving && (x != 0 || y != 0))
            {
                //�����Ɋ��G�����������ړ��ł��Ȃ��Ƃ���������ǉ�����
                _nextPosition = transform.position + new Vector3(x, y, 0);

                //�Q�[���}�l�[�W���[�Ƀv���C���[�̏ꏊ��n��
                GameManager.Instance.PlayerX += (int)x;
                GameManager.Instance.PlayerY += (int)y * -1;

                transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);

                GameManager.Instance.TurnType = GameManager.TurnManager.Enemy;
            }
            else
            {

            }
        }
        else
        {
            Debug.Log("Shift");
        }

        _isMoving = false;
    }

    /// <summary>
    /// �_���W�����̋��ɃA�N�Z�X���Ď��̏ꏊ���ړ��\�����ׂ�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool judgeMove(int x, int y)
    {
        //�}�b�v�f�[�^�ɃA�N�Z�X
        int value = DgGenerator.Instance.Layer.GetMapData(_gameManagerIns.PlayerX + x, _gameManagerIns.PlayerY + y * -1);

        // Debug.Log(value);
        //0�͕ǁA1�͓�
        if (value == 0)
        {
            return false;

        }
        else if (value == 1)
        { 
           return true;
        }

        return false;

    }

    /// <summary>
    /// �_���[�W���󂯂鏈��
    /// </summary>
    /// <param name="damage">�H�炤�_���[�W</param>
    public void AddDamage(float damage)
    {
        Debug.Log($"�v���C���[��{damage}�̃_���[�W���󂯂�");
        _playerHp -= damage;
    }

    /// <summary>
    /// �A�C�e��������A�C�e�����擾����
    /// </summary>
    /// <param name="searchName">�A�C�e����</param>
    /// <returns></returns>
    public Item GetItem(string searchName) 
    {
        return _itemDataBase.GetItemLists().Find(itemName => itemName.GetItemName == searchName);
    }
}