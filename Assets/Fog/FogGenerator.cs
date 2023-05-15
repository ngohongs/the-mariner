using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayingField))]
public class FogGenerator : MonoBehaviour
{
    private PlayingField playingField;
    public GameObject[] clouds;
    public float distance = 10f;
    public float spacicng = 1.0f;
    public float border = 0.33f;
    // Start is called before the first frame update

    void Start()
    {
        playingField= GetComponent<PlayingField>();

        var pfWidth = playingField.playingWidth;
        var pfHeight = playingField.playingWidth;

        var xmin = transform.position.x - distance;
        var xmax = transform.position.x + pfWidth + distance;
        var ymin = transform.position.z - distance;
        var ymax = transform.position.z + pfHeight + distance;

        var pxmin = transform.position.x - border;
        var pxmax = transform.position.x + pfWidth + border;
        var pymin = transform.position.z - border;
        var pymax = transform.position.z + pfHeight + border;

        for (float x = xmin - 0.5f; x < xmax + 0.5f; x += spacicng)
        {
            for (float y = ymin - 0.5f; y < ymax + 0.5f; y += spacicng)
            { 
                if (x > pxmin && x < pxmax && y > pymin && y < pymax)
                    continue;
                GameObject cloud = Instantiate(clouds[Random.Range(0, clouds.Length)]);
                cloud.transform.position = new Vector3(x, 0, y);
                cloud.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
                cloud.name = "Cloud_" + x + "_" + y;
                cloud.transform.parent = transform;
            }
        }
    }
}
