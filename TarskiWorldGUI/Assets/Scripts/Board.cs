using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
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

    private void InternCreateMap()
    {
        for (int x = 0; x < _heigh; x++)
        {
            for (int z = 0; z < _width; z++)
            {
                Field instance =  Instantiate(_prefab, new Vector3(x * 5, 0, z * -5), Quaternion.identity, _anchor);
                _obj.Add(instance);
                instance.Init(x, z);

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
