using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable_SpawnParticle : Selectable_ABhvr
{
    [SerializeField]
    private ParticleSystem _particleSystemPrefab = default;
    private ParticleSystem _currentParticleSystem = default;


    protected override void StartSelectedListener(SelectableObject.SelectableArgs arg0)
    {
        base.StartHoveredListener(arg0);

        _currentParticleSystem = Instantiate(_particleSystemPrefab, this.transform);


    }
    protected override void EndSelectedListener(SelectableObject.SelectableArgs arg0)
    {
        base.EndHoveredListener(arg0);
        Destroy(_currentParticleSystem.gameObject);
    }
}
