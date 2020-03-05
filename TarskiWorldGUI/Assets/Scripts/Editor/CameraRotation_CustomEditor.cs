using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraRotation))]
public class CameraRotation_CustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("RotateCameraDefault"))
        {
            ((CameraRotation)target).SetCameraDefault();
        }
    }
}
