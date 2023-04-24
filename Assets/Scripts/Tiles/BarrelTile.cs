using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTile : Tile
{
    public int foodAmount = 8;

    private void Awake()
    {
        tileType = TileTypeShort.Barrel;
    }

    public override bool ApplyEffect(PlayingField field, out bool wait)
    {
        
        field.ship.AddFood(foodAmount);
        wait = false;
        field.tilemap.Replace(x, y, TileType.Empty);
        return false;
    }
}
