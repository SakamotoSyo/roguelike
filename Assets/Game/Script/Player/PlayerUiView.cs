using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUiView : MonoBehaviour
{
    [Header("HpのRectTransform")]
    [SerializeField] RectTransform _rectCurrent;
    [Header("MaxHpのテキスト")]
    [SerializeField] Text _maxHpText;
    [Header("現在のHpのテキスト")]
    [SerializeField] Text _currentHpText;
    [Header("Hp変化にかける時間")]
    [SerializeField] float _changeValueInterval = 0.5f;


    [Tooltip("HPバー最長の長さ")]
    float _maxHpWidth;
    [Tooltip("HPバーの最大値")]
    int _maxHp;
    [Tooltip("現在のHp")]
    int _currentHp;
    [Tooltip("アニメーションに使う一時的な現在値")]
    int _saveCurrentHp;
    [Tooltip("アニメーションを格納する変数")]
    Tween _anim;


    void Awake()
    {
        //最大の長さを保存しておく
        _maxHpWidth = _rectCurrent.sizeDelta.x;
    }

    /// <summary>
    ///MaxHpをセットする
    /// </summary>
    /// <param name="value">MaxのHp</param>
    public void SetMax(int value) 
    {
        _maxHp = value;
        _maxHpText.text = value.ToString();
    }

    public void SetCurrent(int newCurrentHp) 
    {
        Debug.Log(newCurrentHp);
        bool isPlus = newCurrentHp > _currentHp;

        _currentHp = newCurrentHp;
        _anim.Kill();

        if (isPlus)
        {
            _currentHpText.text = _currentHp.ToString();
            _saveCurrentHp = _currentHp;
            //バーの長さを更新
            _rectCurrent.SetWidth(GetWidth(_currentHp));
        }
        else 
        {
            _anim = DOTween.To(() => _saveCurrentHp,
                value =>
                {
                    _saveCurrentHp = value;
                    _currentHpText.text = _saveCurrentHp.ToString();
                    _rectCurrent.SetWidth(GetWidth(value));
                },
                _currentHp, 
                _changeValueInterval);
        }

    }

    /// <summary>
    /// 引数にHpを入れるとRect用に計算したものを返してくれる
    /// </summary>
    /// <param name="value">Hp</param>
    /// <returns></returns>
    float GetWidth(int value) 
    {
        float width = Mathf.InverseLerp(0, _maxHp, value);
        return Mathf.Lerp(0, _maxHpWidth, width);
    }

  
}

public static class UIExtensions 
{
    /// <summary>
    /// 現在の値をRectにセットする
    /// </summary>
    /// <param name="width"></param>
    public static void SetWidth(this RectTransform rect, float width)
    {
        Vector2 s =  rect.sizeDelta;
        s.x = width;
        rect.sizeDelta = s;
    }
}
