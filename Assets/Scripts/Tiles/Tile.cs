using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum TileType
{
    Barrel,
    CurrentDown,
    CurrentLeft,
    CurrentRight,
    CurrentUp,
    Empty,
    Land,
    Sand,
    Shallow,
    Shipwreck,
    Vortex,
}

public enum TileTypeShort 
{
    Barrel,
    Current,
    Empty,
    Land,
    Sand,
    Shallow,
    Shipwreck,
    Vortex,
}

public enum TileCode : int
{
    Barrel = 'b',
    CurrentDown = 'd',
    CurrentLeft = 'l',
    CurrentRight = 'r',
    CurrentUp = 'u',
    Empty = 'e',
    Land = 'i',
    Sand = 'x',
    Shallow = 's',
    Shipwreck = 'w',
    Vortex = 'v',
}

public static class TileCodeTranslation
{
    public static Dictionary<TileCode, TileType> TileCodeToType = new Dictionary<TileCode, TileType>() {
        {TileCode.Barrel, TileType.Barrel},
        {TileCode.CurrentDown, TileType.CurrentDown},
        {TileCode.CurrentLeft, TileType.CurrentLeft},
        {TileCode.CurrentRight, TileType.CurrentRight},
        {TileCode.CurrentUp, TileType.CurrentUp},
        {TileCode.Empty, TileType.Empty},
        {TileCode.Land, TileType.Land},
        {TileCode.Sand, TileType.Sand},
        {TileCode.Shallow, TileType.Shallow},
        {TileCode.Shipwreck, TileType.Shipwreck},
        {TileCode.Vortex, TileType.Vortex},
    };
}

public abstract class Tile : MonoBehaviour
{
    public TileTypeShort tileType;
    public int x = -1, y = -1;
    public abstract bool ApplyEffect(PlayingField field, out bool wait);

    public virtual bool IsMoveable() => true;

    public AudioSource soundEffect;
}
