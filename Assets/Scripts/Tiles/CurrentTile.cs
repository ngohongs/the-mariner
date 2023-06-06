using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CurrentTile : Tile
{
    private void Awake()
    {
        tileType = TileTypeShort.Current;
    }

    public override bool ApplyEffect(PlayingField field, out bool wait)
    {
        if (field.ship.activeSkills[(int)ESkill.STREAM_SKIP]) {
            wait = false;
            return false;
        }
        
        wait = true;
        field.Move(GetDirection());
        field.tilemap.Replace(x, y, TileType.Empty);
        wait = true;
        return true;
    }

    public Direction GetDirection()
    {
        var forward = transform.forward;

        if (forward == new Vector3(0, 0, 1))
            return Direction.Up;
        else if (forward == new Vector3(0, 0, -1))
            return Direction.Down;
        else if (forward == new Vector3(1, 0, 0))
            return Direction.Right;
        else if (forward == new Vector3(-1, 0, 0))
            return Direction.Left;
      
        Debug.LogError("Error in getting diredtion");
        return Direction.None;
    }

    public void Rotate(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: transform.rotation = Quaternion.Euler(Vector3.zero); break;
            case Direction.Down: transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0)); break;
            case Direction.Right: transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0)); break;
            case Direction.Left: transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0)); break;
            case Direction.UpRight:
            case Direction.UpLeft:
            case Direction.DownRight:
            case Direction.DownLeft:
            default:
                Debug.LogError("Invalid rotation for current tile");
                break;
        }
    }

    public override void Hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public override void Show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
