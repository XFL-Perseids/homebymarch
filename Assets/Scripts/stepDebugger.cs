using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.Android;
using UnityEngine.InputSystem;

public class TrackerOfSteps : MonoBehaviour
//THIS DOES WAY TOO MUCH STUFF REMIND ME TO REFACTOR THIS AT SPRINT'S END
{



  [SerializeField]TMP_Text messageText, DEBUGTEXT;

   long stepOffset;
   string stepJsonFilePath;
   private int numberOfSteps;
   private int loadedNumberOfSteps = 0;
   private int sessionNumberOfSteps;
   void Start()
   {
      messageText.text = "??????";
      sessionNumberOfSteps = 0;
       stepJsonFilePath = Application.persistentDataPath + "/stepData.json";
       if (Application.isEditor) { return; }
       RequestPermission(); 
       InputSystem.EnableDevice(StepCounter.current);
       messageText.text = "Now tracking steps. If you're still seeing this, Xyxar fucked up.";
       if (System.IO.File.Exists(stepJsonFilePath)){
        
        
        loadStepData();
         // loads the thing
        updateStepsText(); // sets counter to the loaded step count

       }
       
   }

   void Update()
   {
       if (Application.isEditor) { return; }
      sessionNumberOfSteps = StepCounter.current.stepCounter.ReadValue(); // 
      numberOfSteps = sessionNumberOfSteps; // not += because it would go from +1 to +2 etc instead of +1 constant per step
       if (stepOffset == 0)
       {
           stepOffset = sessionNumberOfSteps;
           DEBUGTEXT.text = ("Step offset " + stepOffset);
       }
       else
       {
           updateStepsText();
           saveStepData();
       }
   }

   void updateStepsText(){
    messageText.text = "Steps: " + (sessionNumberOfSteps + loadedNumberOfSteps).ToString();
   }

   void saveStepData(){
    StepData data = new StepData();
    data.numberOfSteps = numberOfSteps;
    string stepCountString = JsonUtility.ToJson(data);


    System.IO.File.WriteAllText(stepJsonFilePath, stepCountString);
   }

   void loadStepData(){
    string stringCountJson = System.IO.File.ReadAllText(stepJsonFilePath);

    loadedNumberOfSteps = JsonUtility.FromJson<StepData>(stringCountJson).numberOfSteps;
    updateStepsText();
   }


   async void RequestPermission()
   {
       #if UNITY_EDITOR
           DEBUGTEXT.text = ("Editor Platform");
       #endif
       #if UNITY_ANDROID
           AndroidRuntimePermissions.Permission stepTrackerResult = await AndroidRuntimePermissions.RequestPermissionAsync("android.permission.ACTIVITY_RECOGNITION");
           AndroidRuntimePermissions.Permission fileManagementResult = await AndroidRuntimePermissions.RequestPermissionAsync("android.permission.MANAGE_EXTERNAL_STORAGE");
           if (stepTrackerResult == AndroidRuntimePermissions.Permission.Granted & fileManagementResult == AndroidRuntimePermissions.Permission.Granted)
               DEBUGTEXT.text = ("Step offset amount " + stepOffset);
           else
            DEBUGTEXT.text = ("Permission state: " + stepTrackerResult);
       #endif
   }
}