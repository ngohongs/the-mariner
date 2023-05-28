using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : Tile
{
    private void Awake()
    {
        tileType = TileTypeShort.Empty;
    }

    public override bool ApplyEffect(PlayingField field, out bool wait) 
    {
        wait = false;
        return false; 
    }

    public void PlayVortexSound()
    {
        if (soundEffect != null)
        {
            soundEffect.Play();
        }
    }
}
