using UnityEngine;
using Unity;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UtilityAI;
using MyGame.Enemies;



class PlayerBasicAttack : MonoBehaviour {
    
    public Sensor enemyDetector;
    public Enemy enemy;

    [Header("UI Settings")]
    public Image attackButtonImage;

    void Attack(){

        (Transform target, float angle) = enemyDetector.GetClosestTarget("Enemy");
        Debug.Log("getting enemy");

        enemy = target.GetComponent<Enemy>();

        if (angle < 60 / 2f){
            // animation to turn towards target. set a delay?
        } 

    
        

        enemy.TakeDamage(5);
        Debug.Log("hit");
        
        //trigger cooldown here
    }
}