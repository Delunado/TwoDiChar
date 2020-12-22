using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public abstract class StateBase
{
    protected Player player;
    protected SimplePriorityQueue<StateBase, int> stateQueue;

    public abstract void Tick();
    public abstract void FixedTick();

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public StateBase(Player player)
    {
        stateQueue = new SimplePriorityQueue<StateBase, int>();
        this.player = player;
    }

    protected void AddState(StateBase state, int priority)
    {
        if (state != null)
            stateQueue.Enqueue(state, priority);
    }
}
