using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "TarskiWorld/Predicate")]
public class Predicate : ScriptableObject
{
    public string PredicateIdentifier = "";
    public GameObject Prefab;
}
