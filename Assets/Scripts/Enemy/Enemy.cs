using UnityEngine;
using UnityEngine.AI;

namespace HomeByMarch
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDetector))]
    public class Enemy : Entity
    {

        [SerializeField] PlayerDetector playerDetector;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;

        [SerializeField] float wanderRadius = 10f;
        private StateMachine stateMachine;
        void Start()
        {
            stateMachine = new StateMachine();
    
            var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
            var chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player);
            // Initialize other components if necessary
            At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));

            stateMachine.SetState(wanderState);
        }



        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

        void Update()
        {
            stateMachine.Update();
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }
    }
}
