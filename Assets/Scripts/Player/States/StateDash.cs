using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDash : StateBase
{
    int dashCountAux;

    public StateDash(Player player) : base(player)
    {
    }

    public override void FixedTick()
    {
        player.UpdateLastInputDirection();

        if (dashCountAux > 0)
        {
            player.DashImpulse();
            dashCountAux--;

            if (player.CheckWallSlide())
            {
                AddState(new StateSlide(player), 2);
                dashCountAux = 0;
            }
        } else
        {
            player.MovementGround();

            if (player.Controller.collisions.below)
            {
                if (player.CheckHorizontalInput())
                    AddState(new StateWalking(player), 1);
                else
                    AddState(new StateIdle(player), 1);
            } else
            {
                AddState(new StateFalling(player), 2);
            }

            if (player.CheckWallSlide())
            {
                AddState(new StateSlide(player), 3);
            }
        }

        player.SetState(stateQueue);
    }

    public override void Tick()
    {
        
    }

    public override void OnStateEnter()
    {
        dashCountAux = player.Stats.DashCount;
        player.ResetVelocity();
        player.DashUsed();
    }
}
