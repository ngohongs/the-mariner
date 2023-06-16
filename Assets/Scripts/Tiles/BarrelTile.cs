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
    private bool eaten = false;
    private TMP_Text _textOverlayText;

    public float bonus = 50f;
    public int diceSize = 3;


    private void TriggerOverlay(Character c) {
        Debug.Log("Chef " + gameObject.name);
        if (eaten)
        {
            return;
        }
        if (c.Skill == ESkill.CHEF) {
            _textOverlayText = textOverlay.GetComponent<TMP_Text>();
            int change = Random.Range(0, diceSize);
            if (change == 0)
            {
                foodAmount = (int)((1 + bonus / 100) * foodAmount);
            }
            _textOverlayText.text = foodAmount.ToString();
            textOverlay.SetActive(true);
        }
    }

    private void OnEnable() {
        ActiveCharacterEventManager.OnCharacterAdded += TriggerOverlay;
    }

    private void OnDisable()
    {
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

    public override void Hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        if (transform.childCount == 3)
            transform.GetChild(2).gameObject.SetActive(false);
    }

    public override void Show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }
}

