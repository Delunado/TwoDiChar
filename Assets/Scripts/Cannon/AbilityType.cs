using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityType : ScriptableObject
{
    [SerializeField] FloatSO abilityDelay;
    public float AbilityDelay { get => abilityDelay.value; }

    [SerializeField] protected FloatSO abilityDuration;
    public float AbilityDuration { get => abilityDuration.value; }

    protected Player player;


    public abstract void UseAbility();

    public virtual void InitAbility()
    {
        player = FindObjectOfType<Player>();
    }

    public virtual void UpdateAbility()
    {

    }
}
