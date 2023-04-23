using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipwreckTile : Tile
{
    //TODO: Character specific shipwreck
   
    
    public override bool ApplyEffect(PlayingField field, out bool wait)
    {
        wait = false;
        return false;
    }
}
