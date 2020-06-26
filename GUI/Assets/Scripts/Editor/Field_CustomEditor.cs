using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Field))]
public class Field_CustomEditor : Editor
{
    string param = "";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        //if (GUILayout.Button("AddText"))
        //{
        //    ((Field)target).SpawnText(param);
        //}

        param = GUILayout.TextField(param);
        GUILayout.EndHorizontal();
    }
}
