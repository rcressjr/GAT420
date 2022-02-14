using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IdleState
{
    public ChaseState(StateAgent owner, string name) : base(owner, name) { }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        owner.movement.MoveTowards(owner.enemy.transform.position);
    }
}
