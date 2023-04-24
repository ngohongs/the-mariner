using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour {

    [SerializeField]
    private GameObject _shipwreckDialogue;
    // Start is called before the first frame update

    public delegate void OnShipwreckStep();
    public static event OnShipwreckStep onShipwreckStep;

    public static void TriggerShipwreckStep() {
        if (onShipwreckStep != null)
            onShipwreckStep();
    }
    
    private void OnEnable() {
        onShipwreckStep += TriggerShipwreckDialogue;
    }

    private void TriggerShipwreckDialogue() {
        _shipwreckDialogue.SetActive(!_shipwreckDialogue.activeSelf);
    }
}
