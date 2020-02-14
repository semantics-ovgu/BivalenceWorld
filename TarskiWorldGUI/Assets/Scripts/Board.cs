using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab = default;

    [SerializeField]
    private int _width = 8;
    [SerializeField]
    private int _heigh = 8;

    [SerializeField]
    private Material _blackMaterial = default;
    [SerializeField]
    private Transform _anchor = default;
    [SerializeField, HideInInspector]
    private List<GameObject> _obj = new List<GameObject>();


    public void CreateMap()
    {
        DestroyMap();
        InternCreateMap();
    }

    public void DestroyMap()
    {
        for (int i = _obj.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(_obj[i]);
        }
        _obj = new List<GameObject>();
    }

    private void InternCreateMap()
    {
        for (int x = 0; x < _heigh; x++)
        {
            for (int z = 0; z < _width; z++)
            {
               var instance =  Instantiate(_prefab, new Vector3(x * 10, 0, z * 10), Quaternion.identity, _anchor);
                _obj.Add(instance);

                if(x % 2 == 0 && z % 2 != 0)
                {
                    instance.GetComponent<MeshRenderer>().material = _blackMaterial;
                }
                if (x % 2 != 0 && z % 2 == 0)
                {
                    instance.GetComponent<MeshRenderer>().material = _blackMaterial;
                }
            }
        }
    }

}
