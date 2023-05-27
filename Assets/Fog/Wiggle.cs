using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    public float minSize = 0.15f;
    public float maxSize = 0.33f;
    public float minSpeed = 0.25f;
    public float maxSpeed = 1f;

    private float targetSize = 0.33f;
    private float targetSpeed = 1.0f;

    private bool inTween = false;

    // Update is called once per frame
    void Update()
    {
        if (transform == null)
            return;
        
        if (!inTween)
        {
            inTween = true;
            transform.DOScale(targetSize, targetSpeed).OnComplete(() =>
            {
                GetNewTargets();
                inTween = false;
            });
        }
    }

    void GetNewTargets()
    {
        targetSize = Random.Range(minSize, maxSize);
        targetSpeed = Random.Range(minSpeed, maxSpeed);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
