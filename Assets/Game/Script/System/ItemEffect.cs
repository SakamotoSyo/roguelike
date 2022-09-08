using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemEffect : MonoBehaviour
{
    [SerializeField] PlayerMove _playerMove;

    [Header("������ԃX�s�[�h")]
    float _blowAwaySpeed = 0.05f;

    DgGenerator _generator;

    GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _generator = DgGenerator.Instance;
        _gameManager = GameManager.Instance;
        if (_playerMove == null) _playerMove = _gameManager.PlayerObj.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// �A�C�e���̌��ʂ𔻒肷��
    /// </summary>
    public void UseItemEffect(string ItemName)
    {
        switch (ItemName)
        {
            case "������΂��̏�":
                BlowAwayEffect();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ������΂�����������֐�
    /// </summary>
    void BlowAwayEffect()
    {
        int PosX = (int)transform.position.x;
        int PosY = (int)transform.position.y * -1;

        int dirX = (int)_playerMove.PlayerDirection.x;
        int dirY = (int)_playerMove.PlayerDirection.y;

        //�v���C���[���܂������Ă��Ȃ��Ƃ�
        if (dirX == 0 && dirY == 0)
        {
            dirY = -1;
        }

        //�A�C�e�������ǂ̍��W�܂Ŕ�ё�����
        while (_generator.Layer.GetMapData(PosX + dirX, PosY + dirY * -1) == 1)
        {
            dirX += Math.Sign(dirX);
            dirY += Math.Sign(dirY);
        }

        //���̂܂܂��ƕǂɖ��܂��Ă��܂��̂ň�}�X���ǂ�
        dirX += Math.Sign(dirX) * -1;
        dirY += Math.Sign(dirY) * -1;

        Vector2 MyPos = new Vector2(PosX, PosY * -1);
        //�ړ����鎟�̏ꏊ
        Vector3 _nextPosition = new Vector3(PosX + dirX, PosY * -1 + dirY, 0);
        //�ړ�����
        StartCoroutine(BlowAwayMove(MyPos, _nextPosition));
    }

    IEnumerator BlowAwayMove(Vector2 MyPos, Vector3 _nextPosition)
    {
        //������ԑO�̍��W�͓���
        _generator.Layer.SetData((int)transform.position.x, (int)transform.position.y * -1, MapNum.LoadNum);
        float posKeep = 0;
        while (transform.position != _nextPosition)
        {
            Debug.Log("������΂���");
            //�������X�s�[�h
            posKeep += _blowAwaySpeed;
            //�ړ�����
            transform.position = Vector3.Lerp(MyPos, _nextPosition, posKeep);
            yield return new WaitForSeconds(0.01f);
        }
        //�G�̍��W���ړ�������
        _generator.Layer.SetData((int)transform.position.x, (int)transform.position.y * -1, MapNum.EnemyNum);
        _gameManager.TurnType = GameManager.TurnManager.Enemy;
    }
}
