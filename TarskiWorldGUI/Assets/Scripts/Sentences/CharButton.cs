using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class CharButton : ASentenceButton
{
    [SerializeField]
    private int _uniCodeIndex = 0;

    protected override string GetDisplayString()
    {
        return char.ConvertFromUtf32(_uniCodeIndex); 
    }

    private void Start()
    {
        _textElement.text = GetDisplayString();
    }

    protected override void ButtonClickedListener()
    {
        SpaceBeforeText();
        GameManager.Instance.GetTextInputField().CurrentTextInputElement?.AddUnicodeId(_uniCodeIndex);
        SpaceAfterText();
    }
}
