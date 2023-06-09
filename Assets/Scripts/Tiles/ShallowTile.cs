using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShallowTile : Tile
{
    
    private void Awake()
    {
        tileType = TileTypeShort.Shallow;
    }

    public override bool ApplyEffect(PlayingField field, out bool wait)
    {
        if (soundEffect != null)
        {
            soundEffect.Play();
        }
        wait = true;
        field.Shift();
        return false;
    }
}
