using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClaimQuestButton : MonoBehaviour, ISelectHandler{

    private UnityAction onSelectAction;
    public Button button {get; private set;}

    public void Initialize(string displayName, UnityAction selectAction){
        this.button = this.GetComponent<Button>();
        this.onSelectAction = selectAction;
        // this.buttonText = this.GetComponentInChildren<TMP_Text>();
        
    }

    public void OnSelect(BaseEventData eventData){
        onSelectAction();
    }
}