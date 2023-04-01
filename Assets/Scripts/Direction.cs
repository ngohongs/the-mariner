using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight, None }

public static class DirTranslation
{
    public static Vector2Int[] DirToVec =
    {
        new Vector2Int( 0,  1),
        new Vector2Int( 0, -1),
        new Vector2Int(-1,  0),
        new Vector2Int( 1,  0),
        new Vector2Int(-1,  1),
        new Vector2Int( 1,  1),
        new Vector2Int(-1, -1),
        new Vector2Int( 1, -1),
        new Vector2Int( 0,  0),
    };
}
