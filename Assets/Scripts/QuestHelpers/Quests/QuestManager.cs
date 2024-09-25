using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestManager : MonoBehaviour{
    private Dictionary<string, Quest> questMap;

    private int currentPlayerLevel = 1; // change to actual player's level
    private int currentTotalSteps;

    

    void Awake(){
        questMap = CreateQuestMap();

        Quest quest = GetQuestById("AchieveTotalStepCount");


    }

    private void OnEnable(){
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;

        
        //add a listener for player level thx
    }

    private void OnDisable(){
        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;

        
        //add a listener for player level thx
        
    }

    private void Start(){
        foreach (Quest quest in questMap.Values){
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }


    private bool areRequirementsMet(Quest quest){

        bool requirementsMet = true;

        if (currentPlayerLevel < quest.info.levelRequired){
            requirementsMet = false;
        }

        foreach(QuestInfoSO prerequesiteQuestInfo in quest.info.questPrerequesites){
            if (GetQuestById(prerequesiteQuestInfo.id).state != QuestState.FINISHED){
                requirementsMet = false;
            }
        }

        return requirementsMet;
    }

    void Update(){
        foreach(Quest quest in questMap.Values){
            if(quest.state == QuestState.REQUIREMENTS_NOT_MET && areRequirementsMet(quest)){
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private void ChangeQuestState(string id, QuestState state){
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.instance.questEvents.QuestStateChange(quest);
    }

    private void StartQuest(string id){
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
 
    }

    private void AdvanceQuest(string id){

        Quest quest = GetQuestById(id);

        if(quest.CurrentQuestStepExists()){
            quest.InstantiateCurrentQuestStep(this.transform);
        }

        else{
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
        
    }

    private void FinishQuest(string id){
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);

    }

    private void ClaimRewards(Quest quest){
        Debug.Log("wow ! you are did it!");
    }
    private Dictionary<string, Quest> CreateQuestMap(){
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        foreach (QuestInfoSO questInfo in allQuests){
            if(idToQuestMap.ContainsKey(questInfo.id)){
                Debug.LogWarning("duplicate id found:" + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }

        return idToQuestMap;
    }

    private Quest GetQuestById(string id){
        Quest quest = questMap[id];
        
        if(quest == null){
            Debug.LogError("ID not found:" + id);

        }
        return quest;
    }
}