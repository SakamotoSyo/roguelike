using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDirection
{
    /// <summary>
    /// 向いている方向を取得する
    /// </summary>
    /// <returns></returns>
    public Vector2 GetDirection();

}
