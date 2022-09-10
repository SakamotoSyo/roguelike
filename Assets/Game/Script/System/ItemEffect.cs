using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemEffect : MonoBehaviour
{
    [SerializeField] PlayerMove _playerMove;

    [Header("吹き飛ぶスピード")]
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
    /// アイテムの効果を判定する
    /// </summary>
    public void UseItemEffect(string ItemName)
    {
        switch (ItemName)
        {
            case "吹き飛ばしの杖":
                BlowAwayEffect();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 吹き飛ばす処理をする関数
    /// </summary>
    void BlowAwayEffect()
    {
        int PosX = (int)transform.position.x;
        int PosY = (int)transform.position.y * -1;

        int dirX = (int)_playerMove.PlayerDirection.x;
        int dirY = (int)_playerMove.PlayerDirection.y;

        //プレイヤーがまだ動いていないとき
        if (dirX == 0 && dirY == 0)
        {
            dirY = -1;
        }

        //アイテムがが壁の座標まで飛び続ける
        while (_generator.Layer.GetMapData(PosX + dirX, PosY + dirY * -1) == 1)
        {
            dirX += Math.Sign(dirX);
            dirY += Math.Sign(dirY);
        }

        //このままだと壁に埋まってしまうので一マスもどる
        dirX += Math.Sign(dirX) * -1;
        dirY += Math.Sign(dirY) * -1;

        Vector2 MyPos = new Vector2(PosX, PosY * -1);
        //移動する次の場所
        Vector3 _nextPosition = new Vector3(PosX + dirX, PosY * -1 + dirY, 0);
        //移動処理
        StartCoroutine(BlowAwayMove(MyPos, _nextPosition));
    }

    IEnumerator BlowAwayMove(Vector2 MyPos, Vector3 _nextPosition)
    {
        //吹っ飛ぶ前の座標は道に
        _generator.Layer.SetData((int)transform.position.x, (int)transform.position.y * -1, MapNum.LoadNum);
        float posKeep = 0;
        while (transform.position != _nextPosition)
        {
            Debug.Log("吹き飛ばした");
            //ここがスピード
            posKeep += _blowAwaySpeed;
            //移動処理
            transform.position = Vector3.Lerp(MyPos, _nextPosition, posKeep);
            yield return new WaitForSeconds(0.01f);
        }
        //敵の座標を移動させる
        _generator.Layer.SetData((int)transform.position.x, (int)transform.position.y * -1, MapNum.EnemyNum);
        _gameManager.TurnType = GameManager.TurnManager.Enemy;
    }
}
