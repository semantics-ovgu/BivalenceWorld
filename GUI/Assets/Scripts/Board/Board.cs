using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using UnityEngine;

public class Board : MonoBehaviour, IDebug
{
    [SerializeField]
    private Field _prefab = default;

    [SerializeField]
    private int _width = 8;
    [SerializeField]
    private int _heigh = 8;

    private bool _hasUnsavedChanges = false;
    public bool HasUnsavedChanges => _hasUnsavedChanges;

    [SerializeField]
    private Transform _camPos = default;
    [SerializeField]
    private Transform _cam2DPos = default;
    public Transform CamPos => _camPos;
    public Transform Cam2DPos => _cam2DPos;

    private string _savePath = "";
    public string SavePath => _savePath;

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
        _hasUnsavedChanges = true;
        CreateMap();
    }

    public string GetName()
    {
        if (string.IsNullOrEmpty(_savePath))
        {
            return "Unsaved World";
        }
        else
        {
            return Path.GetFileNameWithoutExtension(_savePath);
        }
    }

    public void SetSavePath(string path)
    {
        _savePath = path;
        _hasUnsavedChanges = false;
    }

    public void SetUnsavedChanges()
    {
        _hasUnsavedChanges = true;
    }

    public Field GetFieldFromCoord(int x, int z)
    {
        foreach (var item in _obj)
        {
            if (item.GetX() == x && item.GetZ() == z)
            {
                return item;
            }
        }

        return null;
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

    public void RotateMap(bool clockwise = true)
    {
        List<PredicateContainer> predicates = new List<PredicateContainer>();
        foreach (var obj in _obj)
        {
            if (obj.HasPredicateInstance())
            {
                var container = new PredicateContainer
                {
                    Predicate = obj.GetPredicateInstance(),
                    X = obj.GetX(),
                    Y = obj.GetZ()
                };

                if (clockwise)
                {
                    container.RotateClockwise(_width, _heigh);
                }
                else
                {
                    container.RotateCounterClockwise(_width, _heigh);
                }
                predicates.Add(container);

                obj.ResetPredicate();
            }
        }

        foreach (var obj in predicates)
        {
            var field = GetFieldFromCoord(obj.X, obj.Y);
            field.AddPredicateObj(obj.Predicate);
            obj.Predicate.transform.position = new Vector3(field.transform.position.x, 0, field.transform.position.z);
            GameManager.Instance.GetSelectionManager().ResetSelection();

        }
    }

    private class PredicateContainer
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PredicateObj Predicate { get; set; }

        public void RotateClockwise(int width, int height)
        {
            (int x, int y) origin = (X, Y);
            X = origin.y;
            Y = height - origin.x - 1;
        }

        public void RotateCounterClockwise(int width, int height)
        {
            (int x, int y) origin = (X, Y);
            X = width - origin.y - 1;
            Y = origin.x;
        }
    }

    public void DestroyMap()
    {
        for (int i = _obj.Count - 1; i >= 0; i--)
        {
            var field = _obj[i];
            if (field.HasPredicateInstance())
            {
                Destroy(field.GetPredicateInstance().gameObject);
            }
            DestroyImmediate(field.gameObject);
        }
        _obj = new List<Field>();
    }

    public int GetDebugID()
    {
        return 1;
    }

    public bool IsConstantUsed(string constant)
    {
        return _obj.Any(o => o?.GetPredicateInstance()?.GetConstant().Contains(constant) ?? false);
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
                Field instance = Instantiate(_prefab, new Vector3(z * 5, 0, x * -5), Quaternion.identity, _anchor);
                _obj.Add(instance);
                instance.Init(x, z);

                if (GameManager.Instance != null)
                {
                    instance.SetDebugMode(GameManager.Instance.IsDebugMode(GetDebugID()));
                }

                if (x % 2 == 0 && z % 2 != 0)
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
