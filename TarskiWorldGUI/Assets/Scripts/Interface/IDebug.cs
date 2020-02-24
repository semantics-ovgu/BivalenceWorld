using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDebug
{
    int GetDebugID();
    void DebugModeChanged(bool isDebug);
}
