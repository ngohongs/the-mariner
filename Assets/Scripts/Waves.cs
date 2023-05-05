using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    [Serializable]
    public struct Octave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }

    public List<Octave> octaves;

    private PlayingField playingField;
    private Tilemap tilemap;

    private void Awake()
    {
        playingField = GetComponent<PlayingField>();
        tilemap = playingField.tilemap;
    }

    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < tilemap.width; x++)
        {
            for (int y = 0; y < tilemap.height; y++)
            {
                var height = 0.0f;
                for (int o = 0; o < octaves.Count; o++)
                {
                    if (octaves[o].alternate)
                    {
                        var perl = Mathf.PerlinNoise((x * octaves[o].scale.x) / tilemap.width, (y * octaves[o].scale.y) / tilemap.height) * Mathf.PI * 2f;
                        height += Mathf.Cos(perl + octaves[o].speed.magnitude * Time.time) * octaves[o].height;
                    }
                    else
                    {
                        var perl = Mathf.PerlinNoise((x * octaves[o].scale.x + Time.time * octaves[o].speed.x) / tilemap.width, (y * octaves[o].scale.y + Time.time * octaves[o].speed.y) / tilemap.height) - 0.5f;
                        height += perl * octaves[o].height;
                    }
                }
                var position = tilemap.map[tilemap.Index(x, y)].transform.position;
                position.y = height;
                tilemap.map[tilemap.Index(x, y)].transform.position = position;
            }
        }
    }
}
