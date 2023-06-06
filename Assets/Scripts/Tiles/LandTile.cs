using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTile : Tile
{
    private void Awake()
    {
        tileType = TileTypeShort.Land;
    }

    public override bool ApplyEffect(PlayingField field, out bool wait)
    {
        if(field.ship.activeSkills[(int) ESkill.GET_HEALTH] && field.ship.cooldownCounter < field.ship.cooldown) {
            wait = true;
            field.Shift();
            field.ship.cooldownCounter++;
            return true;
        }

        if (field.ship.activeSkills[(int)ESkill.GET_HEALTH] && field.ship.cooldownCounter >= field.ship.cooldown) {
            field.ship.cooldownCounter = 0;
        }
        
        wait = false;
        return false;
    }

    public override bool IsMoveable(Ship ship) {
        if(ship.activeSkills[(int) ESkill.GET_HEALTH]) {
            return true;
        }

        return false;
    }

    public override void Hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public override void Show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
