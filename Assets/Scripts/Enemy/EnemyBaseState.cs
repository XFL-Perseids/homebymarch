using UnityEngine;


namespace HomeByMarch{
    public abstract class EnemyBaseState :IState{
        
        protected readonly Enemy enemy;
        protected readonly Animator animator;
        protected const float crossFadeDuration = 0.1f;
       
        protected EnemyBaseState(Enemy enemy, Animator animator){
            this.enemy = enemy;
            this.animator = animator;
        }
        public virtual void OnEnter(){
            
        }
        public virtual void Update(){
            
        }
        public virtual void FixedUpdate(){
            
        }
        public virtual void OnExit(){
            
        }
    }
}