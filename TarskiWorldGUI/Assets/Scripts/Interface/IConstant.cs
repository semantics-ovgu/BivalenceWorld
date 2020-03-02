using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConstant
{
    List<string> GetConstantsList();
    void AddConstant(string predicate);
    void RemoveConstant(string constant);
}