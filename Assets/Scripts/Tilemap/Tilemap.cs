using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class Tilemap : MonoBehaviour
{
    [Min(1)]
    public int width = 1;
    [Min(1)]
    public int height = 1;

    public int end = 1;

    [Space]
    [Header("NEEDS TO BE IN SAME ORDER AS ENUM IN Tile.cs (i.e. IN ALPHABETICAL)")]
    public List<GameObject> tiles;

    [Space]
    public List<Tile> map;

    [Header("Generator")]
    public TextAsset template;

    private void OnDrawGizmos()
    {
        var o = transform.position;

        Gizmos.color = Color.magenta;

        for (int i = 0; i < width + 1; i++)
        {
            var start = new Vector3(i, 0, 0) + o;
            var end = new Vector3(i, 0, height) + o;
            Gizmos.DrawLine(start, end)
            ;
            for (int j = 0; j < height + 1; j++)
            {
                start = new Vector3(0, 0, j) + o;
                end = new Vector3(width, 0, j) + o;
                Gizmos.DrawLine(start, end);
            }
        }

        Gizmos.color = Color.yellow;
        for (int i = 0; i < width; i++)
        {
            Gizmos.DrawSphere(new Vector3(i + 0.5f , 0.5f, end + 0.5f) + o, 0.25f);
        }
    }

    private void Start()
    {
        if (IsEmpty())
        {
            Generate();
        }
    }

    bool IsEmpty()
    {
        return map.Count == 0;
    }

    public void Generate()
    {
        if (!IsEmpty())
            Delete();

        map = new List<Tile>(new Tile[height * width]);

        for (int i = 0; i < height * width; i++)
        {
            Insert(i, (TileType)UnityEngine.Random.Range(0, tiles.Count));
        }
    }

    public void Delete()
    {
        if (IsEmpty())
            return;

        foreach (Tile tile in map)
        {
            if (tile == null) continue;
            if (Application.isEditor)
            {
                DestroyImmediate(tile.gameObject);
            }
            else
            {
                Destroy(tile.gameObject);
            }
        }

        map.Clear();
    }

    public int Row(int i)
    {
        return i / width;
    }

    public int Col(int i)
    {
        return i % width;
    }

    public int Index(int x, int y)
    {
        return y * width + x;
    }

    public Tile At(int x, int y)
    {
        int i = Index(x, y);
        return map[i];
    }

    public Tile Replace(int x, int y, TileType type)
    {
        int i = Index(x, y);

        Destroy(map[i].gameObject);

        return Insert(x, y, type);
    }

    public Tile Insert(int x, int y, TileType type)
    {
        return Insert(Index(x, y), type);
    }

    public Tile Insert(int i, TileType type)
    {
        map[i] = Instantiate(tiles[(int)type], transform).GetComponent<Tile>();
        map[i].name += "_(" + Col(i) + "," + Row(i) + ")";
        map[i].transform.localPosition = new Vector3(Col(i) + 0.5f, 0, Row(i) + 0.5f);
        map[i].x = Col(i);
        map[i].y = Row(i);
        return map[i];
    }

    public void Load()
    {
        var config = template.text.Split(new[] { "\r\n", "\r", "\n", "\t", " " }, StringSplitOptions.RemoveEmptyEntries);

        if (config.Length < 4)
        {
            Debug.LogError("Error while loading - invalid template");
            return;
        }
        
        if (!Int32.TryParse(config[0], out int end))
        {
            Debug.LogError("Error while loading end from template");
            return;
        }

        if (!Int32.TryParse(config[1], out int wid))
        {
            Debug.LogError("Error while loading width from template");
            return;
        }

        if (!Int32.TryParse(config[2], out int hei))
        {
            Debug.LogError("Error while loading height from template");
            return;
        }

        var stringMap = config[3..].Reverse();

        if (stringMap.Count() != hei || end > hei)
        {
            Debug.LogError("Error while loading height doesn't match or is smaller than end");
            return;
        }

        List<TileType> tilemap = new List<TileType>();

        foreach (var row in stringMap)
        {
            if (row.Length != wid)
            {
                Debug.LogError("Error while loading width doesn't match");
                return;
            }

            foreach (int c in row)
            {
                if (!Enum.IsDefined(typeof(TileCode), c))
                {
                    Debug.LogError("Error while loading unknown tile: " + c);
                    return;
                }

                var tile = TileCodeTranslation.TileCodeToType[(TileCode)c];

                tilemap.Add(tile);
            }
        }

        Delete();

        width = wid;
        height = hei;
        this.end = end;

        map = new List<Tile>(new Tile[height * width]);

        for (int i = 0; i < tilemap.Count; i++)
        {
            Insert(i, tilemap[i]);
        }

    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(Tilemap))]
public class TilemapInspector : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck(); // Start recording changes
        base.OnInspectorGUI(); // Draw the default inspector

        Tilemap tilemap = (Tilemap)target;

        if (GUILayout.Button("Generate tilemap from a template"))
        {
            Debug.Log("Generating tilemap from template");
            tilemap.Load();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate tilemap procedurally"))
        {
            Debug.Log("Generating tilemap randomly");
            tilemap.Generate();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Delete tilemap's content"))
        {
            Debug.Log("Emptying tilemap");
            tilemap.Delete();
        }

        if (EditorGUI.EndChangeCheck()) // Check if any changes were made
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene()); // Mark scene as dirty
        }
    }
}

#endif
