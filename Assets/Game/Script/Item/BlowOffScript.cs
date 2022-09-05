using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlowOffScript : MonoBehaviour
{
    [Header("RigidBody")]
    [SerializeField] Rigidbody2D _rb;

    [Header("飛ばすスピード")]
    [SerializeField] float _speed;

    [Tooltip("PlayerMoveのScript")]
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
            EffectCs.UseItemEffect("吹き飛ばしの杖");
            Destroy(gameObject);
        }
    }

    ///// <summary>
    ///// アイテムを投げるメソッド
    ///// </summary>
    ///// <param name="item"></param>
    //public IEnumerator ThrowItem(Item item)
    //{
    //    //プレイヤーの動いた方向を持ってくる
    //    int x = (int)_playerMove.PlayerDirection.x;
    //    int y = (int)_playerMove.PlayerDirection.y;

    //    if (x == 0 && y == 0)
    //    {
    //        y = -1;
    //    }

    //    //アイテムがが壁の座標まで飛び続ける
    //    while (_dgGenerator.Layer.GetMapData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1) == 1)
    //    {
    //        x += Math.Sign(x);
    //        y += Math.Sign(y);
    //    }

    //    x += Math.Sign(x) * -1;
    //    y += Math.Sign(y) * -1;

    //    //移動する次の場所
    //    Vector3 _nextPosition = new Vector3(_gameManager.PlayerX + x, _gameManager.PlayerY * -1 + y, 0);
    //    //今の場所から目的地までの距離
    //    var _distance_Two = Vector3.Distance(transform.position, _nextPosition);
    //    //配列にアイテムの場所をセットする
    //    _dgGenerator.Layer.SetData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1, 3);
    //    Debug.Log(_dgGenerator.Layer.GetMapData(_gameManager.PlayerX + x, _gameManager.PlayerY + y * -1));
    //    //移動処理
    //    StartCoroutine(ItemThrowMove(Item, _nextPosition, _distance_Two));

    //    yield return new WaitForSeconds(0.08f);
    //    //アイテムのオブジェクトをゲームマネージャーに渡す
    //    _gameManager.SetItemObjList(Item);

    //    _playerStatusCs.RemoveItem(item);

    //    ResetMenu();
    //}

    ///// <summary>
    ///// アイテムを指定の場所まで移動させる
    ///// </summary>
    ///// <param name="Item"></param>
    ///// <param name="_nextPosition"></param>
    //private IEnumerator ItemThrowMove(GameObject Item, Vector3 _nextPosition, float _distance_Two)
    //{
    //    while (Item.transform.position != _nextPosition)
    //    {
    //        //ここがスピード
    //        _testPosition += 0.05f;
    //        //移動処理
    //        Item.transform.position = Vector3.Lerp(_gameManager.PlayerObj.transform.position, _nextPosition, _testPosition);
    //        yield return new WaitForSeconds(0.01f);
    //    }

    //    _testPosition = 0;
    //}
}
