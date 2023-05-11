using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class Ship : MonoBehaviour
{
    [SerializeField]
    private HorizontalLayoutGroup _activeCharactersLayoutGroup;
    [SerializeField]
    private GameObject _activeCharacterButtonPrefab;
    
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
                foodUI = GameController.instance.uIContoller.GetUIElement("Food").GetComponent<TextMeshProUGUI>();
            }
        }
    }

    public int foodConsumption = 1;

    private Character[] characters = new Character[(int) ESkill.COUNT];
    public bool[] skills = new bool[(int) ESkill.COUNT];
    public bool[] activeSkills = new bool[(int) ESkill.COUNT];
    
    public PlayingField field;
    

    public TextMeshProUGUI foodUI;

    public void Center()
    {
        var center = new Vector3(field.shipX + field.xOffset + 0.5f, 0, field.shipY + field.yOffset + 0.5f);
        center += field.transform.position;

        transform.position = center;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void ActiveCharacterClicked(ESkill skill) {
        activeSkills[(int)skill] = !activeSkills[(int)skill];
        
        if(skill == ESkill.STREAM_SKIP && activeSkills[(int)skill]) {
            foodConsumption += 1;
        } else if(skill == ESkill.STREAM_SKIP && !activeSkills[(int)skill]) {
            foodConsumption -= 1;
        }
        
    }
    
    public void AddActiveCharacter(Character c) {
        var btn = Instantiate(_activeCharacterButtonPrefab, _activeCharactersLayoutGroup.transform);
        btn.GetComponent<Button>().onClick.AddListener( () => {
            ActiveCharacterEventManager.CharacterClicked(c.Skill);
        });
        
        btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = c.Name;
    }
    
    public void AddCharacter(Character c) {
        if (!this.skills[(int) c.Skill]) {

            
            if (c.Skill == ESkill.STREAM_SKIP) {
                AddActiveCharacter(c);
            } else if (c.Skill == ESkill.DEATH_SKIP) {
                this.activeSkills[(int)ESkill.DEATH_SKIP] = true;
            }
            
            this.characters[(int) c.Skill] = c;
            this.skills[(int) c.Skill] = true;
            
            ActiveCharacterEventManager.CharacterAdded(c);
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

    private void OnEnable() {
        ActiveCharacterEventManager.OnCharacterClicked += ActiveCharacterClicked;
    }

    private void Start()
    {
        foodStored = initialFoodAmount;
        foodUI = GameController.instance.uIContoller.GetUIElement("Food").GetComponent<TextMeshProUGUI>();
        if (foodUI != null)
        {
            foodUI.text = foodStored.ToString();
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