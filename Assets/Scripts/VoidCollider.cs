using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidCollider : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        var otherGameObject = other.gameObject;
        Destroy(otherGameObject);
    }
}
