using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Tooltip("GameManeger�̃C���X�^���X")]
    private GameManager _gameManager;

    [Tooltip("���쒆���ǂ���")]
    private bool _isMoving;

    [Tooltip("�v���C���[������������")]
    private Vector3 _playerDirection;
    public Vector3 PlayerDirection => _playerDirection;

    [Tooltip("�v���C���[�����Ɉړ�����ꏊ")]
    private Vector3 _nextPosition;



    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    /// <summary>
    /// �ړ��̓��͏���
    /// </summary>
    public void MoveInputKey()
    {
        //_countTime = 0;
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        //�V�t�g�������Ă���Ƃ��͈ړ��ł��Ȃ�����
        if (!Input.GetButton("Lock"))
        {
            _isMoving = judgeMove((int)x, (int)y);
            //�ړ���ɏ�Q�����Ȃ����ǂ���
            if (_isMoving && (x != 0 || y != 0))
            {
                //�v���C���[�̕�����ۑ�����
                _playerDirection = new Vector3(x, y, 0);

                //�Q�[���}�l�[�W���[�Ńv���C���[���ǂ̕����ɂ��邩���肷��i
                // _gameManagerIns.SetPlayerRoomNum((int)(transform.position.x + x)�@, (int)(transform.position.y + y) * -1);

                //�A�C�e���������ɗ����Ă��Ȃ����ǂ���
                ItemJudge(x, y);

                //�����Ɋ��G�����������ړ��ł��Ȃ��Ƃ���������ǉ�����
                _nextPosition = transform.position + new Vector3(x, y, 0);

                //�Q�[���}�l�[�W���[�Ƀv���C���[�̏ꏊ��n��
                _gameManager.SetPlayerPosition((int)x, (int)y * -1);

                //�ړ�����
                transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);

                //�s�����I������̂Ń^�[���t�F�[�Y��ς���
                _gameManager.TurnType = GameManager.TurnManager.Enemy;
                Debug.Log("�����̍s�����I���܂���");
            }
            else
            {

            }
        }
        else
        {
            if (x != 0 || y != 0)
            {
                _playerDirection = new Vector2(x, y);
                Debug.Log($"{_playerDirection}�v���C���[�̕��������߂܂���");
            }
        }



        _isMoving = false;
    }

    /// <summary>
    /// �_���W�����̋��ɃA�N�Z�X���Ď��̏ꏊ���ړ��\�����ׂ�
    /// </summary>
    /// <param name="x">�ړ����邘���W</param>
    /// <param name="y">�ړ�����y���W</param>
    /// <returns></returns>
    private bool judgeMove(int x, int y)
    {
        //�}�b�v�f�[�^�ɃA�N�Z�X
        int value = DgGenerator.Instance.Layer.GetMapData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1);

        // Debug.Log(value);

        //0�͕ǁA1�͓�
        if (value == 0)
        {
            return false;

        }
        else if (value == 1 || value == 3)
        {
            return true;
        }

        return false;

    }

    /// <summary>
    /// �����ɃA�C�e�����Ȃ����ǂ������肷��
    /// </summary>
    public void ItemJudge(float x, float y)
    {
        foreach (var i in _gameManager.ItemObjList)
        {
            //�v���C���[�ƃA�C�e���̍��W���d�Ȃ��Ă�����
            if (i.transform.position == this.gameObject.transform.position + new Vector3(x, y, 0))
            {
                var objCs = i.GetComponent<ItemObjectScript>();
                var PlayerStatus = gameObject.GetComponent<PlayerStatus>();

                //�A�C�e�����C���x���g���ɃZ�b�g����
                PlayerStatus.SetItem(objCs.ItemInfomation);
                //�����ɒu�����A�C�e�������X�g����Remove
                _gameManager.RemoveItemObjList(i);
                //�����Ă���A�C�e�����폜
                objCs.DestroyObj();

                return;

            }
        }
    }
}
