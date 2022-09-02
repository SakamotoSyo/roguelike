using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MisssionManager : MonoBehaviour
{
    [Header("ミッションを格納するリスト")]
    [SerializeField] List<MissionData> _missionList = new List<MissionData>();
    [Header("ミッションを表示するGameObject")]
    [SerializeField] List<GameObject> _missionText = new List<GameObject>();
    [Tooltip("ミッションのDictionary")]
    Dictionary<MissionData, GameObject> _missionDict = new Dictionary<MissionData, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _missionList.Count; i++) 
        {
            _missionText[i].GetComponent<Text>().text = _missionList[i].MissionInfo;
            _missionDict.Add(_missionList[i], _missionText[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ミッションを達成させる
    /// </summary>
    /// <param name="useType"></param>
    /// <param name="Name"></param>
    public void MissionJudge(UseType useType, string Name)
    {
        var achievement = _missionList.Where(x => useType == x._useType)
             .Where(x => x.name == Name).Where(x => !x.MissionAchievement).ToList();

        //foreach()

    }
}
