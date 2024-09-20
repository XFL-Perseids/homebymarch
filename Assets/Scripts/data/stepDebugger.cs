using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrackerOfSteps : MonoBehaviour
{
    [SerializeField] TMP_Text dailyStepsText, overallStepsText, DEBUGTEXT;

    private long stepOffset = 0;
    private int numberOfSteps = 0;
    private int dailySteps = 0;
    private int overallSteps = 0;
    private DateTime lastTrackedDate;

    private SaveDataManager saveDataManager;

    void Awake()
    {
        // Initialize SaveDataManager
        saveDataManager = FindObjectOfType<SaveDataManager>();

        if (saveDataManager == null)
        {
            DEBUGTEXT.text = "SaveDataManager is missing.";
            Debug.LogError("SaveDataManager not found.");
        }
    }

    void Start()
    {
        dailyStepsText.text = "Daily Steps: 0";
        overallStepsText.text = "Overall Steps: 0";
        DEBUGTEXT.text = "Initializing step tracker...";

        if (Application.isEditor) { return; }

        RequestPermission();
        InputSystem.EnableDevice(StepCounter.current);

        LoadStepData();
        CheckNewDay();
        UpdateSteps();
    }

    void OnEnable()
    {
        if (Application.isEditor) { return; }

        LoadStepData();  // Reload step data when the script is enabled
        UpdateSteps();   // Update the displayed step counts
    }

    void Update()
    {
        if (Application.isEditor) { return; }

        if (StepCounter.current != null)
        {
            numberOfSteps = StepCounter.current.stepCounter.ReadValue(); // Read step count from StepCounter

            if (stepOffset == 0)
            {
                stepOffset = numberOfSteps;
                DEBUGTEXT.text = "Step offset: " + stepOffset;
            }
            else
            {
                // Update step counts
                UpdateSteps();
                SaveStepData();
            }
        }
    }

    void CheckNewDay()
    {
        DateTime currentDate = DateTime.Now;
        if (currentDate.Date != lastTrackedDate.Date)
        {
            dailySteps = 0; // Reset daily steps on a new day
            lastTrackedDate = currentDate; // Update last tracked date
            SaveStepData();
        }
    }

    void UpdateSteps()
    {
        // Update daily and overall step counts based on loaded data
        int newSteps = numberOfSteps - (int)stepOffset;
        if (newSteps > 0)
        {
            dailySteps += newSteps;
            overallSteps += newSteps;

            // Update the step offset to the current number of steps
            stepOffset = numberOfSteps;
        }

        // Update UI text for daily and overall step counts
        dailyStepsText.text = "Daily Stepss: " + dailySteps;
        overallStepsText.text = "Overall Steps: " + overallSteps;
    }

    void SaveStepData()
    {
        PlayerData data = new PlayerData
        {
            dailySteps = dailySteps,
            overallSteps = overallSteps,
            lastTrackedDate = lastTrackedDate.ToString()
        };

        saveDataManager.SaveStepData(data);  // Use SaveDataManager to save
    }

    void LoadStepData()
    {
        PlayerData data = saveDataManager.LoadStepData();  // Use SaveDataManager to load

        dailySteps = data.dailySteps;
        overallSteps = data.overallSteps;
        lastTrackedDate = DateTime.Parse(data.lastTrackedDate);
    }

   async void RequestPermission()
{
    #if UNITY_EDITOR
        DEBUGTEXT.text = "Editor Platform - No permissions needed.";
    #else
        // Request permissions for Android asynchronously
        AndroidRuntimePermissions.Permission stepTrackerResult = await AndroidRuntimePermissions.RequestPermissionAsync("android.permission.ACTIVITY_RECOGNITION");
        AndroidRuntimePermissions.Permission fileManagementResult = await AndroidRuntimePermissions.RequestPermissionAsync("android.permission.MANAGE_EXTERNAL_STORAGE");
        AndroidRuntimePermissions.Permission fileManagementWriteResult = await AndroidRuntimePermissions.RequestPermissionAsync("android.permission.WRITE_EXTERNAL_STORAGE");

        if (stepTrackerResult == AndroidRuntimePermissions.Permission.Granted && fileManagementResult == AndroidRuntimePermissions.Permission.Granted)
        {
            DEBUGTEXT.text = "Permissions granted.";
        }
        else
        {
            DEBUGTEXT.text = "Permission denied: " + stepTrackerResult;
        }
    #endif
}

}
