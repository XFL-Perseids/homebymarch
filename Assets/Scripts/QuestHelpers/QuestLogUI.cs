using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestLogUI : MonoBehaviour{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private QuestLogScrollingList scrollingList;
    [SerializeField] private TMP_Text questDisplayNameText;
    [SerializeField] private TMP_Text questStatusText;
    [SerializeField] private TMP_Text goldRewardText;
    [SerializeField] private TMP_Text experienceRewardText;
    // [SerializeField] private TMP_Text itemRewardText; // replace this with an item when you can
    [SerializeField] private TMP_Text levelRequirementsText;
    [SerializeField] private TMP_Text questRequirementsText;

    private Button defaultQuestButton;


    private void onEnable(){
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }
    private void onDisable(){
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }
    private void QuestStateChange(Quest quest){
        QuestLogButton questLogButton = scrollingList.CreateButton(quest, () => {
            SetQuestLogInfo(quest);
        });

        if (defaultQuestButton == null){
            defaultQuestButton = questLogButton.button;
            defaultQuestButton.Select();
        }
    }

    private void SetQuestLogInfo (Quest quest){
        questDisplayNameText.text = quest.info.displayName;
        levelRequirementsText.text = "Required Level:" + quest.info.levelRequired;
        questRequirementsText.text = "";

        foreach(QuestInfoSO prerequesiteQuestInfo in quest.info.questPrerequesites){
            questRequirementsText.text += prerequesiteQuestInfo.displayName + "\n";
        }

        goldRewardText.text = quest.info.goldReward.ToString();
        experienceRewardText.text = quest.info.experienceReward.ToString();


    }

    
}