using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesScriptkekw : MonoBehaviour {
    public bool isHades = false;
    private void OnEnable() {
        Ship.OnShipRessurected += JustDeleteTheVec;
    }

    private void OnDisable()
    {
        Ship.OnShipRessurected -= JustDeleteTheVec;
    }

    private void OnDestroy()
    {
        Ship.OnShipRessurected -= JustDeleteTheVec;
    }

    private void JustDeleteTheVec() {
        if(isHades)
            Destroy(transform.gameObject);
    }

}
