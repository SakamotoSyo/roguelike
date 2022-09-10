using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogScript : SingletonBehaviour<LogScript>
{

    [Tooltip("TextLog���i�[����List")]
    private List<string> _logList = new List<string>();

    [Header("���O���o�͂���p�l��")]
    [SerializeField] private GameObject _logPanel;

    [Header("���O���o�͂���e�L�X�g")]
    [SerializeField] private Text _logText;

    [Header("���b�Z�[�W���ǂ̒��x�\�����邩")]
    [SerializeField] private float _messageTime;

    [Header("���b�Z�[�W�̃X�N���[���ɂ����鎞��")]
    [SerializeField] private float _messageScroll;

    [Header("���b�Z�[�W�̃X�N���[���o�[")]
    [SerializeField] private Scrollbar _scrollbar;

    private float _messageCountTime;

    private Coroutine _textCoroutine;

    void Start()
    {
    }

    private void Update()
    {
         LogActive();

        if (_scrollbar.value != 0)
        {
            LogScroll();
        }
    }

    /// <summary>
    /// Log�Ƀ��b�Z�[�W���o�͂���
    /// </summary>
    /// <param name="message">���b�Z�[�W</param>
    public void OutPutLog(string message)
    {

        //���ɑO�̃e�L�X�g���c���Ă����ꍇ���s����Text���o�͂���
        if (_logList.Count == 0)
        {
            _logList.Add(message);
            _logText.text = message;
        }
        else
        {
            var St = "\n" + message;
            _logList.Add(St);
            _logText.text += St;
        }

        //���Ƀp�l�����\������Ă�����p�l���̕\�����Ԃ����ɖ߂�
        if (_textCoroutine == null)
        {
            _textCoroutine = StartCoroutine(LogActive());

        }
        else
        {
            StopCoroutine(_textCoroutine);
            _textCoroutine = null;
            _textCoroutine = StartCoroutine(LogActive());
        }
    }

    /// <summary>
    /// �p�l�����A�N�e�B�u��A�N�e�B�u�؂�ւ���
    /// </summary>
    private IEnumerator LogActive()
    {
        _logPanel.SetActive(true);
        yield return new WaitForSeconds(_messageTime);
        _logPanel.SetActive(false);

        StopCoroutine(_textCoroutine);
        _textCoroutine = null;
    }

    /// <summary>���������؂ꂽ����Text��Scroll���鏈��</summary>
    private void LogScroll()
    {
        _messageCountTime += Time.deltaTime;
        if (_messageScroll > _messageCountTime)
        {
            return;
        }

        _scrollbar.value -= 0.01f;

        if (_scrollbar.value <= 0)
        {
            _scrollbar.value = 0;
        }

        _messageCountTime = 0;
    }

    /// <summary>
    /// ���x���A�b�v�����Ƃ��ɌĂ΂��
    /// </summary>
    public void PlayerLevelUpProcess(int level)
    {
        OutPutLog($"�v���C���[�̓��x��{level}�ɃA�b�v����");
    }

}
