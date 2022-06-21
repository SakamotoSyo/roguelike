using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�_���W�����̋����
public class DgDivision
{
    //�O���̋�`���
    public DgRect Outer;

    //�����ɍ�������[�����
    public DgRect Room;

    //�R���X�g���N�^
    public DgDivision() 
    {
        Outer = new DgRect();
        Room = new DgRect();
    }

    
     //��`�Ǘ�
   public class DgRect 
    {
        public int Left = 0;
        public int Top = 0;
        public int Right = 0;
        public int Bottom = 0;


        /// <summary>
        /// ��`�̏����܂Ƃ߂Đݒ肷��
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
        /// ��`�̕���Ԃ��Ă����
        /// </summary>
        /// <returns></returns>
        public int Width() 
        {
            return Right - Left;
        }

        /// <summary>
        /// ��`�̍�����Ԃ��Ă����
        /// </summary>
        /// <returns></returns>
        public int Height() 
        {
            return Bottom - Top;
        }

        /// <summary>
        /// �ʐς�Ԃ��Ă����
        /// </summary>
        /// <returns></returns>
        public int Measure() 
        {
            return Width() * Height();
        }

        /// <summary>
        /// ��`�̏����R�s�[����
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
