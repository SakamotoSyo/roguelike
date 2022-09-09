using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogScript : SingletonBehaviour<LogScript>
{

    [Tooltip("TextLogを格納するList")]
    private List<string> _logList = new List<string>();

    [Header("ログを出力するパネル")]
    [SerializeField] private GameObject _logPanel;

    [Header("ログを出力するテキスト")]
    [SerializeField] private Text _logText;

    [Header("メッセージをどの程度表示するか")]
    [SerializeField] private float _messageTime;

    [Header("メッセージのスクロールにかける時間")]
    [SerializeField] private float _messageScroll;

    [Header("メッセージのスクロールバー")]
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
    /// Logにメッセージを出力する
    /// </summary>
    /// <param name="message">メッセージ</param>
    public void OutPutLog(string message)
    {

        //既に前のテキストが残っていた場合改行してTextを出力する
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

        //既にパネルが表示されていたらパネルの表示時間を元に戻す
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
    /// パネルをアクティブ非アクティブ切り替える
    /// </summary>
    private IEnumerator LogActive()
    {
        _logPanel.SetActive(true);
        yield return new WaitForSeconds(_messageTime);
        _logPanel.SetActive(false);

        StopCoroutine(_textCoroutine);
        _textCoroutine = null;
    }

    /// <summary>文字が見切れた時にTextをScrollする処理</summary>
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
    /// レベルアップしたときに呼ばれる
    /// </summary>
    public void PlayerLevelUpProcess(int level)
    {
        OutPutLog($"プレイヤーはレベル{level}にアップした");
    }

}
