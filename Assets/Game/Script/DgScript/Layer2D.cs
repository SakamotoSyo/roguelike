using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer2D
{
    [Tooltip("�}�b�v�̕�")]
    int _width;
    [Tooltip("�}�b�v�̍���")]
    int _height;
    [Tooltip("�̈�O���w�肵�����̒l")]
    int _outOfRange = -1;
    [Tooltip("�}�b�v�f�[�^")]
    int[] _values;
    [Tooltip("�J�v�Z����")]
    public int Width => _width;
    public int Height => _height;


    //�R���X�g���N�^
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
    /// �̈�O���ǂ����`�F�b�N
    /// </summary>
    /// <param name="x">X���W</param>
    /// <param name="y">Y���W</param>
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

        //�̈��
        return false;
    }

    /// <summary>
    /// �w�肵�����W�̃}�b�v�f�[�^���X�g�̒��g�ŕԂ�
    /// </summary>
    /// <param name="x">x���W</param>
    /// <param name="y">�����W</param>
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
    /// �}�b�v�f�[�^�̃��X�g�̒��ɒl������
    /// </summary>
    /// <param name="x">x���W</param>
    /// <param name="y">�����W</param>
    /// <param name="v">���߂�l</param>
    public void SetData(int x, int y, int v) 
    {
        if (IsOutOfRange(x, y)) 
        {
            return;
        }

        _values[y * Width + x] = v;
    }

    /// <summary>
    /// �}�b�v�f�[�^�̃��X�g�̒����ׂĂɓ����l������
    /// </summary>
    /// <param name="v">�����l</param>
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
    /// �w�肵���l�̒������Ŏw�肵���l�Ŗ��߂�
    /// </summary>
    /// <param name="x">x���W(����)</param>
    /// <param name="y">y���W(����)</param>
    /// <param name="w">�w�肵��x���W����w�̒l���l������</param>
    /// <param name="h">�w�肵��x���W���炙�̒l���l������</param>
    /// <param name="v">���߂�l</param>
    public void FillRect(int x, int y, int w, int h, int v) 
    {
        for (var i = 0; i < h; i++) 
        {
            for (var j = 0; j < w; j++) 
            {
                //���Ԃɒl�����Ă���
                int a = x + w;
                int b = y + h;

                SetData(a, b, v);
            } 
        }
    }

    //���W���C���f�b�N�X�ɕϊ�����
    public int ToIdx(int x, int y) 
    {
        return x + (y * Width);
    }
}
