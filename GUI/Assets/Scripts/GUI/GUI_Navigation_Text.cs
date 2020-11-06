using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Navigation_Text : MonoBehaviour
{
	public enum EType
	{
		Sentences,
		WorldText,
		Game
	}

	[SerializeField]
	private List<GUI_TextFieldButton> _panels = default;
	[SerializeField]
	private GUI_TextFieldButton _textFieldButtonPrefab = default;
	[SerializeField]
	private Transform _anchor = default;
	[SerializeField]
	private Button _createNewTextButton = default;

	[SerializeField]
	private List<ATextPanel> _textPanelsInstances = new List<ATextPanel>();

	[SerializeField, Header("ReadOnly")]
	private CurrentSelected _currentSelectedInstance = default;

	[System.Serializable]
	public class CurrentSelected
	{
		public ATextPanel CurrrentActivatePanel = default;
		public GUI_TextFieldButton Button = default;
	}

	private void Awake()
	{
		_currentSelectedInstance = new CurrentSelected();
		if (_panels.Count > 0)
		{
			for (var i = 0; i < _panels.Count; i++)
			{
				var element = _panels[i];
				element.GetButton().onClick.AddListener(() => ButtonClickedListener(element));
			}

			ButtonClickedListener(_panels[0]);
		}
		_createNewTextButton.onClick.AddListener(CreateNewTextButtonListener);
	}

	public void ActivatePanel(EType type)
	{
		ATextPanel targetPanel = _textPanelsInstances.Find(x => x.GetType() == type);
		if (targetPanel == null)
		{
			Debug.LogWarning("Can not find targetPanel with EnuM: " + type.ToString());
		}
		else
		{
			ActivatePanel(targetPanel);
		}
	}


	public void CreateTextInstance(List<string> text)
	{
		var instance = Instantiate(_textFieldButtonPrefab, _anchor);
		instance.SetTexts(text);
		instance.GetButton().onClick.AddListener(() => ButtonClickedListener(instance));
		_panels.Add(instance);
		ButtonClickedListener(instance);
	}

	public void CreateGame()
	{
		if (_currentSelectedInstance.CurrrentActivatePanel is GUI_TextInputField field)
		{
			_currentSelectedInstance.Button.SetTexts(field.GetInputFieldText());
		}
		ActivatePanel(EType.Game);
	}

	public void CreateModelRepresentation()
	{

		if (_currentSelectedInstance.CurrrentActivatePanel is GUI_TextInputField field)
		{
			_currentSelectedInstance.Button.SetTexts(field.GetInputFieldText());
		}
		ActivatePanel(EType.WorldText);
	}

	public void DeleteText()
	{

	}

	private void CreateNewTextButtonListener()
	{
		CreateTextInstance(new List<string>());
	}

	private void ButtonClickedListener(GUI_TextFieldButton textField)
	{
		if (_currentSelectedInstance != null && _currentSelectedInstance.CurrrentActivatePanel != null)
		{
			if (_currentSelectedInstance.CurrrentActivatePanel is GUI_TextInputField field)
			{
				_currentSelectedInstance.Button.SetTexts(field.GetInputFieldText());
				ActivatePanel(EType.Sentences);
				SetCurrentInstanceText(textField);
			}
			else
			{
				ActivatePanel(EType.Sentences);
				SetCurrentInstanceText(textField);
			}

		}
		else
		{
			ActivatePanel(EType.Sentences);
			SetCurrentInstanceText(textField);
		}
	}

	private void SetCurrentInstanceText(GUI_TextFieldButton textfield)
	{
		_currentSelectedInstance.Button = textfield;
		if (_currentSelectedInstance.CurrrentActivatePanel is GUI_TextInputField field)
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

	private void ActivatePanel(ATextPanel panel)
	{
		if(_currentSelectedInstance.CurrrentActivatePanel != null)
			_currentSelectedInstance.CurrrentActivatePanel.DeactivatePanel();

		_currentSelectedInstance.CurrrentActivatePanel = panel;
		_currentSelectedInstance.CurrrentActivatePanel.ActivatePanel();
	}
}
