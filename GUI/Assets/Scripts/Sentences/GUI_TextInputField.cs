using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GUI_TextInputField : APage
{
	[SerializeField]
	List<GUI_TextInputElement> _inputFields = default;
	public List<GUI_TextInputElement> InputField => _inputFields;

	[SerializeField]
	private GUI_TextInputElement _currentTextInputElement = default;
	public GUI_TextInputElement CurrentTextInputElement => _currentTextInputElement;

	[SerializeField]
	private GUI_SentenceButton _addNewSentenceButton = default;
	[SerializeField]
	private GUI_SentenceButton _removeLastButton = default;

	[SerializeField]
	private GUI_TextInputElement _prefab = default;
	[SerializeField]
	private Transform _anchor = default;


	private void Awake()
	{
		foreach (var item in _inputFields)
		{
			item.SelectedTextInputFieldElementeEvent.AddEventListener(SelectedListener);
		}

		_addNewSentenceButton.GetButton().onClick.AddListener(AddNewSentenceListener);
		_removeLastButton.GetButton().onClick.AddListener(RemoveLastSentenceListener);
	}

	public void DeleteSentencesTexts()
	{

		for (int i = _inputFields.Count-1; i >= 1 ; i--)
		{
			var instance = _inputFields[i];
			_inputFields.Remove(instance);
			Destroy(instance.gameObject);
		}
	}

	public void AddNewSentenceListener()
	{
		GUI_TextInputElement instance = Instantiate(_prefab, _anchor);
		instance.SelectedTextInputFieldElementeEvent.AddEventListener(SelectedListener);
		_inputFields.Add(instance);
	}

	public void AddNewTextToDefault(string txt)
	{
		if (_inputFields.Count > 0)
		{
			_inputFields[0].RemoveText();
			_inputFields[0].AddText(txt);
		}
		else
		{
			Debug.LogWarning("Can not find default Text for GUI_TextInputField", this.gameObject);
		}
	}

	public void AddNewSentence(string text)
	{
		AddNewSentenceListener();
		_inputFields[_inputFields.Count-1].AddText(text);
	}

	public void RemoveLastSentenceListener()
	{
		if (_inputFields.Count > 1)
		{
			GUI_TextInputElement targetTextField = null;
			if (_currentTextInputElement != null)
			{
				targetTextField = _currentTextInputElement;
			}
			else
			{
				targetTextField = _inputFields[_inputFields.Count - 1];
			}
			_inputFields.Remove(targetTextField);
			Destroy(targetTextField.gameObject);
		}
	}

	public void CleanAllText()
	{
		foreach (var item in InputField)
		{
			if (!item.IsEmptyString())
			{
				item.RemoveText();
			}
		}
	}

	public List<string> GetInputFieldText()
	{
		List<string> list = new List<string>();

		foreach (var item in InputField)
		{

			list.Add(item.GetInputText());
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
