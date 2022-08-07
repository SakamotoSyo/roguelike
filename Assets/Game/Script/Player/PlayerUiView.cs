using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUiView : MonoBehaviour
{
    [SerializeField] Slider _hpScroll;

    [SerializeField] Text _maxHpText;
    [SerializeField] Text _currentHpText;

    void Awake()
    {

    }

    /// <summary>
    /// 現在のHpの状況を更新する関数
    /// </summary>
    /// <param name="maxHp"></param>
    /// <param name="currentHp"></param>
    public void SetHp(float maxHp, float currentHp)
    {
        _hpScroll.value = currentHp / maxHp;
        _maxHpText.text = maxHp.ToString();
        _currentHpText.text = currentHp.ToString();
        Debug.Log($"{maxHp}の通知を受け取った");
    }


}
