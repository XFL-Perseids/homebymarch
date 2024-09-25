using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveTotalStepCountQuestStep : QuestStep{




    private int totalStepsNeeded = 300;
    private int currentTotalSteps = 0;
    TrackerOfSteps stepTracker = new TrackerOfSteps();




    private void OnEnable(){
        GameEventsManager.instance.stepEvents.onStepAdded += stepAdded;
    }

    private void onDisable(){
        GameEventsManager.instance.stepEvents.onStepAdded -= stepAdded;
    }


    private void stepAdded(){ //every time player takes a step, this should be called to check if the player has completed
        if (stepTracker.overallSteps < totalStepsNeeded){
            currentTotalSteps = stepTracker.overallSteps;
        }
        if (stepTracker.overallSteps >= totalStepsNeeded){
            CompleteQuest();
        }
    }

}