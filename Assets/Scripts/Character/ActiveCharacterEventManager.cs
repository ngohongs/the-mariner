using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveCharacterEventManager {

    public static event Action<ESkill> OnCharacterClicked;
    
    public static void CharacterClicked(ESkill skill) {
        OnCharacterClicked?.Invoke(skill);
    }

    public static event Action<Character> OnCharacterAdded;
    public static void CharacterAdded(Character c) {
        OnCharacterAdded?.Invoke(c);
    }

}
