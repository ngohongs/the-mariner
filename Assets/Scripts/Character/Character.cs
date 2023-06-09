using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

    //TODO: DIALOGUE SYSTEM
    
    public static List<Character> _allCharacters = new List<Character>() {
        new Character(
            "The Chef",
            ESkill.CHEF,
            "",
            "The Chef is a great cook with a keen sense for spotting barrels with extra food. He'll  share his observations! (Passive)"
        ),
        new Character(
            "Machio-san",
            ESkill.STREAM_SKIP,
            "",
            "Machio-san can help your ship push through treacherous currents, but he'll need a small fee of extra food to do so. (Active)"
        ),
        new Character(
            "Hades",
            ESkill.DEATH_SKIP,
            "",
            "Hades is a master of resurrection. He will bring you back to life, but only once. (Passive)"
        ),
        new Character(
            "The Diplomat",
            ESkill.GET_HEALTH,
            "",
            "Diplomat will guide you through the land without harm. It will cost you few turns though. (Active)"
        ),
    };
    
    private ESkill _skill;
    public ESkill Skill {
        get => _skill;
    }

    private String _name;
    public String Name { 
        get => _name;
    }

    private String _description;
    public String Description {
        get => _description;
    }
    //TODO: Image
    private String[] _messages = new String[(int) EMessage.COUNT];

    public String Message(EMessage msg) {
        return this._messages[(int)msg];
    }
    
    public Character(String name, ESkill eSkill, String welcomeMessage, String description) {
        this._skill = eSkill;
        this._name = name;
        this._messages[(int) EMessage.WELCOME] = welcomeMessage;
        this._description = description;
    }
    
}
