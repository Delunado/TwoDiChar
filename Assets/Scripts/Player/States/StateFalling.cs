using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFalling : StateBase
{
    public StateFalling(Player player) : base(player)
    {
    }

    public override void FixedTick()
    {
        player.UpdateLastInputDirection();
        player.MovementGround();
        player.Aiming(CannonBase.AimingMode.AIMING_AIR);
        player.SpecialAbility();

        if (player.Controller.collisions.below)
        {
            if (player.CheckHorizontalInput())
                AddState(new StateWalking(player), 2);
            else
                AddState(new StateIdle(player), 2);
        }

        if (player.CheckWallSlide())
        {
            AddState(new StateSlide(player), 3);
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
