using UnityEngine;
using Unity;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


class PlayerBasicAttack : MonoBehaviour {
    
    public GameObject closestEnemy;
    private EnemyDetector enemyDetector;

    private PlayerData playerData;
    public int attackRange = playerData.attackRange;// adjust if needed

    

    void Attack(){

        closestEnemy = enemyDetector.closestEnemy;
        if (enemyDetector.nearestDistance <= attackRange){
            ///animations
            closestEnemy.TakeDamage(playerData.attack);
            Debug.Log("hit");
        }
        

    }
}