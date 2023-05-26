using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShipEndLevel : MonoBehaviour {
    
    public GameObject shipObject;
    // Start is called before the first frame update
    void Start()
    {
        PlayingField.OnEndingEvent?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        shipObject.transform.position -= Vector3.up * Time.deltaTime * 1.0f;
    }
}
