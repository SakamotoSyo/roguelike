using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelDataScript : MonoBehaviour
{
    [Header("レベルアップデータ")]
    [SerializeField] private TextAsset _levelUpTable;

    private Dictionary<int, PlayerStatusData> _statusData = new Dictionary<int, PlayerStatusData>();

    void Awake()
    {
        LoadingLevelData();
    }

    /// <summary>
    /// シーンの初めにレベルアップのデータを読み込む
    /// </summary>
    private void LoadingLevelData()
    {
        //テキストの読み込み
        StringReader sr = new StringReader(_levelUpTable.text);
        //最初の一行目はスキップ
        sr.ReadLine();

        while (true)
        {
            //一行ずつ読み込む
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
    /// レベルを指定するのそのレベルに対するステータスを返してくれる
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
/// プレイヤーのパラメーターを格納する構造体
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