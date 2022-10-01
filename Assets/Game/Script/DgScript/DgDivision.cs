using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ダンジョンの区画情報
public class DgDivision
{
    /// <summary> 外周の矩形情報</summary>
    public DgRect Outer => _outer;
    DgRect _outer;

    /// <summary>区画内に作ったルーム情報</summary>
    public DgRect Room => _room;
    DgRect _room;

    /// <summary> コンストラクタ</summary>
    public DgDivision()
    {
        _outer = new DgRect();
        _room = new DgRect();
    }

    //矩形管理
    public class DgRect
    {
        public int Left => _left;
        public int Top => _top;
        public int Right => _right;
        public int Bottom => _bottom;

        int _left = 0;
        int _right = 0;
        int _top = 0;
        int _bottom = 0;

        /// <summary>
        /// 矩形の情報をまとめて設定する
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public void Set(int left, int top, int right, int bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        /// <summary>
        /// 値をセットする
        /// </summary>
        /// <param name="num"></param>
        public void SetBottom(int num)
        {
            _bottom = num;
        }

        /// <summary>
        /// 値をセットする
        /// </summary>
        /// <param name="num"></param>
        public void SetRight(int num)
        {
            _right = num;
        }

        /// <summary>
        /// 矩形の幅を返してくれる
        /// </summary>
        /// <returns></returns>
        public int Width()
        {
            return Right - Left;
        }

        /// <summary>
        /// 矩形の高さを返してくれる
        /// </summary>
        /// <returns></returns>
        public int Height()
        {
            return Bottom - Top;
        }

        /// <summary>
        /// 面積を返してくれる
        /// </summary>
        /// <returns></returns>
        public int Measure()
        {
            return Width() * Height();
        }

        /// <summary>
        /// 矩形の情報をコピーする
        /// </summary>
        /// <param name="rect"></param>
        public void Copy(DgRect rect)
        {
            _left = rect.Left;
            _top = rect.Top;
            _right = rect.Right;
            _bottom = rect.Bottom;
        }
    }

}
