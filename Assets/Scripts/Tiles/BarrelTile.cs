using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrelTile : Tile
{
    public int foodAmount = 8;
    public GameObject textOverlay;
    public GameObject textPrefab;
    private bool eaten = false;
    private TMP_Text _textOverlayText;

    public float bonus = 50f;
    public int diceSize = 3;


    private void TriggerOverlay(Character c) {

        var overlay = Instantiate(textPrefab, textOverlay.transform);
        
        if (c.Skill == ESkill.CHEF) {
            int change = Random.Range(0, diceSize);
            if (change == 0)
            {
                foodAmount = (int)((1 + bonus / 100) * foodAmount);
            }
            _textOverlayText = overlay.GetComponent<TMP_Text>();
            _textOverlayText.text = foodAmount.ToString();
            
        }
    }

    private void OnEnable() {
        ActiveCharacterEventManager.OnCharacterAdded += TriggerOverlay;
    }

    private void OnDisable() {
        ActiveCharacterEventManager.OnCharacterAdded -= TriggerOverlay;
    }

    private void Awake()
    {
        tileType = TileTypeShort.Barrel;
    }

    public override bool ApplyEffect(PlayingField field, out bool wait)
    {
        if (eaten)
        {
            wait = false;
            return false;
        }

        if (soundEffect != null)
        {
            soundEffect.Play();
        }
        textOverlay.SetActive(false);
        eaten = true;
        field.ship.AddFood(foodAmount);
        wait = false;
        transform.GetChild(0).gameObject.SetActive(false);
        return false;
    }
}
