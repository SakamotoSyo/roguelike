using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelDataScript : MonoBehaviour
{
    [Header("���x���A�b�v�f�[�^")]
    [SerializeField] private TextAsset _levelUpTable;

    private Dictionary<int, PlayerStatusData> _statusData = new Dictionary<int, PlayerStatusData>();

    void Awake()
    {
        LoadingLevelData();
    }

    /// <summary>
    /// �V�[���̏��߂Ƀ��x���A�b�v�̃f�[�^��ǂݍ���
    /// </summary>
    private void LoadingLevelData()
    {
        //�e�L�X�g�̓ǂݍ���
        StringReader sr = new StringReader(_levelUpTable.text);
        //�ŏ��̈�s�ڂ̓X�L�b�v
        sr.ReadLine();

        while (true)
        {
            //��s���ǂݍ���
            string line = sr.ReadLine();

            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            string[] parts = line.Split(',');

            int level = int.Parse(parts[0]);
            PlayerStatusData stats = new PlayerStatusData(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
            _statusData.Add(level, stats);
        }
    }

    /// <summary>
    /// ���x�����w�肷��̂��̃��x���ɑ΂���X�e�[�^�X��Ԃ��Ă����
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public PlayerStatusData GetLevelStatus(int level)
    {
        if (_statusData.ContainsKey(level))
        {
            return _statusData[level];
        }

        return default;
    }
}
/// <summary>
/// �v���C���[�̃p�����[�^�[���i�[����\����
/// </summary>
public struct PlayerStatusData
{
    public float Maxhp;
    public float Attack;
    public float Exp;

    public PlayerStatusData(float maxhp = 0f, float attack = 0f, float exp = 0f)
    {
        Maxhp = maxhp;
        Attack = attack;
        Exp = exp;
    }

}