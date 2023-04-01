using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : Tile
{
    public override bool ApplyEffect(PlayingField field) 
    {
        return false; 
    }
}
