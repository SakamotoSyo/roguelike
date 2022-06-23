using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IDamageble
{
    public enum MoveType
    {
        WaitKey,
        Move,
    }

    [SerializeField, Header("�ő�HP")] private int _maxHp;
    [SerializeField, Header("HP")] private float _playerHp;
    [SerializeField, Header("�U����")] private float _power;
    [SerializeField, Header("�s���̉�")] private int _actionNum;

    [Tooltip("�Q�[���}�l�[�W���[")]
    private GameManager _gameManagerIns;

    [Tooltip("�v���C���[�����Ɉړ�����ꏊ")]
    private Vector3 _nextPosition;

    [Tooltip("���쒆���ǂ���")]
    private bool _isMoving;
    //�v���C���[�̃��[�u�^�C�v
    private MoveType _moveType = MoveType.WaitKey;

    //Test�p
    private float _waitTime = 0.1f;
    private float _countTime = 0;

    private void Start()
    {
        _gameManagerIns = GameManager.Instance;
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
            ////�����Ɋ��G�����������ړ��ł��Ȃ��Ƃ���������ǉ�����
            //_nextPosition = transform.position + new Vector3(x, y, 0);
            //_moveType = MoveType.Move;
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
            Debug.Log("����");
            return false;

        }
        else if (value == 1)
        {
            Debug.Log("�Ă΂ꂽ");
            return true;

        }

        return false;

    }

    public void AddDamage(float damage) 
    {
        Debug.Log($"�v���C���[��{damage}�̃_���[�W���󂯂�");
        _playerHp -= damage;
    }
}
