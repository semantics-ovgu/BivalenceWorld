using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IDebug
{
    [SerializeField]
    private Field _prefab = default;

    [SerializeField]
    private int _width = 8;
    [SerializeField]
    private int _heigh = 8;

    [SerializeField]
    private Material _blackMaterial = default;
    [SerializeField]
    private Transform _anchor = default;
    [SerializeField]
    private List<Field> _obj = new List<Field>();
    public List<Field> GetFieldElements() => _obj;

    private void Awake()
    {
        GameManager.Instance?.AddObjToDebugList(this);
        GameManager.Instance?.RegisterBoard(this);
        CreateMap();
    }

    public IConstant GetBoardWithTargetConstant(string constant)
    {
        for (int i = 0; i < _obj.Count; i++)
        {
            Field item = _obj[i];
            List<string> constantList = item.GetConstantsList();
            if (constantList != null && constantList.Contains(constant))
            {
                return item.GetComponent<IConstant>();
            }
        }
        return null;
    }

    public void CreateMap()
    {
        DestroyMap();
        InternCreateMap();
    }

    public void DestroyMap()
    {
        for (int i = _obj.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(_obj[i].gameObject);
        }
        _obj = new List<Field>();
    }

    public int GetDebugID()
    {
        return 1;
    }

    public void DebugModeChanged(bool isDebug)
    {
        SetDebugModeForField(isDebug);
    }


    private void SetDebugModeForField(bool value)
    {
        foreach (var field in _obj)
        {
            field.SetDebugMode(value);
        }
    }

    private void InternCreateMap()
    {
        for (int z = 0; z < _heigh; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                Field instance =  Instantiate(_prefab, new Vector3(z * 5, 0, x * -5), Quaternion.identity, _anchor);
                _obj.Add(instance);
                instance.Init(x, z);

                if(GameManager.Instance != null)
                {
                    instance.SetDebugMode(GameManager.Instance.IsDebugMode(GetDebugID()));
                }
   

                if(x % 2 == 0 && z % 2 != 0)
                {
                    instance.SetMaterial(_blackMaterial);
                }
                if (x % 2 != 0 && z % 2 == 0)
                {
                    instance.SetMaterial(_blackMaterial);
                }
            }
        }
    }
}
