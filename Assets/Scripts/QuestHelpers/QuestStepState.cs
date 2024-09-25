using Unity;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class QuestStepState{
    public string state;

    public QuestStepState(string state){
        this.state = state;
    }

    public QuestStepState(){
        this.state = "";
    }

}