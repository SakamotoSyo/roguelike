using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "MissionData", menuName = "MissionData")]
public class MissionData : ScriptableObject
{
    [Header("名前")]
    public string NameString = "";
    [Header("使用タイプ")]
    public UseType _useType = UseType.Use;
    [Header("特定の回数")]
    public int Number;
    [Header("ミッションの内容")]
    public string MissionInfo;

    [Tooltip("ミッションを達成できたかどうか")]
    public bool MissionAchievement;
}

public enum UseType 
{
    Use,
    Throw,
    Put,
    Knock,

}
