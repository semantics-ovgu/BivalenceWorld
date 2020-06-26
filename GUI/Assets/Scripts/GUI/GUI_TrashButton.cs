using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_TrashButton : MonoBehaviour
{
    [SerializeField]
    private Button _targetButton = default;

    private void OnValidate()
    {
        if (_targetButton == null)
            _targetButton = GetComponent<Button>();
    }

    private void Awake()
    {
        _targetButton.onClick.AddListener(ButtonClickedListener);
    }

    private void ButtonClickedListener()
    {
        GameManager.Instance.GetTextInputField().CurrentTextInputElement?.RemoveText();
    }
}
