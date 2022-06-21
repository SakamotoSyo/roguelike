using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ダンジョンの区画情報
public class DgDivision
{
    //外周の矩形情報
    public DgRect Outer;

    //区画内に作ったルーム情報
    public DgRect Room;

    //コンストラクタ
    public DgDivision() 
    {
        Outer = new DgRect();
        Room = new DgRect();
    }

    
     //矩形管理
   public class DgRect 
    {
        public int Left = 0;
        public int Top = 0;
        public int Right = 0;
        public int Bottom = 0;


        /// <summary>
        /// 矩形の情報をまとめて設定する
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public void Set(int left, int top, int right, int bottom) 
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
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
            Left = rect.Left;
            Top = rect.Top;
            Right = rect.Right;
            Bottom = rect.Bottom;
        }
    }

}
