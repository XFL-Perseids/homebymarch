using UnityEngine;
using System;
using System.Collections.Generic;

namespace UtilityAI{
    [RequireComponent(typeof(SphereCollider))]
    public class Sensor : MonoBehaviour{

        
        public float detectionRadius;
        public List<string> targetTags = new();
        SphereCollider sphereCollider;


        readonly List<Transform> detectedCreatures = new(10);
        
        void Start(){
            sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = 100;
        }

        void OnTriggerEnter(Collider other){
            ProcessTrigger(other, transform => detectedCreatures.Add(transform));

        }

        void OnTriggerExit(Collider other){
            ProcessTrigger(other, transform => detectedCreatures.Remove(transform));
        }

        void ProcessTrigger(Collider other, Action<Transform> action){
            if (other.CompareTag("Untagged")){
                return;
            }

            foreach (string t in targetTags){
                if(other.CompareTag(t)){
                    action(other.transform);
                }
            }
        }

        public (Transform, float) GetClosestTarget(string tag){

            if (detectedCreatures.Count == 0) {
                return (null, 0);
            }

            Transform closestTarget = null;
            float closestDistance = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            float angleToNearestTarget = 0;

            foreach (Transform potentialTarget in detectedCreatures){
                if(potentialTarget.CompareTag(tag)){
                    Vector3 directionToTarget = potentialTarget.position - currentPosition;
                    float distanceToTarget = directionToTarget.sqrMagnitude;
                    float angleToTarget = Vector3.Angle(directionToTarget, transform.forward);

                    if(distanceToTarget < closestDistance){
                        closestDistance = distanceToTarget;
                        closestTarget = potentialTarget;
                        angleToNearestTarget = angleToTarget;
                        
                    }
                }
            }

            return (closestTarget, angleToNearestTarget);


        }

    }
}