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
    protected GUI_TextFieldCloseButton _textFieldButtonDefault = default;
    [SerializeField]
    protected APage _page = default;
    [SerializeField]
    private GUI_TextInputField _field = default;

    private GUI_TextFieldButton _currentSelectedButton = default;

    public GUI_TextFieldButton GetCurrentSelectedButton()
    {
        SaveText(_currentPageInstance.Button as GUI_TextFieldButton, _field);
        return _currentSelectedButton;
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        _textPanelsInstances.Add(new Pair()
        {
            Button = _textFieldButtonDefault,
            Page = _page
        });

        _textFieldButtonDefault.SetTexts(new List<string>());
        SetCurrentSelectButton(_textFieldButtonDefault);
        _textFieldButtonDefault.GetButton().onClick.AddListener(() => ButtonClickedListener(_textFieldButtonDefault));
        ButtonClickedListener(_textFieldButtonDefault);
        _createNewTextButton.onClick.AddListener(CreateNewTextButtonListener);
        _textFieldButtonDefault.DestroyObjectEvent.AddEventListener(DefaultButtonResetClickedListener);
    }

    private void DefaultButtonResetClickedListener(GUI_TextFieldCloseButton arg0)
    {
        ButtonClickedListener(_textFieldButtonDefault);
        _currentSelectedButton.SetButtonName("Untitled Sentences");
        if (_currentPageInstance.Page is GUI_TextInputField field)
        {
            CleanField(field);
        }
    }

    public GUI_TextFieldButton CreateTextInstance(string name, List<string> text)
    {
        GUI_TextFieldCloseButton instance = Instantiate(_textfieldButtonPrefab, _buttonAnchor);
        var page = new Pair()
        {
            Button = instance,
            Page = _page
        };

        instance.SetButtonName(name);
        _textPanelsInstances.Add(page);
        instance.SetTexts(text);
        SetCurrentSelectButton(instance);
        instance.GetButton().onClick.AddListener(() => ButtonClickedListener(instance));
        instance.DestroyObjectEvent.AddEventListener(RemoveTextInstance);
        ButtonClickedListener(instance);

        return instance;
    }

    public void OverwriteCurrentText(string name, List<string> text)
    {
        _currentSelectedButton.SetButtonName(name);
        _currentSelectedButton.SetTexts(text);
        SetCurrentInstanceText(_currentSelectedButton);
    }

    private void SetCurrentSelectButton(GUI_TextFieldButton newFieldButton)
    {
        if (_currentSelectedButton != null && newFieldButton != _currentSelectedButton)
        {
            _currentSelectedButton.UnHover();
        }

        _currentSelectedButton = newFieldButton;
        _currentSelectedButton.Hover();
    }

    private void MessageBoxDontSaveButtonClickedEventListener(GUI_TextFieldButton obj)
    {
        DestroyButton(obj);
    }

    private void MessageBoxSaveButtonClickedEventListener(GUI_TextFieldButton obj)
    {
        if (GameManager.Instance.SaveCurrentSentences(obj))
        {
            DestroyButton(obj);
        }
    }

    private void DestroyButton(GUI_TextFieldButton button)
    {
        var instance = _textPanelsInstances.Find(x => x.Button == button);
        if (instance != null)
        {
            _textPanelsInstances.Remove(instance);
            Destroy(instance.Button.gameObject);
        }

        var fieldButton = _textPanelsInstances[0];
        ButtonClickedListener(fieldButton.Button);
    }

    private void RemoveTextInstance(GUI_TextFieldButton arg0)
    {
        if (_currentSelectedButton == arg0)
        {
            SaveText(arg0, _field);
        }

        if (arg0.HasUnsavedChanges)
        {
            var messageBox = GameManager.Instance.CreateMessageBox();
            messageBox.Init($"Do you want to save the changes you made in \"{arg0.GetButtonName()}\"?");
            messageBox.OnSaveButtonClickedEvent.AddListener(() => MessageBoxSaveButtonClickedEventListener(arg0));
            messageBox.OnDontSaveButtonClickedEvent.AddListener(() => MessageBoxDontSaveButtonClickedEventListener(arg0));
        }
        else
        {
            MessageBoxDontSaveButtonClickedEventListener(arg0);
        }
    }

    private void CreateNewTextButtonListener()
    {
        CreateTextInstance("Untitled Sentences", new List<string>()
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
        SetCurrentSelectButton(textfield);
        _currentPageInstance.Button = textfield;
        if (_currentPageInstance.Page is GUI_TextInputField field)
        {
            List<string> text = textfield.GetText();
            CleanField(field);

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
        }
    }

    private void CleanField(GUI_TextInputField field)
    {
        field.DeleteSentencesTexts();
        field.InputField[0].InputField.text = "";
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
