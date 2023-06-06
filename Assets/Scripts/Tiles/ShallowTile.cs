using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShallowTile : Tile
{
    
    private void Awake()
    {
        tileType = TileTypeShort.Shallow;
    }

    public override bool ApplyEffect(PlayingField field, out bool wait)
    {
        if (soundEffect != null)
        {
            soundEffect.Play();
        }
        wait = true;
        field.Shift();
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
