using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ClickButtonStep : QuestStep
{
[SerializeField] private Button button;


    void OnEnable(){
        button.onClick.AddListener(CompleteQuest);

    }

    void OnDisable(){
        button.onClick.RemoveListener(CompleteQuest);
    }



    protected override void SetQuestStepState(string state){
        ///
    }
}
