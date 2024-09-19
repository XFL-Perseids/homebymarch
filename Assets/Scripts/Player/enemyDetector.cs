using Unity;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Enemies;
using MyGame.Player;
using UtilityAI;



namespace MyGame.Player{
    [RequireComponent(typeof(SphereCollider))]

    public class EnemyDetector : Sensor{
        public PlayerData playerData;
        



        readonly List<Transform> detectedCreatures = new(10);
        
        void Start(){


            detectionRadius = playerData.attackRange;
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