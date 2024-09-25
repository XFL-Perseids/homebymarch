using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest{
    public QuestInfoSO info;
    public QuestState state;
    private int currentQuestStepIndex;

    public Quest(QuestInfoSO questInfo){
        this.info = questInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentQuestStepIndex = 0;
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
            questStep.InitializeQuestStep(info.id);
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
}