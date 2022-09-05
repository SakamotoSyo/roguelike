using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlowOffScript : MonoBehaviour
{
    [Header("RigidBody")]
    [SerializeField] Rigidbody2D _rb;

    [Header("��΂��X�s�[�h")]
    [SerializeField] float _speed;

    [Tooltip("PlayerMove��Script")]
    PlayerMove _playerMove;

    DgGenerator _generator;

    // Start is called before the first frame update
    void Start()
    {
        _playerMove = GameManager.Instance.PlayerObj.GetComponent<PlayerMove>();
        var dir = _playerMove.PlayerDirection;
        _rb.velocity = dir * _speed;
        _generator = DgGenerator.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.TryGetComponent(out ItemEffect EffectCs)) 
        {
            if (collision.gameObject.tag == "Player") return;
            EffectCs.UseItemEffect("������΂��̏�");
            Destroy(gameObject);
        }
    }

    ///// <summary>
    ///// �A�C�e���𓊂��郁�\�b�h
    ///// </summary>
    ///// <param name="item"></param>
    //public IEnumerator ThrowItem(Item item)
    //{
    //    //�v���C���[�̓����������������Ă���
    //    int x = (int)_playerMove.PlayerDirection.x;
    //    int y = (int)_playerMove.PlayerDirection.y;

    //    if (x == 0 && y == 0)
    //    {
    //        y = -1;
    //    }

    //    //�A�C�e�������ǂ̍��W�܂Ŕ�ё�����
    //    while (_dgGenerator.Layer.GetMapData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1) == 1)
    //    {
    //        x += Math.Sign(x);
    //        y += Math.Sign(y);
    //    }

    //    x += Math.Sign(x) * -1;
    //    y += Math.Sign(y) * -1;

    //    //�ړ����鎟�̏ꏊ
    //    Vector3 _nextPosition = new Vector3(_gameManager.PlayerX + x, _gameManager.PlayerY * -1 + y, 0);
    //    //���̏ꏊ����ړI�n�܂ł̋���
    //    var _distance_Two = Vector3.Distance(transform.position, _nextPosition);
    //    //�z��ɃA�C�e���̏ꏊ���Z�b�g����
    //    _dgGenerator.Layer.SetData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1, 3);
    //    Debug.Log(_dgGenerator.Layer.GetMapData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1));
    //    //�ړ�����
    //    StartCoroutine(ItemThrowMove(Item, _nextPosition, _distance_Two));

    //    yield return new WaitForSeconds(0.08f);
    //    //�A�C�e���̃I�u�W�F�N�g���Q�[���}�l�[�W���[�ɓn��
    //    _gameManager.SetItemObjList(Item);

    //    _playerStatusCs.RemoveItem(item);

    //    ResetMenu();
    //}

    ///// <summary>
    ///// �A�C�e�����w��̏ꏊ�܂ňړ�������
    ///// </summary>
    ///// <param name="Item"></param>
    ///// <param name="_nextPosition"></param>
    //private IEnumerator ItemThrowMove(GameObject Item, Vector3 _nextPosition, float _distance_Two)
    //{
    //    while (Item.transform.position != _nextPosition)
    //    {
    //        //�������X�s�[�h
    //        _testPosition += 0.05f;
    //        //�ړ�����
    //        Item.transform.position = Vector3.Lerp(_gameManager.PlayerObj.transform.position, _nextPosition, _testPosition);
    //        yield return new WaitForSeconds(0.01f);
    //    }

    //    _testPosition = 0;
    //}
}
