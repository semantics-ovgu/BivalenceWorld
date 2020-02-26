using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    public GenericEvent<GUI_TextInputElement> SelectedTextInputFieldElementeEvent = new GenericEvent<GUI_TextInputElement>();

    private void Awake()
    {
        SetInteractableButton(false);
        _validateImage.ActivateImage(false);
        //_inputFields.onEndEdit.AddListener(EndEdit);
        _inputFields.onValueChanged.AddListener(EndEdit);
        _inputFields.onSelect.AddListener(Selected);
        _inputFields.onDeselect.AddListener(Deselected);
    }

    private void Deselected(string arg0)
    {
        Debug.Log("DESelected");
        //SelectedTextInputFieldElementeEvent.InvokeEvent(null);
    }

    private void Selected(string arg0)
    {
        Debug.Log("Selected");
        SelectedTextInputFieldElementeEvent.InvokeEvent(this);
    }

    public void AddUnicodeId(int unicodeId)
    {
        _inputFields.text = _inputFields.text +  char.ConvertFromUtf32(unicodeId);
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
