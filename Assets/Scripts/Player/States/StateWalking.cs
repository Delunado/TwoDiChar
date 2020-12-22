using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWalking : StateBase
{
    public StateWalking(Player player) : base(player)
    {
    }

    public override void FixedTick()
    {
        player.UpdateLastInputDirection();
        player.MovementGround();
        player.Aiming(CannonBase.AimingMode.AIMING_GROUND);
        player.SpecialAbility();

        if (!player.CheckHorizontalInput())
        {
            AddState(new StateIdle(player), 4);
        }

        if (!player.Controller.collisions.below)
        {
            AddState(new StateFalling(player), 3);
        }

        if (player.CheckWallSlide())
        {
            AddState(new StateSlide(player), 2);
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
