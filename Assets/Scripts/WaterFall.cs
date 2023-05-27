using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFall : MonoBehaviour {
    [SerializeField] private GameObject originGameobject;

    [SerializeField] private GameObject tile;
    // Start is called before the first frame update
    [SerializeField] private float spawnRateSeconds = 0.00001f;
    [SerializeField] private float powerMin = 1.0f;
    [SerializeField] private float powerMax = 5.0f;
    
    private float _cumulativeTime = 0.0f;
    
    private void SpawnRow() {
        for (var i = 0; i < 5; i++) {
            var n = Instantiate(tile);
            var position = originGameobject.transform.position + new Vector3(-i, 0, 0);
            tile.transform.position = position;
            var rigid = n.GetComponent<Rigidbody>();
            rigid.mass = 30.0f;
            var maxPower = Random.Range(powerMin, powerMax);
            rigid.velocity = Vector3.down * maxPower;
            rigid.AddForce(0, maxPower, 0);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _cumulativeTime += Time.deltaTime;
        
        if(_cumulativeTime >= spawnRateSeconds) {
            _cumulativeTime = 0.0f;
            SpawnRow();
        }
    }
}
