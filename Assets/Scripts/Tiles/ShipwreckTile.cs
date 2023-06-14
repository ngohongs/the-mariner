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
        if (!field.IsInPlayingField(field.shipX, field.shipY - 1) ||
            !field.IsInTilemap(field.shipX, field.shipY - 1))
        {
            wait = false;
            return false;
        }
        DialogManager.TriggerShipwreckStep();
        field.tilemap.Replace(x, y, TileType.Empty);
        wait = false;
        return false;
    }

    public override void Hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public override void Show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
