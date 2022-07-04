using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;
using static PlayerAttributes;

public class Earth1 : MonoBehaviour
{
    public static float dmgredcution;       // Float to save the damage reduction in.
    public static bool earth1IsActive;      // Bool to check whether or not the earthspell is active.
    
    /// <summary>
    /// Sets the damage reduction according to the players skilllevel.
    /// Starts the coroutine for the spell.
    /// </summary>
    private void Awake()
    {
        if (skillTree.skillLevels[2] == 0)
        {
            dmgredcution = 0.9f - playerAttributesScript.magicDamage / 200;
        }
        if (skillTree.skillLevels[2] == 1)
        {
            dmgredcution = 0.8f - playerAttributesScript.magicDamage / 200;
        }
        if (skillTree.skillLevels[2] == 2)
        {
            dmgredcution = 0.7f - playerAttributesScript.magicDamage / 200;
        }
        if (skillTree.skillLevels[2] == 3)
        {
            dmgredcution = 0.6f - playerAttributesScript.magicDamage / 200;
        }
        if (skillTree.skillLevels[2] == 4)
        {
            dmgredcution = 0.5f - playerAttributesScript.magicDamage / 200;
        }
        if (skillTree.skillLevels[2] == 5)
        {
            dmgredcution = 0.4f - playerAttributesScript.magicDamage / 200;
        }
        StartCoroutine(Earth1Duration());
    }

    /// <summary>
    /// Activates the spell for a set amount of time.
    /// </summary>
    /// <returns></returns>
    IEnumerator Earth1Duration()
    {
        earth1IsActive = true;
        Debug.Log("earth1isactive");
        yield return new WaitForSecondsRealtime(19);
        earth1IsActive = false;
        Debug.Log("earth1isnotactive");
    }
}
