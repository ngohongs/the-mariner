using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum TileType {
    Barrel,
    Current,
    Empty,
    Land,
    Shallow,
    Shipwreck,
    Vortex,
}

public abstract class Tile : MonoBehaviour
{
    public int x = -1, y = -1;
    public abstract bool ApplyEffect(PlayingField field);

    public virtual bool IsMoveable() => true; 
}
