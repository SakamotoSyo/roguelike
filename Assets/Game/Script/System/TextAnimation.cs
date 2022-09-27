using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextAnimation : MonoBehaviour
{
    [SerializeField] float _waitColor;
    [SerializeField] Text _text;

    float _timeCount;
    float _colorA = 0f;

    bool _isColorUp = false;

    void Start()
    {
        _text = GetComponent<Text>();
    }

    void Update()
    {
        _timeCount += Time.deltaTime;
        if (_colorA <= 0.9 && !_isColorUp && _waitColor < _timeCount)
        {
            _colorA += 0.04f;
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _colorA);
            _timeCount = 0;

            if (_colorA > 0.9) 
            {
                _isColorUp = true;
            }
        }
        else if (_colorA >= 0 && _isColorUp && _waitColor < _timeCount) 
        {
            _colorA -= 0.04f;
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _colorA);
            _timeCount = 0;

            if (_colorA == 0) 
            {
                _isColorUp = false;
            }
        }
    }

}
