/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovement : Movement
{
    public override Vector3 velocity { get => navMeshAgent.velocity; set => navMeshAgent.velocity = value; }

    NavMeshAgent navMeshAgent;

    public Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public Update()
    {
        navMeshAgent.speed = movementdata.maxSpeed;
        navMeshAgent.acceleration = movementdata.maxForce;
        navMeshAgent.angularSpeed = movementdata.turnRate;
    }

    public override ApplyForce(Vector3 force)
    {
        return force;
    }

    public override void MoveTowards(Vector3 target)
    {
        return navMeshAgent.SetDestination(target);
    }

    public override void resume()
    {
        return navMeshAgent.isStopped = false;
    }

    public override void Stop()
    {
        return navMeshAgent.isStopped = true;
    }
}
*/