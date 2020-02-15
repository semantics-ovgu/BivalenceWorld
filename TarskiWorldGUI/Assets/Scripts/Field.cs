using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    // X Y
    // Konstanten (Unendlich A - Z)
    // Predicate (Tet und BIG -> Liste)

    [SerializeField]
    private Transform _anchor = default;
    [SerializeField]
    private GameObject _debugInformationCanvas = default;
    [SerializeField]
    private GameObject _obj = default;

    [SerializeField]
    private GameObject _worldCanvasPrefab = default;

    private List<GameObject> _constant = new List<GameObject>();
    private List<GameObject> _predicate = new List<GameObject>();
    private int _x;
    private int _z;

    public void Init(int x, int z)
    {
        _x = x;
        _z = z;
        SpawnTextIntern("X: " + x + "\nZ: " + z);
    }

    public void SpawnDefaultElement()
    {
        var instance = Instantiate(_obj, _anchor);
        _predicate.Add(instance);
    }

    public void DeleteObjs()
    {
        for (int i = _constant.Count -1 ; i >= 0; i--)
        {
            DestroyImmediate(_constant[i]);
        }
    }

    private void SpawnTextIntern(string txt)
    {

        GameObject canvas = Instantiate(_debugInformationCanvas, this.transform.position, Quaternion.identity, this.transform);

        canvas.transform.SetParent(this.transform);
        canvas.transform.localRotation = Quaternion.Euler(90, 90, 0);
        canvas.transform.localPosition = new Vector3(0f, 0.15f, 0f);
        canvas.transform.localScale = new Vector3(1,1,1);
        //canvas.transform.localRotation.SetFromToRotation(canvas.transform.localRotation.ToEuler(), new Vector3(90, 90, 0));
        canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = txt;
    }

    public void SpawnText(string constant)
    {
        if(_predicate.Count > 0)
        {
            GameObject obj = _predicate[0];
           
            GameObject canvas = Instantiate(_worldCanvasPrefab, obj.transform.position, Quaternion.identity, obj.transform);
            canvas.transform.localPosition = new Vector3(0, 1, 0);
            canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = constant;
        }
    }
}
