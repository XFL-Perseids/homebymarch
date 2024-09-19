using UnityEngine;

namespace UtilityAI{
    public abstract class Action : ScriptableObject{
        public string targetTag;
        public Consideration consideration;

        public virtual void Initialize(Context context){

        }

        public float CalculateUtility(Context context) => consideration.Evaluate(context);

        public abstract void Execute(Context context);
    }
}