using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public int foodStored = 15;
    public int foodConsumption = 1;

    public PlayingField field;

    public void Center()
    {
        var center = new Vector3(field.playingWidth / 2 + field.xOffset + 0.5f, 0, field.playingHeight / 2 + field.yOffset + 0.5f);
        center += field.transform.position;

        transform.position = center;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void SetFoodConsumption(int consumption)
    {
        foodStored = consumption;
    }

    public void ConsumeFood()
    {
        foodStored -= foodConsumption; 
    }

    public void AddFood(int amount) 
    {
        foodStored += amount;
    }

    public bool NoFood()
    {
        return foodStored <= 0;
    }

}

[CustomEditor(typeof(Ship)), CanEditMultipleObjects]
public class ShipInspector : Editor
{
    public override void OnInspectorGUI()
    {
        Ship ship = (Ship)target;

        base.OnInspectorGUI();

        EditorGUILayout.Space();

        if (GUILayout.Button("Center ship to playing field"))
        {
            Debug.Log("Centering ship to playing field");
            ship.Center();
        }
    }
}