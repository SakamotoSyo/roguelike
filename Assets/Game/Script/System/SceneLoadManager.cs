using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    /// <summary>
    ///�{�^���C�x���g�Ȃǂ���Ăԗp�̊֐�
    /// </summary>
    /// <param name="SceneName">�V�[���̖��O</param>
    public void ChangeScene(string SceneName)
    {
        Debug.Log($"{SceneName}��Load����܂���");
        SceneManager.LoadScene(SceneName);
    }
}