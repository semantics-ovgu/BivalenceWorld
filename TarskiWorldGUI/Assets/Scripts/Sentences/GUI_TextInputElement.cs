using System;
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


    public void AddText(string txt)
    {
        int carePos = _inputFields.caretPosition;
        var text = _inputFields.text.Insert(carePos, txt);
        SetInputFieldText(text, carePos + txt.Length);
    }

    public void AddUnicodeId(int unicodeId)
    {
        int carePos = _inputFields.caretPosition;
        string newText = char.ConvertFromUtf32(unicodeId);
        var text = _inputFields.text.Insert(carePos, newText);
        SetInputFieldText(text, carePos + newText.Length);;
    }

    public void RemoveText()
    {
        _inputFields.text = "";
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


    private void SetInputFieldText(string txt, int caretPosition)
    {
   
        _inputFields.text = txt;
        SetCaretPosition(caretPosition);
    }

    private void SetCaretPosition(int caretPos)
    {
        _inputFields.Select();
        _inputFields.caretPosition = caretPos;
    }

    private void Deselected(string arg0)
    {
        //SelectedTextInputFieldElementeEvent.InvokeEvent(null);
    }

    private void Selected(string arg0)
    {
        SelectedTextInputFieldElementeEvent.InvokeEvent(this);
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

    internal void ParserValide(bool isValid)
    {
        if (!isValid)
        {
            _validateImage.ParseError();
        }
    }

    private void SetInteractableButton(bool isInteractable)
    {
        _singleValidateButton.interactable = isInteractable;
    }
}
