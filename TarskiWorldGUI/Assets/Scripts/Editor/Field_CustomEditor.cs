using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Field))]
public class Field_CustomEditor : Editor
{
    string param = "";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("AddElement"))
        {
            ((Field)target).SpawnDefaultElement();
        }

        if (GUILayout.Button("DestroyElements"))
        {
            ((Field)target).DeleteObjs();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("AddText"))
        {
            ((Field)target).SpawnText(param);
        }

        param = GUILayout.TextField(param);
        GUILayout.EndHorizontal();
    }
}
