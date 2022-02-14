using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    public DeathState(StateAgent owner, string name) : base(owner, name) { }

    public override void OnEnter()
    {
        owner.movement.Stop();
        owner.animator.SetTrigger("death");
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}
