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
    private bool isCompleted;

    public QuestStep(string name, string desc)
    {
        questName = name;
        description = desc;
        isCompleted = false;
    }

    public void CompleteQuest()
    {
        if (!isCompleted){
        isCompleted = true;
        Destroy(this.gameObject);
        }
    }
}
