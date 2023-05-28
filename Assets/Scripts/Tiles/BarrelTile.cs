using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BarrelTile : Tile
{
    public int foodAmount = 8;
    public GameObject textOverlay;

    private TMP_Text _textOverlayText;
    
    private void TriggerOverlay(Character c) {
        if (c.Skill == ESkill.CHEF) {
            _textOverlayText = textOverlay.GetComponent<TMP_Text>();
            _textOverlayText.text = foodAmount.ToString();
            textOverlay.SetActive(true);
        }
    }

    private void OnEnable() {
        ActiveCharacterEventManager.OnCharacterAdded += TriggerOverlay;
    }

    private void Awake()
    {
        tileType = TileTypeShort.Barrel;
    }

    public override bool ApplyEffect(PlayingField field, out bool wait)
    {
        if (soundEffect != null)
        {
            soundEffect.Play();
        }
        field.ship.AddFood(foodAmount);
        wait = false;
        transform.GetChild(0).gameObject.SetActive(false);
        return false;
    }
}
