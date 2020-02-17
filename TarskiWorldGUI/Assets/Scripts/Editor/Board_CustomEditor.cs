using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Board))]
public class Board_CustomEditor  : Editor
{
    bool param = false;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("CreateBoard"))
        {
            ((Board)target).CreateMap();
        }
        if (GUILayout.Button("DeleteBoard"))
        {
            ((Board)target).DestroyMap();
        }
    }
}
