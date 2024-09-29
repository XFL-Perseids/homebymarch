using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class QuestLogUI : MonoBehaviour{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private QuestLogScrollingList scrollingList;
    [SerializeField] private TMP_Text questDisplayNameText;
    [SerializeField] private TMP_Text questStatusText;
    [SerializeField] private TMP_Text goldRewardText;
    [SerializeField] private TMP_Text experienceRewardText;
    // [SerializeField] private TMP_Text itemRewardText; // replace this with an item when you can

    [Header("Claim Button")]
    private GameObject claimQuestButtonPrefab;
    private Dictionary<string, ClaimQuestButton> idToClaimButtonMap = new Dictionary<string, ClaimQuestButton>();
    private Button defaultQuestButton;
    private ClaimQuestButton claimQuestButton;



    private void OnEnable(){
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;


    }

    private void OnDisable(){
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }
    
    private void QuestStateChange(Quest quest){
        QuestLogButton questLogButton = scrollingList.CreateScrollListButton(quest, () => {
            Debug.Log("h" + quest.info.displayName);
            SetQuestLogInfo(quest);
        });

        if (defaultQuestButton == null){
            defaultQuestButton = questLogButton.button;
        }
    }

    public void SetQuestLogInfo (Quest quest){
        questDisplayNameText.text = quest.info.displayName;
        // levelRequirementsText.text = "Required Level:" + quest.info.levelRequired;
        // questRequirementsText.text = "";

        // foreach(QuestInfoSO prerequesiteQuestInfo in quest.info.questPrerequesites){
        //     questRequirementsText.text += prerequesiteQuestInfo.displayName + "\n";
        // }

        goldRewardText.text = quest.info.goldReward.ToString();
        experienceRewardText.text = quest.info.experienceReward.ToString();

        //image for item rewards

        // claimQuestButton = CreateClaimQuestButton(quest, () => {
        //     //add logic to increase rewards here
        //     GameObject.Destroy(contentParent);
        // });



    }
    public bool doesClaimButtonExist(Quest quest){
        return idToClaimButtonMap.ContainsKey(quest.info.id);
    }

    private ClaimQuestButton InstantiateClaimQuestButton(Quest quest, UnityAction pointerClickAction){
        ClaimQuestButton claimQuestButton = Instantiate(claimQuestButtonPrefab, contentParent.transform).GetComponent<ClaimQuestButton>();

        claimQuestButton.gameObject.name = "claim_" + quest.info.id;
        claimQuestButton.Initialize(quest.info.displayName, pointerClickAction);

        idToClaimButtonMap[quest.info.id] = claimQuestButton;

        return claimQuestButton;

    }

    public ClaimQuestButton CreateClaimQuestButton(Quest quest, UnityAction pointerClickAction){

        ClaimQuestButton claimQuestButton = null;

        if(quest.state == QuestState.CAN_FINISH){
            claimQuestButton = InstantiateClaimQuestButton(quest, pointerClickAction);
        }

        return claimQuestButton;

    }

    
}