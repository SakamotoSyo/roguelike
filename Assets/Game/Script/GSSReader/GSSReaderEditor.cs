using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(GSSReader))]
public class GSSReaderInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var r = target as GSSReader;
        var t = "�X�v���b�h�V�[�g�ǂݍ���";
        EditorGUI.BeginDisabledGroup(r.IsLoading);
        if (GUILayout.Button(t))
        {
            r.Reload();
        }
        EditorGUI.EndDisabledGroup();
    }
}