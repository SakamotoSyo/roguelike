using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBase : MonoBehaviour
{
   

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
        _gameManagerIns.SetPlayerObj(this.gameObject);

    }
    private void Update()
    {
        if (_gameManagerIns.TurnType == GameManager.TurnManager.Player && _waitTime < _countTime)
        {
            MoveInputKey();
        }
        _countTime += Time.deltaTime;
    }

    /// <summary>
    /// �ړ��̓��͏���
    /// </summary>
    private void MoveInputKey()
    {
        _countTime = 0;
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        //�V�t�g�������Ă���Ƃ��͈ړ��ł��Ȃ�����
        if (!Input.GetButton("Lock"))
        {
            _isMoving = judgeMove((int)x, (int)y);
            //�ړ���ɏ�Q�����Ȃ����ǂ���
            if (_isMoving && (x != 0 || y != 0))
            {
                //�����Ɋ��G�����������ړ��ł��Ȃ��Ƃ���������ǉ�����
                _nextPosition = transform.position + new Vector3(x, y, 0);

                //�Q�[���}�l�[�W���[�Ƀv���C���[�̏ꏊ��n��
                _gameManagerIns.SetPlayerPosition((int)x, (int)y * -1);

                //�ړ�����
                transform.position = Vector3.Lerp(transform.position, _nextPosition, 1);

                //�s�����I������̂Ń^�[���t�F�[�Y��ς���
                _gameManagerIns.TurnType = GameManager.TurnManager.Enemy;
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

    private void UIInputKey() 
    {
        if (Input.GetButtonDown("Cancel")) 
        {
            
        }

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

   
}