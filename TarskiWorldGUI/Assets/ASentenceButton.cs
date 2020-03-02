using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ASentenceButton : MonoBehaviour
{
    [SerializeField]
    protected TMPro.TextMeshProUGUI _textElement = default;
    [SerializeField]
    protected Button _button = default;
    [SerializeField]
    private bool _spaceBeforeText = false;
    [SerializeField]
    private bool _spaceAfterText = false;

    private void OnValidate()
    {
        if (_textElement == null)
        {
            _textElement = GetComponent<TMPro.TextMeshProUGUI>();
        }

        if (_textElement != null)
        {
            _textElement.text = GetDisplayString();
        }
    }

    private void Awake()
    {
        _button.onClick.AddListener(ButtonClickedListener);

        if (_button == null)
        {
            _button = GetComponent<Button>();
        }
    }

    protected void SpaceBeforeText()
    {
        if (_spaceBeforeText)
        {
            SetSpaceText();
        }
    }

    protected void SpaceAfterText()
    {
        if (_spaceAfterText)
        {
            SetSpaceText();
        }
    }

    private void SetSpaceText()
    {
        GameManager.Instance.GetTextInputField()?.CurrentTextInputElement.AddText(" ");
    }

    protected abstract void ButtonClickedListener();

    protected abstract string GetDisplayString();
}
