using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UtilityAI{
    [RequireComponent(typeof(NavMeshAgent), typeof(Sensor))]
    public class Brain : MonoBehaviour{
        public List<Action> actions;
        public Context context;

        void Awake(){
            context = new Context(this);

            foreach (var action in actions){
                action.Initialize(context);
            }
        }

        void Update(){
            // UpdateContext();

            Action bestAction = null;
            float highestUtility = float.MinValue;

            foreach(var action in actions){
                float utility = action.CalculateUtility(context);

                if(utility > highestUtility){
                    highestUtility = utility;
                    bestAction = action;
                }
            }

            // if (bestAction != null){
            //     bestAction.execute(context);
            // }
        }

        // void UpdateContext{

        // }
    }
}