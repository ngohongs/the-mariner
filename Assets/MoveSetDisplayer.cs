using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine;

public class MoveSetDisplayer : MonoBehaviour
{
    public GameObject background;

    public float xOffset = 0f;
    public float yOffset = 0f;

    [Header("Must be in same order as in Direction")]
    public MoveTile[] tiles;


    public void Show(bool[] moveSet)
    {
        background.SetActive(true);

        foreach (var tile in tiles) 
        { 
            tile.gameObject.SetActive(true);
        }

        for (Direction dir = 0; dir <= Direction.None; dir++)
        {
            tiles[(int)dir].blocked = !moveSet[(int)dir];
            if (dir == Direction.None)
                tiles[(int)dir].middle = true;
        }
    }

    public void Hide()
    {
        background.SetActive(false);
        
        foreach (var tile in tiles)
        {
            tile.gameObject.SetActive(false);
        }
    }

    public void SetPosition(float x, float y)
    {
        transform.position = new Vector3(x + xOffset, transform.position.y, y + yOffset);
    }
}
