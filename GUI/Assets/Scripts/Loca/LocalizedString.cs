
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomFramework.Localization;
using System;

/// <summary> Helper class to speed up serialization of localized string data. </summary>
[System.Serializable]
public class LocalizedString
{
    public const string UNASSIGNED_TEXT = "_unassignedText_";
    
    [SerializeField]
    private string _path = default;
    public string Path => _path;

    [SerializeField]
    private string _key = default;
    public string Key => _key;

#if UNITY_EDITOR
    [NonSerialized, TextArea(2, 10)]
    private string _preview = "";
#endif

    public LocalizedString(string path, string key)
    {
        _path = path;
        _key = key;
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public void UpdatePreview_Editor(object context)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            _preview = GetLocalizedString(context);
        }
#endif
    }

    public string GetLocalizedString(object context)
    {
        var instance = LocalizationDB.Instance;
        if(instance)
        {
            return instance.Localize(_path, _key, context);
        }

        return "LOC_DB_NULL)";
    }

    private void OpenLocaFile()
    {   
        LocalizationDB.OpenLocaFileAtRawPath(_path);
    }
}
