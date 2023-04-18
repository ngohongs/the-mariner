using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTile : Tile
{
    public int foodAmount = 8;
    public override bool ApplyEffect(PlayingField field, out bool wait)
    {
        field.ship.AddFood(foodAmount);
        wait = false;
        return false;
    }
}
