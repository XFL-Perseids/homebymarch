using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;

public class QuestLogScrollingList: MonoBehaviour{

    [Header("Components")]
    [SerializeField] private GameObject contentParent;

    [Header("Quest Log Button")]
    [SerializeField] private GameObject questLogButtonPrefab;


    private Dictionary<string, QuestLogButton> idToButtonMap = new Dictionary<string, QuestLogButton>();


    public bool doesButtonExist(Quest quest){
        return idToButtonMap.ContainsKey(quest.info.id);
    }

    public bool isQuestViewable(Quest quest){
        return !(quest.state == QuestState.REQUIREMENTS_NOT_MET | quest.state == QuestState.FINISHED);
    }

    public QuestLogButton CreateScrollListButton(Quest quest, UnityAction selectAction){

        QuestLogButton questLogButton = null;
        if (!doesButtonExist(quest) && isQuestViewable(quest)){
            Debug.Log("ok button should be instantiated here" + quest.info.id);
            questLogButton = InstantiateQuestLogButton(quest, selectAction);
        } else {
            if(isQuestViewable(quest)){
            Debug.Log("" + quest.info.id);
            questLogButton = idToButtonMap[quest.info.id];
            //should only show quests you can view (met requirements, in progress, and claimable)
            }
        }
        return questLogButton;
    }
    private QuestLogButton InstantiateQuestLogButton(Quest quest, UnityAction selectAction){
        QuestLogButton questLogButton = Instantiate(questLogButtonPrefab, contentParent.transform).GetComponent<QuestLogButton>();

        questLogButton.gameObject.name = quest.info.id + "_button";
        questLogButton.Initialize(quest.info.displayName, selectAction);

        idToButtonMap[quest.info.id] = questLogButton;

        return questLogButton;

    }
}