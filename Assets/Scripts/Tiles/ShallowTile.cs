using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShallowTile : Tile
{
    public override bool ApplyEffect(PlayingField field)
    {
        field.Shift();
        return false;
    }
}
