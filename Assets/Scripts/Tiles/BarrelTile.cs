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
        if (soundEffect != null)
        {
            soundEffect.Play();
        }
        field.ship.AddFood(foodAmount);
        wait = false;
        transform.GetChild(0).gameObject.SetActive(false);
        return false;
    }
}
