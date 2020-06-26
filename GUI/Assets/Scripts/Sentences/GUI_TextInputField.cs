using System;
using System.Collections.Generic;
using UnityEngine;

public class GUI_TextInputField : MonoBehaviour
{
    [SerializeField]
    List<GUI_TextInputElement> _inputFields = default;
    public List<GUI_TextInputElement> InputField => _inputFields;

    [SerializeField]
    private GUI_TextInputElement _currentTextInputElement = default;
    public GUI_TextInputElement CurrentTextInputElement => _currentTextInputElement;

    public List<GUI_TextInputElement> GetTextInputElement()
    {
        return _inputFields;
    }

    private void Awake()
    {
        foreach (var item in _inputFields)
        {
            item.SelectedTextInputFieldElementeEvent.AddEventListener(SelectedListener);
        }
    }

    public List<string> GetInputFieldText()
    {
        List<string> list = new List<string>();

        foreach (var item in InputField)
        {
            if(!item.IsEmptyString())
            {
                list.Add(item.GetInputText());

            }
        }

        return list;
    }

    public void ResetValidationOnTexts()
    {
	    foreach (var item in _inputFields)
	    {
		    item.ResetValidation();
	    }
    }

    private void SelectedListener(GUI_TextInputElement arg0)
    {
        _currentTextInputElement = arg0;
    }

    public List<GUI_TextInputElement> GetGuiTextElementsWithText()
    {
        var result = new List<GUI_TextInputElement>();
        foreach (GUI_TextInputElement item in InputField)
        {
            if (!item.IsEmptyString())
            {
                result.Add(item);
            }
        }
        return result;
    }

    private void OnValidate()
    {
        _inputFields = new List<GUI_TextInputElement>(GetComponentsInChildren<GUI_TextInputElement>());
    }
}
