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
    /// ���݂�Hp�̏󋵂��X�V����֐�
    /// </summary>
    /// <param name="maxHp"></param>
    /// <param name="currentHp"></param>
    public void SetHp(float maxHp, float currentHp)
    {
        _hpScroll.value = currentHp / maxHp;
        _maxHpText.text = maxHp.ToString();
        _currentHpText.text = currentHp.ToString();
        //Debug.Log($"{maxHp}�̒ʒm���󂯎����");
    }


}
