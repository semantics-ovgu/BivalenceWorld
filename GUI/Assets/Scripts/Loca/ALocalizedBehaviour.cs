using CustomFramework.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class to inherit from to apply localized text from <see cref="LocalizationDB"/> as a <see cref="MonoBehaviour"/>.
/// </summary>
public abstract class ALocalizedBehaviour : MonoBehaviour
{   
    [SerializeField]
    private LocalizedString _localizedString = default;

#if UNITY_EDITOR
    [SerializeField]
    private bool _showRealPreview = true;
#endif

    protected virtual void OnValidate()
    {
        if(Application.isEditor && !Application.isPlaying)
        {
            UpdateAllTexts();
        }
    }

    protected virtual void Awake()
    {   
        UpdateLocalizedText();
    }

    protected virtual void Start()
    {
        var localizationDb = LocalizationDB.Instance;

        if (localizationDb)
        {
            localizationDb.LanguageLoadedEvent.AddEventListener(x => UpdateLocalizedText());
        }
    }

    protected abstract void SetLocalizedText(string text);

        private void UpdateAllTexts()
    {
#if UNITY_EDITOR
        UpdateInspectorText();

        if (_showRealPreview)
            UpdateLocalizedText();

        else
            SetLocalizedText(LocalizedString.UNASSIGNED_TEXT);
#else
          UpdateLocalizedText();
#endif
    }

    private void UpdateLocalizedText()
    {
        SetLocalizedText(_localizedString.GetLocalizedString(this));
    }

#if UNITY_EDITOR

    private void UpdateAllTextsEditor()
    {
        UpdateAllTexts();
    }
  
    private void UpdateInspectorText()
    {
        if(_localizedString != null)
            _localizedString.UpdatePreview_Editor(this);
    }

    private void SelectLocalizationDB()
    {   
        UnityEditor.Selection.activeObject = LocalizationDB.Instance;
    }

#endif
}
