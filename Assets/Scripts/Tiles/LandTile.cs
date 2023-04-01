using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTile : Tile
{
    public override bool ApplyEffect(PlayingField field)
    {
        return false;
    }

    public override bool IsMoveable() => false;
}
