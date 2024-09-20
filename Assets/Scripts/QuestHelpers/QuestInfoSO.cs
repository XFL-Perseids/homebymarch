using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObjects/QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject{
    [SerializeField]public string id {get; private set;}

    [Header("General")]
    public string displayName;

    [Header("Requirements")]
    public int levelRequired;
    public int totalStepsRequired;
    public List<QuestInfoSO> questPrerequesites = new();

    [Header("Steps")]
    public List<GameObject> questStepPrefabs = new();

    [Header("Rewards")]
    public int goldReward;
    public int experienceReward;
    // public Item itemReward; 
    // add item rewards when the inventory is implemented

}