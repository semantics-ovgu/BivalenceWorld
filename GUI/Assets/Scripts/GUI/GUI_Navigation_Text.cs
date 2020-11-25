using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Navigation_Text : GUI_TabNavigation
{
	[SerializeField]
	private Button _createNewTextButton = default;

	[SerializeField]
	protected GUI_TextFieldButton _textfieldButtonPrefab = default;
	[SerializeField]
	protected GUI_TextFieldButton _textFieldButtonDefault = default;
	[SerializeField]
	protected APage _page = default;

	protected override void OnAwake()
	{
		base.OnAwake();

		_textPanelsInstances.Add(new Pair()
		{
				Button = _textFieldButtonDefault,
				Page = _page
		});
		_textFieldButtonDefault.GetButton().onClick.AddListener(() => ButtonClickedListener(_textFieldButtonDefault));
		ButtonClickedListener(_textFieldButtonDefault);
		_createNewTextButton.onClick.AddListener(CreateNewTextButtonListener);
	}

	public void CreateTextInstance(List<string> text)
	{
		GUI_TextFieldButton instance = Instantiate(_textfieldButtonPrefab, _buttonAnchor);
		var page = new Pair()
		{
				Button = instance,
				Page = _page
		};

		_textPanelsInstances.Add(page);
		instance.SetTexts(text);
		instance.GetButton().onClick.AddListener(() => ButtonClickedListener(instance));
		ButtonClickedListener(instance);
	}

	private void CreateNewTextButtonListener()
	{
		CreateTextInstance(new List<string>());
	}

	private void ButtonClickedListener(GUI_TabButton button)
	{
		if (_currentInstance != null && _currentInstance != null)
		{
			if (_currentInstance.Page is GUI_TextInputField field)
			{
				SaveText(_currentInstance.Button as GUI_TextFieldButton, field);
				ActivatePanel(GUI_TabNavigation.EType.Sentences);
				SetCurrentInstanceText(button as GUI_TextFieldButton);
			}
			else
			{
				ActivatePanel(GUI_TabNavigation.EType.Sentences);
				SetCurrentInstanceText(button as GUI_TextFieldButton);
			}
		}
		else
		{
			ActivatePanel(GUI_TabNavigation.EType.Sentences);
			SetCurrentInstanceText(button as GUI_TextFieldButton);
		}
	}

	private void SaveText(GUI_TextFieldButton button, GUI_TextInputField field)
	{
		if (button != null)
		{
			button.SetTexts(field.GetInputFieldText());
		}
	}

	private void SetCurrentInstanceText(GUI_TextFieldButton textfield)
	{
		_currentInstance.Button = textfield;
		if (_currentInstance.Page is GUI_TextInputField field)
		{
			List<string> text = textfield.GetText();

			for (int i = 0; i < field.InputField.Count; i++)
			{
				field.InputField[i].RemoveText();
				if (i < text.Count)
				{
					field.InputField[i].AddText(text[i]);
				}
			}
		}
	}

	public override void CreateGame()
	{
		if (_currentInstance.Page is GUI_TextInputField field)
		{
			SaveText(_currentInstance.Button as GUI_TextFieldButton, field);
		}

		base.CreateGame();
	}

	public override void CreateModelRepresentation()
	{
		if (_currentInstance.Page is GUI_TextInputField field)
		{
			SaveText(_currentInstance.Button as GUI_TextFieldButton, field);
		}
		base.CreateModelRepresentation();
	}
}
