using System.Collections;
using System.Collections.Generic;
using GameUI;
using GameUI.Scripts;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;
using static PlayerSkillsystem; 

/// <summary>
/// Display the playerstats (nur Balken)
/// </summary>
public class PlayerStatDisplay : MonoBehaviour
{
    public PlayerAttributes player;
    public UiScreenManager screenManager;

    public Image healthBar;
    public TextMeshProUGUI HealthText;

    public Image staminaBar;
    public TextMeshProUGUI StaminaText;

    public Image XPBar;
    public TextMeshProUGUI XPBarText;

    public Image manaBar;
    public TextMeshProUGUI ManaText;



    /// <summary>
    /// Update each frame the values to display it. Open the Death screen when the player have no healthpoint.
    /// </summary>
    void Update() {
    
        healthBar.fillAmount = player.currentHealth / player.maxHealth;
        HealthText.text = Math.Round((Decimal)player.currentHealth, 0, MidpointRounding.AwayFromZero).ToString();


        XPBar.fillAmount = playerskillsystem.playerlevel.GetExp() / playerskillsystem.playerlevel.GetExpToLevelUp();
        XPBarText.text = Math.Round((Decimal)playerskillsystem.playerlevel.GetExp(), 0, MidpointRounding.AwayFromZero).ToString();


        if (player.currentHealth == 0) {
            Debug.Log("tot");
            screenManager.OpenDeathUi();
        }
       
        staminaBar.fillAmount = player.currentStamina / player.maxStamina;
        StaminaText.text = Math.Round((Decimal)player.currentStamina, 0, MidpointRounding.AwayFromZero).ToString();

        manaBar.fillAmount = player.currentMana / player.maxMana;
        ManaText.text = Math.Round((Decimal)player.currentMana, 0, MidpointRounding.AwayFromZero).ToString();

    }
}
