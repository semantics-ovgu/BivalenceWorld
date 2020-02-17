using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FloatVar))]
public class FloatVar_CustomEditor : Editor
{
    string param = "";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Send Changed Event"))
        {
            ((FloatVar)target).ForceChangedEvent();
        }


    }
}
