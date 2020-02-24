using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_TextInputElement : MonoBehaviour
{
    [SerializeField]
    private Button _singleValidateButton = default;

    [SerializeField]
    private GUI_ValidateImage _validateImage = default;

    [SerializeField]
    private TMPro.TMP_InputField _inputFields = default;

    public Button ValidateButton => _singleValidateButton;

    private void Awake()
    {
        SetInteractableButton(false);
        _validateImage.ActivateImage(false);
        //_inputFields.onEndEdit.AddListener(EndEdit);
        _inputFields.onValueChanged.AddListener(EndEdit);
    }

    private void EndEdit(string arg0)
    {
        if (string.IsNullOrEmpty(arg0))
        {
            SetInteractableButton(false);
            _validateImage.ActivateImage(false);
        }
        else
        {
            SetInteractableButton(true);
        }
    }

    private void SetInteractableButton(bool isInteractable)
    {
        _singleValidateButton.interactable = isInteractable;
    }

    public bool IsEmptyString()
    {
        return string.IsNullOrEmpty(_inputFields.text) ? true : false;
    }

    public string GetInputText()
    {
        return _inputFields.text;
    }

    public void Validate(bool v)
    {
        _validateImage.SetColor(v);
    }
}
