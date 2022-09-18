using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUiView : MonoBehaviour
{
    [Header("Hp��RectTransform")]
    [SerializeField] RectTransform _rectCurrent;
    [Header("MaxHp�̃e�L�X�g")]
    [SerializeField] Text _maxHpText;
    [Header("���݂�Hp�̃e�L�X�g")]
    [SerializeField] Text _currentHpText;
    [Header("Hp�ω��ɂ����鎞��")]
    [SerializeField] float _changeValueInterval = 0.5f;


    [Tooltip("HP�o�[�Œ��̒���")]
    float _maxHpWidth;
    [Tooltip("HP�o�[�̍ő�l")]
    int _maxHp;
    [Tooltip("���݂�Hp")]
    int _currentHp;
    [Tooltip("�A�j���[�V�����Ɏg���ꎞ�I�Ȍ��ݒl")]
    int _saveCurrentHp;
    [Tooltip("�A�j���[�V�������i�[����ϐ�")]
    Tween _anim;


    void Awake()
    {
        //�ő�̒�����ۑ����Ă���
        _maxHpWidth = _rectCurrent.sizeDelta.x;
    }

    /// <summary>
    ///MaxHp���Z�b�g����
    /// </summary>
    /// <param name="value">Max��Hp</param>
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
            //�o�[�̒������X�V
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
    /// ������Hp�������Rect�p�Ɍv�Z�������̂�Ԃ��Ă����
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
    /// ���݂̒l��Rect�ɃZ�b�g����
    /// </summary>
    /// <param name="width"></param>
    public static void SetWidth(this RectTransform rect, float width)
    {
        Vector2 s =  rect.sizeDelta;
        s.x = width;
        rect.sizeDelta = s;
    }
}
