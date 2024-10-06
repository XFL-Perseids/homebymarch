using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UserLevel : MonoBehaviour{

    [SerializeField]
    public TMP_Text levelText;
    public TMP_Text currentStepCountText;
    public TMP_Text totalStepsForNextLevelText;
    public TMP_Text remainingStepsForNextLevelText;

    [Header("ui stuff")]
    public Image experienceBarImage;



    public int currentUserLevel;
    public int totalStepsForNextLevel;
    public int currentStepCount;
    private string stepJsonFilePath;
    public int remainingStepsForNextLevel;
    private string stepCountData;


    void Awake(){
        stepJsonFilePath = Application.persistentDataPath + "/stepData.json";
        stepCountData = System.IO.File.ReadAllText(stepJsonFilePath);
        currentStepCount = JsonUtility.FromJson<StepData>(stepCountData).numberOfSteps;

        
        currentUserLevel = CalculateUserLevel(currentStepCount);
        totalStepsForNextLevel = CalculateTotalStepsForLevel(currentUserLevel);
    }

    void Update(){

        currentUserLevel = CalculateUserLevel(currentStepCount);
        totalStepsForNextLevel = CalculateTotalStepsForLevel(currentUserLevel + 1);
        currentStepCount = JsonUtility.FromJson<StepData>(stepCountData).numberOfSteps;
        remainingStepsForNextLevel = totalStepsForNextLevel - currentStepCount;
        UpdateText();
        UpdateExperienceBar();
        

        


    }

    public int CalculateUserLevel(int stepCount){
        



            


        return Mathf.FloorToInt(Mathf.Pow((stepCount / 100), (1/2.35f))) + 1;



    }

    public int CalculateTotalStepsForLevel(int level){

        return Mathf.FloorToInt(100 * Mathf.Pow(level - 1, 2.35f));

    }

    void UpdateText(){

        levelText.text = currentUserLevel.ToString();
        currentStepCountText.text = "Total steps: " + currentStepCount.ToString();
        totalStepsForNextLevelText.text = "Walk a total of " + ReformatIntToText(totalStepsForNextLevel) + " steps to advance to Level " + (currentUserLevel + 1);
        

    }

    public string ReformatIntToText(int number){

        if (number >= 1000){
            return "" + Mathf.Floor(number / 1000) + "K";
        } else {
            return number.ToString();
        }



    }

    // floor(100[(x-1)^2.35])

    public void UpdateExperienceBar(){
        int totalStepsForPreviousLevel = CalculateTotalStepsForLevel(currentUserLevel);

        int differenceInSteps =  totalStepsForNextLevel - totalStepsForPreviousLevel;
        

        float fillAmount = (float)(differenceInSteps - remainingStepsForNextLevel) / differenceInSteps;
        Debug.Log(fillAmount);
        Debug.Log("remaining steps: " + remainingStepsForNextLevel);
        Debug.Log("difference: " + differenceInSteps);

        if (experienceBarImage != null){
            experienceBarImage.fillAmount = fillAmount;
        }

    }


    

    


    


}