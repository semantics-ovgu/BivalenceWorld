using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Button = UnityEngine.UI.Button;

public class GUI_MessageBox : MonoBehaviour
{
    [SerializeField]
    private Button _saveButton = default;
    [SerializeField]
    private Button _dontSaveButton = default;
    [SerializeField]
    private Button _cancelButton = default;
    [SerializeField]
    private TMP_Text _messageText = default;

    public UnityEvent OnSaveButtonClickedEvent = new UnityEvent();
    public UnityEvent OnDontSaveButtonClickedEvent = new UnityEvent();
    public UnityEvent OnCancelButtonClickedEvent = new UnityEvent();

    private void Awake()
    {
        _saveButton.onClick.AddListener(SaveButtonClickedEventListener);
        _dontSaveButton.onClick.AddListener(DontSaveButtonClickedEventListener);
        _cancelButton.onClick.AddListener(CancelButtonEventListener);
    }

    public void Init(string message)
    {
        _messageText.text = message;
    }

    private void CancelButtonEventListener()
    {
        OnCancelButtonClickedEvent.Invoke();
        Destroy(gameObject);
    }

    private void DontSaveButtonClickedEventListener()
    {
        OnDontSaveButtonClickedEvent.Invoke();
        Destroy(gameObject);
    }

    private void SaveButtonClickedEventListener()
    {
        OnSaveButtonClickedEvent.Invoke();
        Destroy(gameObject);
    }
}
