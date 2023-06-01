using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class MoveTile : MonoBehaviour
{
    public Material empty;
    public Material select;
    public Material noWay;
    public Material noWaySelect;
    public Vector2Int direction;

    private bool _blocked = false;

    private bool _middle = false;

    public bool middle
    {
        get { return _middle; }
        set
        {
            _middle = value;
            renderer.material = value ? empty : noWay;
        }
    }
    
    public bool blocked {
        get { 
            return _blocked; 
        }
        set {
            renderer.material = value ? noWay : empty;
            _blocked = value; 
        }
    }

    private new Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    private void OnMouseOver()
    {
        renderer.material = _blocked ? noWaySelect : select;
    }

    private void OnMouseExit()
    {
        if (middle)
        {
            renderer.material = _blocked ? empty : noWay;
            return;
        }
        renderer.material = _blocked ? noWay : empty;
    }
}
