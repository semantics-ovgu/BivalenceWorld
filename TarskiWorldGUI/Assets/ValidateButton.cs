using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValidateButton : MonoBehaviour
{
    [SerializeField]
    private Button _targetButton = default;

    private void Awake()
    {
        _targetButton.onClick.AddListener(ButtonClickedListener);
    }

    private void ButtonClickedListener()
    {
        var resultSentences = new List<string>();
        var list = GameManager.Instance.GetTextInputField().GetGuiTextElementsWithText();
        foreach (var item in list)
        {
            resultSentences.Add(item.GetInputText());
            Debug.Log(item.GetInputText());
        }
    }
}
