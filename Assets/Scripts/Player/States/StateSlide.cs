using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSlide : StateBase
{
    public StateSlide(Player player) : base(player)
    {
    }

    public override void FixedTick()
    {
        player.UpdateLastInputDirection();
        player.WallSlideMovement();
        player.SpecialAbility();

        if (player.Controller.collisions.below)
        {
            AddState(new StateIdle(player), 2);
        } else
        {
            if (!player.Controller.collisions.right && !player.Controller.collisions.left)
            {
                AddState(new StateFalling(player), 2);
            }
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
