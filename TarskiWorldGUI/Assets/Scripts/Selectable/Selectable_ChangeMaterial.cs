using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable_ChangeMaterial : Selectable_ABhvr
{
    [SerializeField]
    private MeshRenderer _renderer = default;
    [SerializeField]
    private Material _selectedMaterial = default;

    private Material _normalMaterial = default;

    protected override void OnAwake()
    {
        base.OnAwake();

    }

    protected override void StartSelectedListener(SelectableObject.SelectableArgs arg0)
    {
	    base.StartHoveredListener(arg0);
        _normalMaterial = _renderer.material;
        SetMaterialToRenderer(_selectedMaterial);
    }
    protected override void EndSelectedListener(SelectableObject.SelectableArgs arg0)
    {

        base.EndHoveredListener(arg0);
        SetMaterialToRenderer(_normalMaterial);
    }

    private void SetMaterialToRenderer(Material material)
    {
        _renderer.material = material;
    }
}
