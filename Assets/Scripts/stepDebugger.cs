using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Android;
using UnityEngine.InputSystem;


public class StepDebugger : MonoBehaviour
{



  [SerializeField]TMP_Text messageText, DEBUGTEXT;

   long stepOffset;
   void Start()
   {
       if (Application.isEditor) { return; }
       RequestPermission(); 
       InputSystem.EnableDevice(StepCounter.current);
   }

   void Update()
   {
       if (Application.isEditor) { return; }

       if (stepOffset == 0)
       {
           stepOffset = StepCounter.current.stepCounter.ReadValue();
           DEBUGTEXT.text = ("Step offset " + stepOffset);
       }
       else
       {
           messageText.text = "Steps: " + (StepCounter.current.stepCounter.ReadValue() - stepOffset).ToString();
       }
   }


   async void RequestPermission()
   {
       #if UNITY_EDITOR
           DEBUGTEXT.text = ("Editor Platform");
       #endif
       #if UNITY_ANDROID
           AndroidRuntimePermissions.Permission result = await AndroidRuntimePermissions.RequestPermissionAsync("android.permission.ACTIVITY_RECOGNITION");
           if (result == AndroidRuntimePermissions.Permission.Granted)
               DEBUGTEXT.text = ("Step offset amount " + stepOffset);
           else
            DEBUGTEXT.text = ("Permission state: " + result);
       #endif
   }
}