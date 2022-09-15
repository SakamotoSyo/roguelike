using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaiksutoraCs: MonoBehaviour
{
    //�}�b�v�̃f�[�^���擾
    int mapY = 10;
    int mapX = 10;
    DgDivision mapData;

    int y = 0;
    int[] PosX = { 1, 0, -1, 0 };
    int[] PosY = { 0, 1, 0, -1 };
    //�v���C���[�̃f�[�^���쐬
    EnemyData _playerData;
    //�S�[���̃f�[�^�����쐬
    EnemyData _enemyData;

    EnemyData _result;
    int movePoint = 0;
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
        mapData = DgGenerator.Instance.GetDivList(x, y * -1);
        //Enemy�̍��W������
        Debug.Log($"�}�b�v�f�[�^Top{mapData.Room.Top}");
        Init();
        _playerData = new EnemyData(_gameManager.PlayerX - mapData.Room.Left, _gameManager.PlayerY - mapData.Room.Top, 100);
        _enemyData = new EnemyData(x - mapData.Room.Left, y - mapData.Room.Top, 0);
        for (int i = 0; i < mapY; i++)
        {
            for (int j = 0; j < mapX; j++)
            {
                asiato[i, j] = 99;

            }
        }

        movePoint = 0;
        UpTansaku(_playerData.PlayerY);
        movePoint = 0;
        DownTansaku(_playerData.PlayerY);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var k = _enemyData.PlayerX+ i - 1;
                var l = _enemyData.PlayerY + j - 1;
                if (k < 0 || l < 0 || k > asiato.Length / mapY - 1 || l > asiato.Length / mapX - 1) 
                {
                   // Debug.Log($"{k > asiato.Length / mapY - 1}��{l > asiato.Length / mapX - 1}");
                    continue;
                }
                Debug.Log($"{k}to{asiato.Length / mapY - 1}��{l}to{ asiato.Length / mapX - 1}");
                var a =  new EnemyData(k + mapData.Room.Left, l + mapData.Room.Top, asiato[l,k]);
                if (_result.MovePoint > a.MovePoint) 
                {
                    Debug.Log($"����ւ��[�[�[�[�[");
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
        mapX = mapData.Room.Right - mapData.Room.Left + 2;
        mapY = mapData.Room.Bottom - mapData.Room.Top + 2;
        mapArray = new int[mapY, mapX];
        asiato = new int[mapY, mapX];
    }

    void UpTansaku(int y)
    {
        //�v���C���[��Y���W��ۑ�
        var movePlayerY = y;
        //����Ɉړ��p�̕ϐ��쐬
        var smovePoint = movePoint;

        //���[�܂ő��Ղ�����
        for (int i = _playerData.PlayerX; i >= 0; i--)
        {
            if (movePlayerY < 0 || asiato.Length / mapX - 1 < movePlayerY)
            {

            }
            //���Ղ�����
            else if (asiato[movePlayerY, i] > smovePoint)
            {
                Debug.Log("asdasd");
                asiato[movePlayerY, i] = smovePoint;
            }

            //���������
            for (int j = 0; j < PosX.Length; j++)
            {
                //�̈�O���w�肵������������߂�
                if (i + PosX[j] >= mapX || i + PosX[j] < 0 || movePlayerY + PosY[j] >= mapY || movePlayerY + PosY[j] < 0)
                {
                    //Console.WriteLine("�Ă΂ꂽ");

                }
                //���������̐������ړ������|�C���g���傫�������ꍇ�㏑������
                else if (asiato[movePlayerY + PosY[j], i + PosX[j]] > smovePoint + 1)
                {
                    asiato[movePlayerY + PosY[j], i + PosX[j]] = smovePoint + 1;
                    // Console.WriteLine(movePoint);
                }
            }

            smovePoint++;

        }

        smovePoint = movePoint;
        //�E�[�܂ő��Ղ�����
        for (int i = _playerData.PlayerX; i < mapX; i++)
        {
            if (movePlayerY < 0 || asiato.Length / mapX - 1 < movePlayerY)
            {

            }
            //���Ղ�����
            else if (asiato[movePlayerY, i] > smovePoint)
            {
                asiato[movePlayerY, i] = smovePoint;
            }

            //���������
            for (int j = 0; j < PosX.Length; j++)
            {
                //�̈�O���w�肵������������߂�
                if (i + PosX[j] >= mapX || i + PosX[j] < 0 || movePlayerY + PosY[j] >= mapY || movePlayerY + PosY[j] < 0)
                {
                    //Console.WriteLine("�Ă΂ꂽ");

                }
                //���������̐������ړ������|�C���g���傫�������ꍇ�㏑������
                else if (asiato[movePlayerY + PosY[j], i + PosX[j]] > smovePoint + 1)
                {
                    asiato[movePlayerY + PosY[j], i + PosX[j]] = smovePoint + 1;
                    //Console.WriteLine(movePoint);
                }
            }

            smovePoint++;
        }

        movePoint++;
        movePlayerY++;

        //������ɂ����T�����ł��Ȃ��ꍇ�ċA�����𔲂���
        if (movePlayerY - 1 > -1)
        {
            y--;
            UpTansaku(y);
        }
        else
        {
            //end
        }

    }

    void DownTansaku(int y)
    {
        //�v���C���[��Y���W��ۑ�
        var movePlayerY = y;
        //����Ɉړ��p�̕ϐ��쐬
        var smovePoint = movePoint;

        //���[�܂ő��Ղ�����
        for (int i = _playerData.PlayerX - 1; i >= 0; i--)
        {
            if (movePlayerY < 0 || asiato.Length / mapX - 1 < movePlayerY)
            {

            }
            //���Ղ�����
            else if (asiato[movePlayerY, i] > smovePoint)
            {
                Debug.Log($"{asiato.Length / mapY - 1}da{movePlayerY}�� {asiato.Length / mapX}la{i}");
                asiato[movePlayerY, i] = smovePoint;
            }

            //���������
            for (int j = 0; j < PosX.Length; j++)
            {
                //�̈�O���w�肵������������߂�
                if (i + PosX[j] >= mapX || i + PosX[j] < 0 || movePlayerY + PosY[j] >= mapY || movePlayerY + PosY[j] < 0)
                {
                    //Console.WriteLine("�Ă΂ꂽ");

                }
                //���������̐������ړ������|�C���g���傫�������ꍇ�㏑������
                else if (asiato[movePlayerY + PosY[j], i + PosX[j]] > smovePoint + 1)
                {
                    asiato[movePlayerY + PosY[j], i + PosX[j]] = smovePoint + 1;
                    //Console.WriteLine(movePoint);
                }
            }

            smovePoint++;

        }

        smovePoint = movePoint;
        //�E�[�܂ő��Ղ�����
        for (int i = _playerData.PlayerX; i < mapX; i++)
        {
               // Debug.Log($"{asiato.Length / mapX}da{movePlayerY}�� {asiato.Length / mapX}la{i}");
            if (movePlayerY < 0 || i < 0 || asiato.Length / mapX  - 1 < movePlayerY || asiato.Length / mapY -1 < i)
            {

            }
            //���Ղ�����
            else if (asiato[movePlayerY, i] > smovePoint)
            {
                asiato[movePlayerY, i] = smovePoint;
            }

            //���������
            for (int j = 0; j < PosX.Length; j++)
            {
                //�̈�O���w�肵������������߂�
                if (i + PosX[j] >= mapX || i + PosX[j] < 0 || movePlayerY + PosY[j] >= mapY || movePlayerY + PosY[j] < 0)
                {
                    //Console.WriteLine("�Ă΂ꂽ");

                }
                //���������̐������ړ������|�C���g���傫�������ꍇ�㏑������
                else if (asiato[movePlayerY + PosY[j], i + PosX[j]] > smovePoint + 1)
                {
                    asiato[movePlayerY + PosY[j], i + PosX[j]] = smovePoint + 1;
                    //Console.WriteLine(movePoint);
                }
            }

            smovePoint++;
        }

        movePoint++;
        movePlayerY++;

        //�������ɂ����T�����ł��Ȃ��ꍇ�ċA�����𔲂���
        if (movePlayerY <= mapY)
        {
            y++;
            DownTansaku(y);
        }
        else
        {
            //end
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


