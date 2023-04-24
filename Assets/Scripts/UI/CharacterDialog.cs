using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDialog : MonoBehaviour {
    
    [SerializeField]
    private HorizontalLayoutGroup _layoutGroup;

    [SerializeField]
    private GameObject _descriptionPrefab;

    [SerializeField]
    private Ship _ship;

    private void AddCharacterToShip(Character c) {
        _ship.GetComponent<Ship>().AddCharacter(c);
        this.transform.gameObject.SetActive(false);
        Debug.Log("dwd");
    }
    private void Awake() {
        foreach (var c in Character._allCharacters) {
            if (!_ship.HasCharacter(c)) {
                var newButton = Instantiate(_descriptionPrefab, _layoutGroup.transform);

                var descriptionText = newButton.transform.GetChild(0).transform;
                descriptionText.GetComponent<TextMeshProUGUI>().text = c.Description;

                var descriptionButtonT = newButton.transform.GetChild(1).transform;
                var descriptionButton = descriptionButtonT.GetComponent<Button>();
                var descriptionButtonText = descriptionButton.transform.GetChild(0);
            
                descriptionButton.onClick.AddListener(() => AddCharacterToShip(c));
                descriptionButtonText.transform.GetComponent<TextMeshProUGUI>().text = c.Name;
            }
        }
    }
}
