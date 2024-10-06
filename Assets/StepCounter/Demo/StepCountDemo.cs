using Repforge.StepCounterPro;
using System;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.Android;
using UnityEngine.InputSystem;

namespace HomeByMarch {
    [Serializable]
    public class StepData // Ensure this class is Serializable
    {
        public int numberOfSteps; // Steps for today
        public int dailySteps; // Daily steps count
        public int overallSteps; // Overall steps count
        public string lastSavedDate; // Store the last saved date to check if a new day has started
    }

    public class StepCountDemo : MonoBehaviour
    {
        public TMP_Text text, debugText;
        public Canvas permissionCanvas;
        int dailyStepCount;
        int overallStepCount; // Track overall steps
        [SerializeField] FloatEventChannel stepCountChannel; // Added FloatEventChannel
        string stepJsonFilePath;

        void OnEnable()
        {
            StepsToday();
            Debug.Log("steps today: " + dailyStepCount);
        }

        void Start()
        {
            InitializeStepCounter();
            LoadStepData(); // Load existing step data
            RequestPermission();
            StepCounterRequest permissionRequest = new StepCounterRequest();
            permissionRequest
                .OnPermissionGranted(OnPermissionGranted)
                .OnPermissionDenied(OnPermissionDenied)
                .RequestPermission();
        }

        private void OnPermissionGranted()
        {
            StepsToday(); // Call this to get today's steps immediately after permission is granted.
        }

        void InitializeStepCounter()
        {
            stepJsonFilePath = Application.persistentDataPath + "/stepData.json";
            debugText.text = stepJsonFilePath;
        }

        private void LoadStepData()
        {
            if (File.Exists(stepJsonFilePath))
            {
                string jsonString = File.ReadAllText(stepJsonFilePath);
                StepData data = JsonUtility.FromJson<StepData>(jsonString);
                
                // Check if the saved date is today
                if (data.lastSavedDate != DateTime.Today.ToString("yyyy-MM-dd"))
                {
                    // It's a new day; reset daily step count
                    dailyStepCount = 0; // Reset daily step count for the new day
                }
                else
                {
                    // Load today's and overall steps if it's the same day
                    dailyStepCount = data.dailySteps;
                }

                overallStepCount = data.overallSteps; // Load overall step count
            }
            else
            {
                overallStepCount = 0; // Initialize if no file exists
                dailyStepCount = 0; // Initialize daily step count
            }
        }

        private void SaveStepData()
        {
            StepData data = new StepData
            {
                numberOfSteps = dailyStepCount,
                dailySteps = dailyStepCount,
                overallSteps = overallStepCount, // Include overall step count
                lastSavedDate = DateTime.Today.ToString("yyyy-MM-dd") // Store today's date
            };

            string stepCountString = JsonUtility.ToJson(data);
            File.WriteAllText(stepJsonFilePath, stepCountString);
            Debug.Log("Step data saved: " + stepCountString); // Debug log to verify saving
        }

        private void OnPermissionDenied()
        {
            permissionCanvas.gameObject.SetActive(true);
        }

        public void StepsToday()
        {
            StepCounterRequest request = new StepCounterRequest();
            request
                .Since(DateTime.Today)
                .OnQuerySuccess((value) => {
                    // Update dailyStepCount based on the steps recorded today
                    dailyStepCount = value; // Fetch today's steps
                    overallStepCount += dailyStepCount; // Update overall step count
                    text.text = value.ToString();
                    stepCountChannel?.Invoke(value); // Invoke the FloatEventChannel
                    SaveStepData(); // Save data after fetching
                })
                .OnPermissionDenied(() => permissionCanvas.gameObject.SetActive(true))
                .Execute();
        }

        // Other methods remain unchanged...

        async void RequestPermission()
        {
#if UNITY_EDITOR
           
#endif
#if UNITY_ANDROID
            AndroidRuntimePermissions.Permission fileManagementResult = await AndroidRuntimePermissions.RequestPermissionAsync("android.permission.MANAGE_EXTERNAL_STORAGE");
#endif
        }
    }
}
