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
    private int stepIndex;

    public void InitializeQuestStep(string questId, int stepIndex, string questStepState){
        this.questId = questId;
        this.stepIndex = stepIndex;
        if (questStepState != null && questStepState != ""){
            SetQuestStepState(questStepState);
        }
    }

    public void CompleteQuest()
    {
        if (!isCompleted){
        isCompleted = true;
        GameEventsManager.instance.questEvents.AdvanceQuest(questId);
        Destroy(this.gameObject);
        }
    }

    protected void ChangeState(string newState, string newStatus){
        GameEventsManager.instance.questEvents.QuestStepStateChange(questId, stepIndex, new QuestStepState(newState, newStatus));
    }

    protected abstract void SetQuestStepState(string state);//override on specific quests + update state
}
