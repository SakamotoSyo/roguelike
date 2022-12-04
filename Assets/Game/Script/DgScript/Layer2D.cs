using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer2D
{
    [Tooltip("マップの幅")]
    int _width;
    [Tooltip("マップの高さ")]
    int _height;
    [Tooltip("領域外を指定した時の値")]
    int _outOfRange = -1;
    [Tooltip("マップデータ")]
    int[] _values;
    [Tooltip("カプセル化")]
    public int Width => _width;
    public int Height => _height;


    //コンストラクタ
    public Layer2D(int width = 0, int height = 0) 
    {
        if (width > 0 && height > 0) 
        {
            Create(width, height);
        }
    }

    public void Create(int width, int height) 
    {
        _width = width;
        _height = height;
        _values = new int[width * height];
    }

    /// <summary>
    /// 領域外かどうかチェック
    /// </summary>
    /// <param name="x">X座標</param>
    /// <param name="y">Y座標</param>
    /// <returns></returns>
    public bool IsOutOfRange(int x, int y) 
    {
        if (x < 0 || x >= Width) 
        {
            return true;
        }
        if (y < 0 || y >= Height) 
        {
            return true;
        }

        //領域内
        return false;
    }

    /// <summary>
    /// 指定した座標のマップデータリストの中身で返す
    /// </summary>
    /// <param name="x">x座標</param>
    /// <param name="y">ｙ座標</param>
    /// <returns></returns>
    public int GetMapData(int x, int y) 
    {
        if (IsOutOfRange(x, y)) 
        {
            return _outOfRange;
        }

        return _values[y * Width + x];
    }

    /// <summary>
    /// マップデータのリストの中に値を入れる
    /// </summary>
    /// <param name="x">x座標</param>
    /// <param name="y">ｙ座標</param>
    /// <param name="v">埋める値</param>
    public void SetData(int x, int y, int v) 
    {
        if (IsOutOfRange(x, y)) 
        {
            return;
        }

        _values[y * Width + x] = v;
    }

    /// <summary>
    /// マップデータのリストの中すべてに同じ値を入れる
    /// </summary>
    /// <param name="v">入れる値</param>
    public void Fill(int v) 
    {
        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++) 
            {
                SetData(i, j, v);
            }
        }
    }


    /// <summary>
    /// 指定した値の中をｖで指定した値で埋める
    /// </summary>
    /// <param name="x">x座標(左上)</param>
    /// <param name="y">y座標(左上)</param>
    /// <param name="w">指定したx座標からwの値分値を入れる</param>
    /// <param name="h">指定したx座標からｙの値分値を入れる</param>
    /// <param name="v">埋める値</param>
    public void FillRect(int x, int y, int w, int h, int v) 
    {
        for (var i = 0; i < h; i++) 
        {
            for (var j = 0; j < w; j++) 
            {
                //順番に値を入れていく
                int a = x + w;
                int b = y + h;

                SetData(a, b, v);
            } 
        }
    }

    //座標をインデックスに変換する
    public int ToIdx(int x, int y) 
    {
        return x + (y * Width);
    }
}
