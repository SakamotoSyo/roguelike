using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    [Header("スクロールスピード")]
    [SerializeField] float _scrollSpeed = 1000f;
    [Header("一回でスクロールする値")]
    [SerializeField] float _scrollValue = 415f;

    [Tooltip("アイテムボタン表示用のコンテンツ")]
    Transform _content;
    [Tooltip("スクロール中かどうか")]
    bool _changeScrollValue;
    [Tooltip("スクロールの目的地")]
    float _destinationValue;
    [Tooltip("アイテム一覧のスクロールのデフォルト値")]
    Vector3 _defaultScrollValue;
    [Tooltip("前に選択していたボタン")]
    public static GameObject PreSelectedButton;

    void Awake()
    {
        _content = transform;
        _defaultScrollValue = _content.transform.position;
    }

    void Update()
    {
        if (!_changeScrollValue) return;

        _content.transform.localPosition = new Vector3(_content.transform.localPosition.x, 
            Mathf.MoveTowards(_content.transform.localPosition.y, _destinationValue, _scrollSpeed * Time.deltaTime), _content.transform.localPosition.z);

        if (Mathf.Abs(_content.transform.localPosition.y - _destinationValue) < 0.2f) 
        {
            _changeScrollValue = false;
            _content.transform.localPosition = new Vector3(0f, _destinationValue, 0f);
        }
    }

    public void ScrollDown(Transform button) 
    {
        if (_changeScrollValue) 
        {
            _changeScrollValue = false;
            _content.transform.localPosition = new Vector3(_content.localPosition.x, _destinationValue, _content.transform.localPosition.z);
        }

        if (ScrollManager.PreSelectedButton != null
            && button.position.y > ScrollManager.PreSelectedButton.transform.position.y) 
        {
            _destinationValue = _content.transform.localPosition.y - _scrollValue;
            _changeScrollValue = true;
        }
    }

    public void ScrollUp(Transform button) 
    {
        if (_changeScrollValue) 
        {
            _changeScrollValue = false;
            _content.transform.localPosition = new Vector3(_content.transform.localPosition.x, _destinationValue, _content.transform.localPosition.z);
        }

        if (ScrollManager.PreSelectedButton != null
            && button.position.y < ScrollManager.PreSelectedButton.transform.position.y) 
        {
            _destinationValue = _content.transform.localPosition.y + _scrollValue;
            _changeScrollValue = true;
        }
    }

    public void Reset()
    {
        PreSelectedButton = null;
        transform.position = _defaultScrollValue;
    }
}
