using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipwreckTile : Tile
{
    //TODO: Character specific shipwreck

    private void Awake()
    {
        tileType = TileTypeShort.Shipwreck;
    }

    public override bool ApplyEffect(PlayingField field, out bool wait) {
        DialogManager.TriggerShipwreckStep();
        field.tilemap.Replace(x, y, TileType.Empty);
        wait = false;
        return false;
    }
}
