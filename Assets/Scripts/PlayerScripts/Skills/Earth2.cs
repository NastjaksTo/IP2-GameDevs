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

    
    public List<int> potionTickTimer = new List<int>();
    
    private void Awake()
    {
        if (skillTree.skillLevels[2] == 0)
        {
            dmgredcution = 0.9f;
        }
        if (skillTree.skillLevels[2] == 1)
        {
            dmgredcution = 0.8f;
        }
        if (skillTree.skillLevels[2] == 2)
        {
            dmgredcution = 0.7f;
        }
        if (skillTree.skillLevels[2] == 3)
        {
            dmgredcution = 0.6f;
        }
        if (skillTree.skillLevels[2] == 4)
        {
            dmgredcution = 0.5f;
        }
        if (skillTree.skillLevels[2] == 5)
        {
            dmgredcution = 0.4f;
        }

        StartCoroutine(Earth2Duration());
    }
    
        
    public IEnumerator regeneratingHealth()
    {
        regenerationTimer = 0.5f - skillTree.skillLevels[8] * 0.23f;
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

    public void applypotion(int ticks)
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
        yield return new WaitForSecondsRealtime(19);
        earth2IsActive = false;
        Debug.Log("earth2isnotactive");
    }

    private void Update()
    {
        applypotion(1 * (1 + skillTree.skillLevels[8]));
    }
}
