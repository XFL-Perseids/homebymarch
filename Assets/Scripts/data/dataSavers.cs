using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.Android;
using UnityEngine.InputSystem;

[System.Serializable]
public class StepData{
  public int numberOfSteps;
  public int dailySteps; 
  public int overallSteps;
}

public class PlayerPositionData{
  public float playerXPosition;
  public float playerYPosition;
  public float playerZPosition;
  
}