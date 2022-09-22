using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollUpScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    ScrollManager _scrollManager;

    void Start()
    {
        _scrollManager = GetComponentInParent<ScrollManager>();
    }
    /// <summary>
    /// �{�^�����I�����ꂽ�Ƃ��Ɏ��s
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        _scrollManager.ScrollUp(transform);

        ScrollManager.PreSelectedButton = gameObject;
    }

    /// <summary>
    /// �{�^�����I���������ꂽ�Ƃ��Ɏ��s
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDeselect(BaseEventData eventData)
    {

    }
}
