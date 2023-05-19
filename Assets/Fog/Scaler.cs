using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    public float scaleSpeed = 1f;
    public float maxSize = 1f;
    public enum ScaleMode { Grow, Shrink };
    private ScaleMode scaleMode;
    private bool scale = false;
    private bool inTween = false;

    // Update is called once per frame
    void Update()
    {
        if (scale && !inTween)
        {
            inTween = true;
            if (scaleMode == ScaleMode.Shrink)
            {
                transform.DOScale(0.0f, scaleSpeed).OnComplete(() => {
                    gameObject.SetActive(false);
                    inTween = false;
                    });
            }
            else if (scaleMode == ScaleMode.Grow)
            {
                transform.DOScale(maxSize, scaleSpeed).OnComplete(() => {
                    inTween = false;
                });
            }
            scale = false;
        }
    }

    public void StartScale(ScaleMode mode)
    {
        scaleMode = mode;
        scale = true;
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
