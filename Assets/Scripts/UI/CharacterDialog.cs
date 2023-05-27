using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDialog : MonoBehaviour {
    
    [SerializeField]
    private GameObject _leftPagePrefab;
    [SerializeField]
    private GameObject _rightPagePrefab;
    

    [SerializeField]
    private Ship _ship;

    public Sprite imagetest;
    public Sprite[] _imageCharacters = new Sprite[4];

    private List<GameObject> _objectsShown = new List<GameObject>(4);
    
    private void AddCharacterToShip(Character c) {
        _ship.GetComponent<Ship>().AddCharacter(c);
        this.transform.gameObject.SetActive(false);
        Debug.Log("dwd");
    }

    private void OnEnable() {
        _ship = GameObject.Find("Ship").GetComponent<Ship>();
        DialogManager.onShipwreckStep += Show;
    }

    public void NextPage() {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
    
    public void PreviousPage() {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }
    

    public void Show() {
        // foreach (Transform child in transform) {
        //     foreach (Transform subChild in child.transform.GetChild(0).transform) {
        //         Destroy(subChild.gameObject);
        //     }
        // }

        foreach(GameObject obj in _objectsShown) {
            Destroy(obj);
        }
        
        int pageCount = 0;
        int cCount = 0;
        
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        foreach (var c in Character._allCharacters) {
            if (!_ship.HasCharacter(c)) {
                
                if(cCount > 1) pageCount = 1;
                
                
                var page = new GameObject();
                if (cCount % 2 == 0) {
                    page = Instantiate(_leftPagePrefab, transform.GetChild(pageCount).transform);
                }
                else {
                    page = Instantiate(_rightPagePrefab, transform.GetChild(pageCount).transform);
                }

                _objectsShown.Add(page);
                var descriptionText = page.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                var addCharacterButton = page.transform.GetChild(3).GetComponent<Button>();
                var addCharacterButtonText = page.transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                var profilePicture = page.transform.GetChild(1).GetComponent<Image>();
                
                descriptionText.GetComponent<TextMeshProUGUI>().text = c.Description;
                profilePicture.GetComponent<Image>().overrideSprite = _imageCharacters[(int) c.Skill];

                addCharacterButton.onClick.AddListener(() => AddCharacterToShip(c));
                addCharacterButtonText.transform.GetComponent<TextMeshProUGUI>().text = c.Name;
                cCount++;
            }
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
