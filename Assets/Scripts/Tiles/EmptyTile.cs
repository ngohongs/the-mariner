using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : Tile
{
    void Awake()
    {
        tileType = TileTypeShort.Empty;
    }

    public override bool ApplyEffect(PlayingField field, out bool wait) 
    {
        wait = false;
        return false; 
    }
}
