using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class Ship : MonoBehaviour
{
    [SerializeField]
    private HorizontalLayoutGroup _activeCharactersLayoutGroup;
    [SerializeField]
    private GameObject _activeCharacterButtonPrefab;
    
    private int _foodStored = 15;
    public int initialFoodAmount = 30;

    public int cooldown = 2;
    public int cooldownCounter = 0;
    
    public Boolean[] wantedCharacters = new bool[Character._allCharacters.Count];
    public Sprite[] _imageCharacters = new Sprite[4];
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
    public bool[] activeSkills = new bool[(int) ESkill.COUNT];
    
    public PlayingField field;
    public TextMeshProUGUI foodUI;

    public static Action OnShipRessurected;
    
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
            foodConsumption += 3;
        } else if(skill == ESkill.STREAM_SKIP && !activeSkills[(int)skill]) {
            foodConsumption -= 3;
        }
        
        ActiveCharacterEventManager.CharacterPlayingField(skill);
    }
    
    public void AddActiveCharacter(Character c) {
        var parent = Instantiate(_activeCharacterButtonPrefab, _activeCharactersLayoutGroup.transform);
        
        parent.transform.GetComponent<Image>().overrideSprite = _imageCharacters[(int) c.Skill];
        var header = parent.transform.GetChild(0);
        //header.GetComponent<TextMeshProUGUI>().text = c.Name;
        
        var btn = parent.transform.GetComponent<Button>();
        var panel = parent.transform.GetChild(1);

        var hades = parent.transform.GetComponent<HadesScriptkekw>();
        hades.isHades = c.Skill == ESkill.DEATH_SKIP;
        
        if (c.Skill == ESkill.STREAM_SKIP || c.Skill == ESkill.GET_HEALTH) {
            btn.onClick.AddListener( () => {
                panel.gameObject.SetActive(!panel.gameObject.activeSelf);
                ActiveCharacterEventManager.CharacterClicked(c.Skill);
            });
        } else {
            btn.enabled = false;
        }
    }
    
    public void AddCharacter(Character c) {
        if (!this.skills[(int) c.Skill]) {

            AddActiveCharacter(c);
            if (c.Skill == ESkill.DEATH_SKIP) {
                this.activeSkills[(int)ESkill.DEATH_SKIP] = true;
            }
            
            this.characters[(int) c.Skill] = c;
            this.skills[(int) c.Skill] = true;
            
            ActiveCharacterEventManager.CharacterAdded(c);
        }
    }

    private void ShipRessurected() {
        for (var i = 0; i < (int)ESkill.COUNT; i++) {
            this.skills[i] = false;
            this.activeSkills[i] = false;
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
        OnShipRessurected += ShipRessurected;
        ActiveCharacterEventManager.OnCharacterClicked += ActiveCharacterClicked;
    }

    private void Start()
    {
        if (GameController.instance == null) {
            return;
        }
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

    private void OnDestroy()
    {
        transform.DOKill();
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