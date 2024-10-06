using Repforge.StepCounterPro;
using System;
using TMPro;
using UnityEngine;

namespace HomeByMarch {
public class StepCountDemo : MonoBehaviour
{
    public TMP_Text text;
    public Canvas permissionCanvas;
    [SerializeField] FloatEventChannel stepCountChannel; // Added FloatEventChannel

    void OnEnable()
    {
        StepsToday();
    }

    void Start()
    {
        StepCounterRequest permissionRequest = new StepCounterRequest();
        permissionRequest
            .OnPermissionGranted(OnPermissionGranted)
            .OnPermissionDenied(OnPermissionDenied)
            .RequestPermission();
    }

    private void OnPermissionGranted()
    {
        StepCounterRequest stepRequest = new StepCounterRequest();
        stepRequest
            .Since(DateTime.Today)
            .OnQuerySuccess((value) => {
                text.text = value.ToString();
                stepCountChannel?.Invoke(value); // Invoke the FloatEventChannel
            })
            .Execute();
    }

    private void OnPermissionDenied()
    {
        permissionCanvas.gameObject.SetActive(true);
    }

    public void OpenAppSettings()
    {
        StepCounterRequest request = new StepCounterRequest();
        request.OpenAppSettings();
    }

    public void StepsBetween5and10MinutesAgo()
    {
        StepCounterRequest request = new StepCounterRequest();
        request
            .Between(DateTime.Now.AddMinutes(-10), DateTime.Now.AddMinutes(-5))
            .OnQuerySuccess((value) => {
                text.text = value.ToString();
                stepCountChannel?.Invoke(value); // Invoke the FloatEventChannel
            })
            .OnPermissionDenied(() => permissionCanvas.gameObject.SetActive(true))
            .Execute();
    }

    public void StepsBetween9and11amToday()
    {
        StepCounterRequest request = new StepCounterRequest();
        request
            .Between(DateTime.Today.AddHours(9), DateTime.Today.AddHours(11))
            .OnQuerySuccess((value) => {
                text.text = value.ToString();
                stepCountChannel?.Invoke(value); // Invoke the FloatEventChannel
            })
            .OnPermissionDenied(() => permissionCanvas.gameObject.SetActive(true))
            .Execute();
    }

    public void StepsPast5Minutes()
    {
        StepCounterRequest request = new StepCounterRequest();
        request
            .Since(DateTime.Now.AddMinutes(-5))
            .OnQuerySuccess((value) => {
                text.text = value.ToString();
                stepCountChannel?.Invoke(value); // Invoke the FloatEventChannel
            })
            .OnPermissionDenied(() => permissionCanvas.gameObject.SetActive(true))
            .Execute();
    }

    public void StepsPastHour()
    {
        StepCounterRequest request = new StepCounterRequest();
        request
            .Since(DateTime.Now.AddHours(-1))
            .OnQuerySuccess((value) => {
                text.text = value.ToString();
                stepCountChannel?.Invoke(value); // Invoke the FloatEventChannel
            })
            .OnPermissionDenied(() => permissionCanvas.gameObject.SetActive(true))
            .Execute();
    }

    public void StepsToday()
    {
        StepCounterRequest request = new StepCounterRequest();
        request
            .Since(DateTime.Today)
            .OnQuerySuccess((value) => {
                text.text = value.ToString();
                stepCountChannel?.Invoke(value); // Invoke the FloatEventChannel
            })
            .OnPermissionDenied(() => permissionCanvas.gameObject.SetActive(true))
            .Execute();
    }

    public void StepsYesterday()
    {
        StepCounterRequest request = new StepCounterRequest();
        request
            .Between(DateTime.Today.AddDays(-1), DateTime.Today)
            .OnQuerySuccess((value) => {
                text.text = value.ToString();
                stepCountChannel?.Invoke(value); // Invoke the FloatEventChannel
            })
            .OnPermissionDenied(() => permissionCanvas.gameObject.SetActive(true))
            .Execute();
    }
}
}