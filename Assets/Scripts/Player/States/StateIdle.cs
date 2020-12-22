using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : StateBase
{
    public StateIdle(Player player) : base(player)
    {
    }

    public override void FixedTick()
    {
        player.UpdateLastInputDirection();
        player.MovementGround();
        player.Aiming(CannonBase.AimingMode.AIMING_GROUND);
        player.SpecialAbility();

        if (player.Controller.collisions.below && player.CheckHorizontalInput())
        {
            AddState(new StateWalking(player), 3);
        }

        if (!player.Controller.collisions.below)
        {
            AddState(new StateFalling(player), 2);
        }

        if (player.CheckDash())
        {
            AddState(new StateDash(player), 1);
        }

        player.SetState(stateQueue);
    }

    public override void Tick()
    {
        
    }
}
