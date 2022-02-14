using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAgent : Agent
{
    [SerializeField] public Perception perception;

    public PathFollower pathFollower;
    public StateMachine stateMachine = new StateMachine();

    public BoolRef enemySeen;
    public FloatRef enemyDistance;
    public FloatRef health;
    public FloatRef timer;

    public GameObject enemy { get; set; }

    void Start()
    {
        stateMachine.AddState(new IdleState(this, typeof(IdleState).Name));
        stateMachine.AddState(new PatrolState(this, typeof(PatrolState).Name));
        stateMachine.AddState(new ChaseState(this, typeof(ChaseState).Name));
        stateMachine.AddState(new DeathState(this, typeof(DeathState).Name));
        stateMachine.AddState(new AttackState(this, typeof(AttackState).Name));
        stateMachine.AddState(new EvadeState(this, typeof(EvadeState).Name));
        
        stateMachine.AddTransition(typeof(IdleState).Name, new Transition(new Condition[] { new BoolCondition(enemySeen, true) }), typeof(ChaseState).Name);
        stateMachine.AddTransition(typeof(IdleState).Name, new Transition(new Condition[] { new FloatCondition(timer, Condition.Predicate.LESS, 0) }), typeof(PatrolState).Name);
        stateMachine.AddTransition(typeof(IdleState).Name, new Transition(new Condition[] { new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 0) }), typeof(DeathState).Name);
        
        stateMachine.AddTransition(typeof(PatrolState).Name, new Transition(new Condition[] { new BoolCondition(enemySeen, true) }), typeof(ChaseState).Name);
        stateMachine.AddTransition(typeof(PatrolState).Name, new Transition(new Condition[] { new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 0) }), typeof(DeathState).Name);
        
        stateMachine.AddTransition(typeof(ChaseState).Name, new Transition(new Condition[] { new BoolCondition(enemySeen, false) }), typeof(IdleState).Name);
        stateMachine.AddTransition(typeof(ChaseState).Name, new Transition(new Condition[] { new BoolCondition(enemySeen, false) }), typeof(AttackState).Name);
        stateMachine.AddTransition(typeof(ChaseState).Name, new Transition(new Condition[] { new FloatCondition(enemyDistance, Condition.Predicate.LESS_EQUAL, 2) }), typeof(AttackState).Name);
        stateMachine.AddTransition(typeof(ChaseState).Name, new Transition(new Condition[] { new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 30) }), typeof(EvadeState).Name);
        
        stateMachine.AddTransition(typeof(AttackState).Name, new Transition(new Condition[] { new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 0) }), typeof(DeathState).Name);
        stateMachine.AddTransition(typeof(AttackState).Name, new Transition(new Condition[] { new FloatCondition(timer, Condition.Predicate.LESS_EQUAL, 0) }), typeof(ChaseState).Name);
        stateMachine.AddTransition(typeof(AttackState).Name, new Transition(new Condition[] { new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 30) }), typeof(EvadeState).Name);
        
        stateMachine.AddTransition(typeof(EvadeState).Name, new Transition(new Condition[] { new BoolCondition(enemySeen, false) }), typeof(IdleState).Name);

        stateMachine.SetState(stateMachine.StateFromName("idle"));
    }

    void Update()
    {
        var enemies = perception.GetGameObjects();
        enemySeen.value = (enemies.Length != 0);
        enemy = (enemies.Length != 0) ? enemies[0] : null;
        timer.value -= Time.deltaTime;
        enemyDistance.value = (enemy != null) ? (Vector3.Distance(transform.position, enemy.transform.position)) : float.MaxValue;

        stateMachine.Update();

        animator.SetFloat("speed", movement.velocity.magnitude);
    }

    private void OnGUI()
    {

    }
}
