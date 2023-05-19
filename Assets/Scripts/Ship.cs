using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using TMPro;
using DG.Tweening;
using static System.TimeZoneInfo;

public class Ship : MonoBehaviour
{
  


    private int _foodStored = 15;
    public int initialFoodAmount = 30;
    public int foodStored

    {
        get
        {
            return _foodStored;
        }
        set
        {
            _foodStored = value;
            if (foodUI != null)
            {
                foodUI.text = foodStored.ToString();
            }
            else
            {
                var foodPanel = GameController.instance.uIContoller.GetUIElement("Food");
                // iterate through all childs in foodPanel and find one named FoodAmount

                foreach (Transform child in foodPanel.transform)
                {
                    if (child.name == "FoodAmount")
                    {
                        foodUI = child.GetComponent<TextMeshProUGUI>();
                        foodUI.text = foodStored.ToString();
                    }
                }
                
            }
        }
    }

    public int foodConsumption = 1;

    private Character[] characters = new Character[(int) ESkill.COUNT];
    public bool[] skills = new bool[(int) ESkill.COUNT];
    
    public PlayingField field;
    

    public TextMeshProUGUI foodUI;

    public void Center()
    {
        var center = new Vector3(field.shipX + field.xOffset + 0.5f, 0, field.shipY + field.yOffset + 0.5f);
        center += field.transform.position;

        transform.position = center;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void AddCharacter(Character c) {
        if (!this.skills[(int) c.Skill]) {
            //TODO: DISPLAY WELCOME MESSAGE
            
            this.characters[(int) c.Skill] = c;
            this.skills[(int) c.Skill] = true;
        }
    }

    public bool HasCharacter(Character c) {
        return this.skills[(int)c.Skill];
    }
    
    public void SetFoodConsumption(int consumption)
    {
        foodConsumption = consumption;
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

    private void Start()
    {
        foodStored = initialFoodAmount;
        var foodPanel = GameController.instance.uIContoller.GetUIElement("Food");
        foreach (Transform child in foodPanel.transform)
        {
            if (child.name == "FoodAmount")
            {
                foodUI = child.GetComponent<TextMeshProUGUI>();
                foodUI.text = foodStored.ToString();
                break;
            }
        }
    }
}


#if UNITY_EDITOR
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
#endif