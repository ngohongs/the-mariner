using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public static Memory instance;

    public TextAsset barrelTextAsset;
    public TextAsset currentTextAsset;
    public TextAsset emptyTextAsset;
    public TextAsset landTextAsset;
    public TextAsset shallowTextAsset;
    public TextAsset shipwreckTextAsset;
    public TextAsset vortexTextAsset;

    Dictionary<TileTypeShort, bool> visitedTileTypes = null;

    public void Awake()
    {
        if (visitedTileTypes == null)
        {
            visitedTileTypes = new Dictionary<TileTypeShort, bool>();
            foreach (TileTypeShort tileType in System.Enum.GetValues(typeof(TileTypeShort)))
            {
                visitedTileTypes.Add(tileType, false);
            }
        }

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool Remember(TileTypeShort tileType)
    {
        if (visitedTileTypes[tileType])
            return false;

        string text = "";

        switch (tileType)
        {
            case TileTypeShort.Barrel: text = barrelTextAsset.text; break;
            case TileTypeShort.Current: text = currentTextAsset.text; break;
            case TileTypeShort.Empty: text = emptyTextAsset.text;  break;
            case TileTypeShort.Land: text = landTextAsset.text; break;
            case TileTypeShort.Shallow: text = shallowTextAsset.text; break;
            case TileTypeShort.Shipwreck: text = shipwreckTextAsset.text; break;
            case TileTypeShort.Vortex: text = vortexTextAsset.text; break;
            default: break;
        }

        GameController.instance.uIContoller.DisplayDialogueText(text);
        visitedTileTypes[tileType] = true;
        return true;
    }

    public bool HasVisited(TileTypeShort tileType)
    {
        return visitedTileTypes[tileType];
    }
}
