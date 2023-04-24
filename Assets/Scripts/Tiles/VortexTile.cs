using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexTile : Tile
{
    public override bool ApplyEffect(PlayingField field, out bool wait)
    {
        wait = true;
        field.Move(GetDirection());
        field.tilemap.Replace(x, y, TileType.Empty);
        return true;
    }

    public Direction GetDirection()
    {
        return (Direction) Random.Range(0, (int)Direction.DownRight);
    }
}
