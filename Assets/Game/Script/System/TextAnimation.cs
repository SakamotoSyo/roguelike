using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextAnimation : MonoBehaviour
{
    [SerializeField] private float _waitColor;
    [SerializeField] private Text _text;

    private float _timeCount;
    private float _colorA = 0f;

    private bool _isColorUp = false;

    private void Start()
    {
        _text = GetComponent<Text>();
    }

    private void Update()
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
