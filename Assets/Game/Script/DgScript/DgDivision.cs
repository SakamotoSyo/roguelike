using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�_���W�����̋����
public class DgDivision
{
    /// <summary> �O���̋�`���</summary>
    public DgRect Outer => _outer;
    DgRect _outer;

    /// <summary>�����ɍ�������[�����</summary>
    public DgRect Room => _room;
    DgRect _room;

    /// <summary> �R���X�g���N�^</summary>
    public DgDivision()
    {
        _outer = new DgRect();
        _room = new DgRect();
    }

    //��`�Ǘ�
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
        /// ��`�̏����܂Ƃ߂Đݒ肷��
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
        /// �l���Z�b�g����
        /// </summary>
        /// <param name="num"></param>
        public void SetBottom(int num)
        {
            _bottom = num;
        }

        /// <summary>
        /// �l���Z�b�g����
        /// </summary>
        /// <param name="num"></param>
        public void SetRight(int num)
        {
            _right = num;
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
            _left = rect.Left;
            _top = rect.Top;
            _right = rect.Right;
            _bottom = rect.Bottom;
        }
    }

}
