using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "MissionData", menuName = "MissionData")]
public class MissionData : ScriptableObject
{
    [Header("���O")]
    public string NameString = "";
    [Header("�g�p�^�C�v")]
    public UseType _useType = UseType.Use;
    [Header("����̉�")]
    public int Number;
    [Header("�~�b�V�����̓��e")]
    public string MissionInfo;

    [Tooltip("�~�b�V������B���ł������ǂ���")]
    public bool MissionAchievement;
}

public enum UseType 
{
    Use,
    Throw,
    Put,
    Knock,

}
