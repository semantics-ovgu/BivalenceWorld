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
	protected GUI_TextFieldCloseButton _textfieldButtonPrefab = default;
	[SerializeField]
	protected GUI_TextFieldButton _textFieldButtonDefault = default;
	[SerializeField]
	protected APage _page = default;
	[SerializeField]
	private GUI_TextInputField _field = default;

	private Button _currentSelectedButton = default;

	protected override void OnAwake()
	{
		base.OnAwake();


		_textPanelsInstances.Add(new Pair()
		{
				Button = _textFieldButtonDefault,
				Page = _page
		});
		_textFieldButtonDefault.SetTexts(new List<string>());
		_currentSelectedButton = _textFieldButtonDefault.GetButton();
		_textFieldButtonDefault.GetButton().onClick.AddListener(() => ButtonClickedListener(_textFieldButtonDefault));
		ButtonClickedListener(_textFieldButtonDefault);
		_createNewTextButton.onClick.AddListener(CreateNewTextButtonListener);
	}

	public void CreateTextInstance(List<string> text)
	{
		GUI_TextFieldCloseButton instance = Instantiate(_textfieldButtonPrefab, _buttonAnchor);
		var page = new Pair()
		{
				Button = instance,
				Page = _page
		};

		_textPanelsInstances.Add(page);
		instance.SetTexts(text);
		_currentSelectedButton = instance.GetButton();
		instance.GetButton().onClick.AddListener(() => ButtonClickedListener(instance));
		instance.DestroyObjectEvent.AddEventListener(RemoveTextInstance);
		ButtonClickedListener(instance);
	}

	private void RemoveTextInstance(GUI_TextFieldCloseButton arg0)
	{
		var instance = _textPanelsInstances.Find(x => x.Button = arg0);
		if (instance != null)
		{
			_textPanelsInstances.Remove(instance);
			Destroy(instance.Button.gameObject);
		}
	}

	private void CreateNewTextButtonListener()
	{
		CreateTextInstance(new List<string>()
		{
				""
		});
	}

	private void ButtonClickedListener(GUI_TabButton button)
	{
		SaveText(_currentPageInstance.Button as GUI_TextFieldButton, _field);
		ActivatePanel(GUI_TabNavigation.EType.Sentences);
		SetCurrentInstanceText(button as GUI_TextFieldButton);
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
		_currentPageInstance.Button = textfield;
		if (_currentPageInstance.Page is GUI_TextInputField field)
		{
	
			List<string> text = textfield.GetText();
			field.DeleteSentencesTexts();

			if (text.Count > 0)
			{
				field.AddNewTextToDefault(text[0]);
			}

			if (text.Count > 1)
			{
				for (int i = 1; i < text.Count; i++)
				{
					field.AddNewSentence(text[i]);
				}
			}

	


			//for (int i = 0; i < field.InputField.Count; i++)
			//{
			//	field.InputField[i].RemoveText();
			//	if (i < text.Count)
			//	{
			//		field.InputField[i].AddText(text[i]);
			//	}
			//}
		}
	}

	public override void CreateGame()
	{
		if (_currentPageInstance.Page is GUI_TextInputField field)
		{
			SaveText(_currentPageInstance.Button as GUI_TextFieldButton, field);
		}

		base.CreateGame();
	}

	public override void CreateModelRepresentation()
	{
		if (_currentPageInstance.Page is GUI_TextInputField field)
		{
			SaveText(_currentPageInstance.Button as GUI_TextFieldButton, field);
		}
		base.CreateModelRepresentation();
	}
}
