using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CharButton : MonoBehaviour
{

    [SerializeField]
    private int _uniCodeIndex = 0;
    [SerializeField]
    private TMPro.TextMeshProUGUI _textElement = default;
    [SerializeField]
    private Button _button = default;

    private void OnValidate()
    {
        if (_textElement == null)
            _textElement = GetComponent<TMPro.TextMeshProUGUI>();

        if(_textElement != null)
        {
            _textElement.text += " ";

        }
    }

    private void Awake()
    {
        _textElement.text = char.ConvertFromUtf32(_uniCodeIndex);
        if (_button == null)
            _button = GetComponent<Button>();

        _button.onClick.AddListener(ButtonClickedListener);
    }



    private static string GetUnicodeForTMPro(string iconUnicode)
    {
        int unicode = int.Parse(iconUnicode, System.Globalization.NumberStyles.HexNumber);
        string result = char.ConvertFromUtf32(unicode);

        return result;
    }

    private void ButtonClickedListener()
    {
        GameManager.Instance.AddUnicodeIDToTextInput(_uniCodeIndex);
    }
}
