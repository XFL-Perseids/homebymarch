using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//quest points are the start/end point of a quest
//require a sphere collider here if player has to be close to the npc to trigger
//if it's a button, then add StartOrEndQuest to the OnPress
public class QuestPoint : MonoBehaviour{

    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool isStartPoint = true;
    [SerializeField] private bool isFinishPoint = true;
    private string questId;
    private QuestState currentQuestState;

    private void Awake(){
        questId = questInfoForPoint.id;
    }

    private void OnEnable(){
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        // add npc listener here
    }

    private void OnDisable(){
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        // remove npc listener here
    }
    

    private void StartOrEndQuest(){
        if (currentQuestState.Equals(QuestState.CAN_START) && isStartPoint){
            GameEventsManager.instance.questEvents.StartQuest(questId);
        } else if (currentQuestState.Equals(QuestState.CAN_FINISH) && isFinishPoint){
            GameEventsManager.instance.questEvents.FinishQuest(questId);
        }
    }



    private void QuestStateChange(Quest quest){
        if (quest.info.id.Equals(questId)){
            currentQuestState = quest.state;
        }
    }
}