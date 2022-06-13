using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerAttributes;

public class PlayerDisplay : MonoBehaviour
{
    public static PlayerDisplay playerDisplay;
    
    public GameObject spellUI;
    public Sprite fire;
    public Sprite ice;
    public Sprite earth;
    public Sprite empty;

    private void Awake() => playerDisplay = this;  

    public void UpdateSpellUI()
    {
        if (playerAttributesScript.fireKnowladgeEquiped)
        {
            spellUI.GetComponent<Image>().sprite = fire;
        }
        else if (playerAttributesScript.iceKnowladgeEquiped)
        {
            spellUI.GetComponent<Image>().sprite = ice;
        }
        else if (playerAttributesScript.earthKnowladgeEquiped)
        {
            spellUI.GetComponent<Image>().sprite = earth;
        }
        else spellUI.GetComponent<Image>().sprite = empty;
    }
}
