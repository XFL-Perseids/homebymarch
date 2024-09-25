using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest{
    public QuestInfoSO info;
    public QuestState state;
    private int currentQuestStepIndex;
    private QuestStepState[] questStepStates;

    public Quest(QuestInfoSO questInfo){
        this.info = questInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentQuestStepIndex = 0;
        this.questStepStates = new QuestStepState[info.questStepPrefabs.Length];

        for(int i = 0; i < questStepStates.Length; i++){
            questStepStates[i] = new QuestStepState();
        }
    }

    public void MoveToNextStep(){

        currentQuestStepIndex++;

    }

    public bool CurrentQuestStepExists(){
        return (currentQuestStepIndex < info.questStepPrefabs.Count);
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform){
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null){
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(info.id, currentQuestStepIndex);
        }
    }

    private GameObject GetCurrentQuestStepPrefab(){
        GameObject questStepPrefab = null;

        if (CurrentQuestStepExists()){
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        }
        else {
            Debug.LogWarning("Step doesnt exist");
        }
        return questStepPrefab;
    }

    public void StoreQuestStepState(QuestStepState questStepState, int stepIndex){
        if (stepIndex < questStepState.Length){
            questStepStates[stepIndex].state = questStepState.state;
        } else{
            Debug.Log("out of range");  
        }
    }

    public QuestData GetQuestData(){
        return new QuestData(state, currentQuestStepIndex, questStepStates);
    }
}