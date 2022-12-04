using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaiksutoraCs: MonoBehaviour
{
    //�}�b�v�̃f�[�^���擾
    int _mapY = 10;
    int _mapX = 10;
    int _movePoint = 0;
    int[] _posX = { 1, 0, -1, 0 };
    int[] _posY = { 0, 1, 0, -1 };
    //�v���C���[�̃f�[�^���쐬
    EnemyData _playerData;
    //�S�[���̃f�[�^�����쐬
    EnemyData _enemyData;
    EnemyData _result;
    DgDivision _mapData;
    DgGenerator _dgGenerator;
    GameManager _gameManager;

    int[,] mapArray;
    int[,] asiato;
    /// <summary>
    /// �_�C�N�X�g�����J�n����֐�
    /// </summary>
    /// <param name="x">Enemy��X���W</param>
    /// <param name="y">Enemy��Y���W</param>
    /// <returns></returns>
    public EnemyData Dijkstra(int x, int y)
    {
        _result = new EnemyData(0, 0, 1000);
        _mapData = DgGenerator.Instance.GetDivList(x, y * -1);
        Init();
        _playerData = new EnemyData(_gameManager.PlayerX - _mapData.Room.Left, _gameManager.PlayerY - _mapData.Room.Top, 100);
        _enemyData = new EnemyData(x - _mapData.Room.Left, y - _mapData.Room.Top, 0);
        for (int i = 0; i < _mapY; i++)
        {
            for (int j = 0; j < _mapX; j++)
            {
                asiato[i, j] = 99;

            }
        }

        _movePoint = 0;
        UpTansaku(_playerData.PlayerY);
        _movePoint = 0;
        DownTansaku(_playerData.PlayerY);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var k = _enemyData.PlayerX+ i - 1;
                var l = _enemyData.PlayerY + j - 1;
                if (k < 0 || l < 0 || k > asiato.Length / _mapY - 1 || l > asiato.Length / _mapX - 1) 
                {
                    continue;
                }
                var a =  new EnemyData(k + _mapData.Room.Left, l + _mapData.Room.Top, asiato[l,k]);
                if (_result.MovePoint > a.MovePoint && _dgGenerator.Layer.GetMapData(k + _mapData.Room.Left, l + _mapData.Room.Top) != MapNum.EnemyNum) 
                {
                    Debug.Log($"����ւ�");
                    _result = a;
                }
            }
        }

        return _result;
    }

    /// <summary>
    /// ������
    /// </summary>
    void Init()
    {
        _gameManager = GameManager.Instance;
        _dgGenerator = DgGenerator.Instance;
        _mapX = _mapData.Room.Right - _mapData.Room.Left + 2;
        _mapY = _mapData.Room.Bottom - _mapData.Room.Top + 2;
        mapArray = new int[_mapY, _mapX];
        asiato = new int[_mapY, _mapX];
    }

    void UpTansaku(int y)
    {
        //�v���C���[��Y���W��ۑ�
        var movePlayerY = y;
        //����Ɉړ��p�̕ϐ��쐬
        var smovePoint = _movePoint;

        //���[�܂ő��Ղ�����
        for (int i = _playerData.PlayerX; i >= 0; i--)
        {
            if (movePlayerY < 0 || asiato.Length / _mapX - 1 < movePlayerY)
            {

            }
            //���Ղ�����
            else if (asiato[movePlayerY, i] > smovePoint)
            {
                Debug.Log("asdasd");
                asiato[movePlayerY, i] = smovePoint;
            }

            //���������
            for (int j = 0; j < _posX.Length; j++)
            {
                //�̈�O���w�肵������������߂�
                if (i + _posX[j] >= _mapX || i + _posX[j] < 0 || movePlayerY + _posY[j] >= _mapY || movePlayerY + _posY[j] < 0)
                {

                }
                //���������̐������ړ������|�C���g���傫�������ꍇ�㏑������
                else if (asiato[movePlayerY + _posY[j], i + _posX[j]] > smovePoint + 1)
                {
                    asiato[movePlayerY + _posY[j], i + _posX[j]] = smovePoint + 1;
                }
            }

            smovePoint++;

        }

        smovePoint = _movePoint;
        //�E�[�܂ő��Ղ�����
        for (int i = _playerData.PlayerX; i < _mapX; i++)
        {
            if (movePlayerY < 0 || asiato.Length / _mapX - 1 < movePlayerY)
            {

            }
            //���Ղ�����
            else if (asiato[movePlayerY, i] > smovePoint)
            {
                asiato[movePlayerY, i] = smovePoint;
            }

            //���������
            for (int j = 0; j < _posX.Length; j++)
            {
                //�̈�O���w�肵������������߂�
                if (i + _posX[j] >= _mapX || i + _posX[j] < 0 || movePlayerY + _posY[j] >= _mapY || movePlayerY + _posY[j] < 0)
                {

                }
                //���������̐������ړ������|�C���g���傫�������ꍇ�㏑������
                else if (asiato[movePlayerY + _posY[j], i + _posX[j]] > smovePoint + 1)
                {
                    asiato[movePlayerY + _posY[j], i + _posX[j]] = smovePoint + 1;
                }
            }

            smovePoint++;
        }

        _movePoint++;
        movePlayerY++;

        //������ɂ����T�����ł��Ȃ��ꍇ�ċA�����𔲂���
        if (movePlayerY - 1 > -1)
        {
            y--;
            UpTansaku(y);
        }
    }

    void DownTansaku(int y)
    {
        //�v���C���[��Y���W��ۑ�
        var movePlayerY = y;
        //����Ɉړ��p�̕ϐ��쐬
        var smovePoint = _movePoint;

        //���[�܂ő��Ղ�����
        for (int i = _playerData.PlayerX - 1; i >= 0; i--)
        {
            if (movePlayerY < 0 || asiato.Length / _mapX - 1 < movePlayerY)
            {

            }
            //���Ղ�����
            else if (asiato[movePlayerY, i] > smovePoint)
            {
                //Debug.Log($"{asiato.Length / _mapY - 1}da{movePlayerY}�� {asiato.Length / _mapX}la{i}");
                asiato[movePlayerY, i] = smovePoint;
            }

            //���������
            for (int j = 0; j < _posX.Length; j++)
            {
                //�̈�O���w�肵������������߂�
                if (i + _posX[j] >= _mapX || i + _posX[j] < 0 || movePlayerY + _posY[j] >= _mapY || movePlayerY + _posY[j] < 0)
                {

                }
                //���������̐������ړ������|�C���g���傫�������ꍇ�㏑������
                else if (asiato[movePlayerY + _posY[j], i + _posX[j]] > smovePoint + 1)
                {
                    asiato[movePlayerY + _posY[j], i + _posX[j]] = smovePoint + 1;
                    //Console.WriteLine(movePoint);
                }
            }

            smovePoint++;

        }

        smovePoint = _movePoint;
        //�E�[�܂ő��Ղ�����
        for (int i = _playerData.PlayerX; i < _mapX; i++)
        {
            if (movePlayerY < 0 || i < 0 || asiato.Length / _mapX  - 1 < movePlayerY || asiato.Length / _mapY -1 < i)
            {

            }
            //���Ղ�����
            else if (asiato[movePlayerY, i] > smovePoint)
            {
                asiato[movePlayerY, i] = smovePoint;
            }

            //���������
            for (int j = 0; j < _posX.Length; j++)
            {
                //�̈�O���w�肵������������߂�
                if (i + _posX[j] >= _mapX || i + _posX[j] < 0 || movePlayerY + _posY[j] >= _mapY || movePlayerY + _posY[j] < 0) ;
                //���������̐������ړ������|�C���g���傫�������ꍇ�㏑������
                else if (asiato[movePlayerY + _posY[j], i + _posX[j]] > smovePoint + 1)
                {
                    asiato[movePlayerY + _posY[j], i + _posX[j]] = smovePoint + 1;
                    //Console.WriteLine(movePoint);
                }
            }

            smovePoint++;
        }

        _movePoint++;
        movePlayerY++;

        //�������ɂ����T�����ł��Ȃ��ꍇ�ċA�����𔲂���
        if (movePlayerY <= _mapY)
        {
            y++;
            DownTansaku(y);
        }
    }

}

//���ʂ�\������  
public struct EnemyData
{
    public int PlayerX;
    public int PlayerY;
    public int MovePoint;
    public EnemyData(int x, int y, int move)
    {
        PlayerX = x;
        PlayerY = y;
        MovePoint = move;
    }
}


