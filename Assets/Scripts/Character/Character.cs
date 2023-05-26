using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

    //TODO: DIALOGUE SYSTEM
    
    public static List<Character> _allCharacters = new List<Character>() {
        new Character(
            "CharA",
            ESkill.CHEF,
            "",
            "CharA is a great cook with a keen sense for spotting barrels with extra food. She'll sometimes share her finds with you!"
        ),
        new Character(
            "CharB",
            ESkill.STREAM_SKIP,
            "",
            "CharB can help your ship push through treacherous currents, but he'll need a small fee of one piece of food to do so."
        ),
        new Character(
            "CharD",
            ESkill.DEATH_SKIP,
            "",
            "CharD is a master of resurrection, but be warned: their magic comes with a catch. If you sink, they'll bring you back to life, but you'll lose all your other crew members in the process. It's like hitting the reset button, but with extra pain and suffering!"
        ),
        new Character(
            "The Diplomat",
            ESkill.GET_HEALTH,
            "",
            "When you visit an unknown land, the Diplomat will convince a few people to join your crew."
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
