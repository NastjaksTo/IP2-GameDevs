using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;
using static PlayerAttributes;

public class Earth2 : MonoBehaviour
{
    public static float dmgredcution;                           // Float to save the damage reduction in.
    public static bool earth2IsActive;                          // Bool to check whether or not the earthspell is active.
    public float regenerationTimer;                             // Float to save the regeneration time.
    
    public List<float> spellTickTimer = new List<float>();      // Float list to save the each tick the spell goes through.
    
    /// <summary>
    /// Sets the damage reduction according to the players skilllevel.
    /// Starts the coroutine for the spell.
    /// Applies the healing effect.
    /// </summary>
    private void Awake()
    {
        dmgredcution = 0.4f - playerAttributesScript.magicDamage / 200;
        StartCoroutine(Earth2Duration());
        applypotion(1 * ((1 + (skillTree.skillLevels[8]) / 1.125f)) + playerAttributesScript.magicDamage / 2);
    }

    /// <summary>
    /// Increases the health of the player over time.
    /// </summary>
    /// <returns></returns>
    public IEnumerator regeneratingHealth()
    {
        regenerationTimer = 1 * (0.25f - (0.05f * skillTree.skillLevels[8]));
        while (spellTickTimer.Count > 0)
        {
            for (int i = 0; i < spellTickTimer.Count; i++)
            {
                spellTickTimer[i]--;
            }
            if (playerAttributesScript.currentHealth < playerAttributesScript.maxHealth) playerAttributesScript.currentHealth += 0.01f;
            else spellTickTimer.Clear();
            spellTickTimer.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(regenerationTimer);
        }
    }

    /// <summary>
    /// Calls the coroutine to regenerate health over time, for each tick.
    /// </summary>
    /// <param name="ticks">The amount of ticks.</param>
    public void applypotion(float ticks)
    {
        if (spellTickTimer.Count <= 0)
        {
            spellTickTimer.Add(ticks);
            StartCoroutine(regeneratingHealth());
        }
        else spellTickTimer.Add(ticks);
    }

    /// <summary>
    /// Activates and deactivates the spell after a set amount of time.
    /// </summary>
    /// <returns></returns>
    IEnumerator Earth2Duration()
    {
        earth2IsActive = true;
        Debug.Log("earth2isactive");
        yield return new WaitForSeconds(19);
        earth2IsActive = false;
        Debug.Log("earth2isnotactive");
    }
}
