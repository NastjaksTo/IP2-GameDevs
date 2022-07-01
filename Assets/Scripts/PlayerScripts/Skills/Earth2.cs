using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;
using static PlayerAttributes;

public class Earth2 : MonoBehaviour
{
    public static float dmgredcution;
    public static bool earth2IsActive;
    

    public float regenerationTimer;

    private bool canusepotion;

    
    public List<float> potionTickTimer = new List<float>();
    
    private void Awake()
    {
        dmgredcution = 0.4f - playerAttributesScript.magicDamage / 200;
        StartCoroutine(Earth2Duration());
    }
    
        
    public IEnumerator regeneratingHealth()
    {
        regenerationTimer = 0.5f - skillTree.skillLevels[8] * 0.075f;
        while (potionTickTimer.Count > 0)
        {
            for (int i = 0; i < potionTickTimer.Count; i++)
            {
                potionTickTimer[i]--;
            }

            if (playerAttributesScript.currentHealth < playerAttributesScript.maxHealth) playerAttributesScript.currentHealth += 0.20f;
            else potionTickTimer.Clear();
            potionTickTimer.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(regenerationTimer);
        }
    }

    public void applypotion(float ticks)
    {
        if (potionTickTimer.Count <= 0)
        {
            potionTickTimer.Add(ticks);
            StartCoroutine(regeneratingHealth());
        }
        else potionTickTimer.Add(ticks);
    }

    IEnumerator Earth2Duration()
    {
        earth2IsActive = true;
        Debug.Log("earth2isactive");
        yield return new WaitForSeconds(19);
        earth2IsActive = false;
        Debug.Log("earth2isnotactive");
    }

    private void Update()
    {
        applypotion(1 * ((1 + (skillTree.skillLevels[8])/1.125f)) + playerAttributesScript.magicDamage / 2);
    }
}
