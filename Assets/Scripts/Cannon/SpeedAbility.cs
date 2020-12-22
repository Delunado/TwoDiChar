using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Speed Ability")]
public class SpeedAbility : AbilityType
{
    private bool usingAbility;
    private Timer abilityTimer;

    public override void UseAbility()
    {
        Debug.Log("Started Using Ability");
        usingAbility = true;
        abilityTimer.Restart();
    }

    public override void UpdateAbility()
    {
        if (usingAbility)
        {
            if (!abilityTimer.Finished)
            {
                abilityTimer.Update();
            }
            else
            {
                usingAbility = false;
                Debug.Log("Finished Using Ability");
            }
        }     
    }

    public override void InitAbility()
    {
        base.InitAbility();
        abilityTimer = new Timer(abilityDuration.value, Timer.TimerType.TIMER_FIXED_UPDATE);
        usingAbility = false;
    }
}
