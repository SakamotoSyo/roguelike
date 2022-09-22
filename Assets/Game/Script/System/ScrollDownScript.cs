using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollDownScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    ScrollManager _scrollManager;

    void Start() 
    {
        _scrollManager = GetComponentInParent<ScrollManager>(); 
    }
    /// <summary>
    /// ボタンが選択されたときに実行
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        _scrollManager.ScrollDown(transform);

        ScrollManager.PreSelectedButton = gameObject;
    }

    /// <summary>
    /// ボタンが選択解除されたときに実行
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDeselect(BaseEventData eventData)
    {
        
    }
}
