using Unity;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;


[System.Serializable]
public abstract class QuestStep : MonoBehaviour
{
    public string questName;
    public string description;
    private bool isCompleted = false;
    private string questId;

    public void InitializeQuestStep(string questId){
        this.questId = questId;
    }

    public void CompleteQuest()
    {
        if (!isCompleted){
        isCompleted = true;
        GameEventsManager.instance.questEvents.AdvanceQuest(questId);
        Destroy(this.gameObject);
        }
    }
}
