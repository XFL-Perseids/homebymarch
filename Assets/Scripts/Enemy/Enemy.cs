using UnityEngine;
using UnityEngine.AI;

namespace HomeByMarch
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : Entity
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;

        private StateMachine stateMachine;

        void OnValidate() => ValidateRefs();

        void Start()    
        {
            stateMachine = new StateMachine();
            // Initialize other components if necessary
        }

        private void ValidateRefs()
        {
            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }

            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            // Optional: Log warnings if any component is missing
            if (agent == null)
            {
                Debug.LogWarning("NavMeshAgent component is missing on " + gameObject.name);
            }
            if (animator == null)
            {
                Debug.LogWarning("Animator component is missing on " + gameObject.name);
            }
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
