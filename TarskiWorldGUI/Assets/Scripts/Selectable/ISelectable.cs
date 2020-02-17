using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    string GetDebugInformation();
    void StartHover();
    void EndHover();
    void Selectable();
    void Deselectable();
    GameObject GetRootObj();
}
