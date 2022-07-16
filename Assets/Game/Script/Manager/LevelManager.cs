using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
    [Header("���x���A�b�v�f�[�^")]
    [SerializeField] private TextAsset _levelUpTable;

    private Dictionary<int, PlayerStatsData> _statusData;

    private void Awake()
    {
        LoadingLevelData();
    }

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

            string[] parts = line.Split(' ');

            int level = int.Parse(parts[0]);
            PlayerStatsData stats = new PlayerStatsData(level, float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
            _statusData.Add(level, stats);
        }
    }

    public PlayerStatsData GetLevelStatus(int level) 
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
public struct PlayerStatsData
{
    public int Level;
    public float Maxhp;
    public float Attack;
    public float Exp;

    public PlayerStatsData(int level, float maxhp,float attack, float exp)
    {
        this.Level = level;
        this.Maxhp = maxhp;
        this.Attack = attack;
        this.Exp = exp;
    }
}