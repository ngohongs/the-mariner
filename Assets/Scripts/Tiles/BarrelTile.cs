using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTile : Tile
{
    public override bool ApplyEffect(PlayingField field, out bool wait)
    {
        wait = false;
        return false;
    }
}
