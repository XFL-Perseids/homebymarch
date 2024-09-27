﻿using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace HomeByMarch
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDetector))]
    public class Enemy : Entity
    {

        [SerializeField] PlayerDetector playerDetector;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] float timeBetweenAttack =1f;

        [SerializeField] float wanderRadius = 10f;
        private StateMachine stateMachine;

        CountdownTimer attackTimer;
        void Start()
        {
            attackTimer = new CountdownTimer(timeBetweenAttack);
            stateMachine = new StateMachine();
    
            var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
            var chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player);
            var attackState = new EnemyAttackState(this, animator, agent,playerDetector.Player);
            // Initialize other components if necessary
            At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
            At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
            At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));
             

            stateMachine.SetState(wanderState);
        }



        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

        void Update()
        {
            stateMachine.Update();
            attackTimer.Tick(Time.deltaTime);
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        public void Attack(){
            if(attackTimer.IsRunning) return;
            attackTimer.Start();
            Debug.Log("Attacking");
        }
    }
}
