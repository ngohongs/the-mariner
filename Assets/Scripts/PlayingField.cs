using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayingField : MonoBehaviour
{
    [Header("Tilemap")]
    public Tilemap tilemap;

    [Header("Playing field")]
    public int playingWidth = 1;
    [Min(1)]
    public int playingHeight = 1;
    [Min(0)]
    public int xOffset = 0;
    [Min(0)]
    public int yOffset = 0;

    [Header("Ship")]
    public Ship ship;
    public int shipX = 2;
    public int shipY = 2;

    [Header("State")]
    public State state = State.Prepare;
    public Tile destination = null;
    public int completed = 0;
    public List<Tile> moveSet;
    public enum State { Prepare, Action, Finish };

    private void OnDrawGizmos()
    {
        if (tilemap == null)
            return;
        Gizmos.color = Color.green;

        var o = new Vector3(xOffset, 0, yOffset);
     
        Gizmos.DrawLine(o + new Vector3(0, 0.125f, 0), o + new Vector3(0, 0.125f, playingHeight));
        Gizmos.DrawLine(o + new Vector3(playingWidth, 0.125f, 0), o + new Vector3(playingWidth, 0.125f, playingHeight));
        Gizmos.DrawLine(o + new Vector3(0, 0.125f, 0), o + new Vector3(playingWidth, 0.125f, 0));
        Gizmos.DrawLine(o + new Vector3(0, 0.125f, playingHeight), o + new Vector3(playingWidth, 0.125f, playingHeight));

        foreach (var m in moveSet)
            Gizmos.DrawSphere(new Vector3(m.x + 0.5f, 0, m.y + 0.5f - completed), 0.25f);
    }

    // Start is called before the first frame update
    void Start()
    {
        var lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.positionCount = 6;
        lineRenderer.SetPositions(new Vector3[]
            {
                new Vector3(0, 0.1f, 0),
                new Vector3(playingWidth, 0.1f, 0),
                new Vector3(playingWidth, 0.1f, playingHeight),
                new Vector3(0, 0.1f, playingHeight),
                new Vector3(0, 0.1f, 0),
                new Vector3(playingWidth, 0.1f, 0),
            }
        );

        if (tilemap)
        {
            tilemap.transform.position = Vector3.zero;
        }

        if (ship)
        {
            Move(shipX, shipY);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Prepare: Prepare(); break;
            case State.Action: Action(); break;
            case State.Finish: Finish(); break;
            default: Debug.LogError("Non existing state"); break;
        }
    }

    public void Prepare()
    {
        destination = null;
        UpdateMoveSet();
        state = State.Action;
    }

    public void Action()
    {
        destination = GetDestination();
        
        if (destination == null)
            return;

        var coord = TileToPlayCoords(destination.x, destination.y);
        Debug.Log(coord);

        Move(coord.x, coord.y);

        var tile = TileIn(shipX, shipY);
        while (IsInPlayingField(shipX, shipY) && IsInTilemap(shipX, shipY) && tile.ApplyEffect(this))
        {
            tile = TileIn(shipX, shipY);
        }


        state = State.Finish;


    }

    public void Finish()
    {
        Shift();

        if (!IsInPlayingField(shipX, shipY))
            Debug.Log("End");

        state = State.Prepare;
    }

    public Tile GetDestination()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                var tile = hit.transform.gameObject.GetComponent<Tile>();

                if (tile != null)
                {
                    if (moveSet.Contains(tile))
                    {
                        Debug.Log(hit.transform.gameObject.name);
                        return tile;
                    }
                }
            }
        }


        return null;
    }

    public void Shift()
    {
        tilemap.transform.localPosition += new Vector3(0, 0, -1);
        Move(shipX, shipY - 1);
        completed++;
    }

    void Move(int x, int y)
    {
        shipX = x;
        shipY = y;
        ship.transform.position = new Vector3(x + 0.5f + xOffset, 0, y + 0.5f + yOffset);
    }

    public void Move(Direction direction)
    {
        var dir = DirTranslation.DirToVec[(int)direction];
        Move(shipX + dir.x, shipY + dir.y);
    }

    public void UpdateMoveSet()
    {
        List<Tile> result = new List<Tile>();

        for (Direction dir = 0; dir < Direction.None; dir++) 
        {
            var t = DirTranslation.DirToVec[(int)dir];
            var x = shipX + t.x;
            var y = shipY + t.y;

            if (!IsInPlayingField(x, y) || !IsInTilemap(x, y))
                continue;

            var tile = TileIn(x, y);

            if (!tile.IsMoveable())
                continue;

            result.Add(tile);            
        }

        moveSet = result;
    }

    private Tile TileIn(int x, int y)
    {
        var tileCoord = PlayToTileCoords(x, y);

        if (tileCoord.x < 0 || tilemap.width  <= tileCoord.x ||
            tileCoord.y < 0 || tilemap.height <= tileCoord.y)
            return null;

        return tilemap.At(tileCoord.x, tileCoord.y);
    }

    private Vector2Int PlayToTileCoords(int x, int y)
    {
        return new Vector2Int(x - xOffset, y - yOffset + completed);
    }

    private Vector2Int TileToPlayCoords(int x, int y)
    {
        return new Vector2Int(x + xOffset, y + yOffset - completed); 
    }

    bool IsInPlayingField(int x, int y)
    {
        return 0 <= x && x < playingWidth &&
               0 <= y && y < playingHeight;
    }

    bool IsInTilemap(int x, int y)
    {
        var coord = PlayToTileCoords(x, y);
        return 0 <= coord.x && coord.x < tilemap.width &&
               0 <= coord.y && coord.y < tilemap.height;
    }
}
