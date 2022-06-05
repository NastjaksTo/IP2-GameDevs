using System.Collections;
using System.Collections.Generic;
using GameUI;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Display the playerstats
/// </summary>
public class PlayerStatDisplay : MonoBehaviour
{
    public PlayerAttributes player;
    public UiScreenManager screenManager;

    public Image healthBar;
    private float maxHealth;
    private float currentHealth;
    
    public Image staminaBar;
    private float maxStamina;
    private float currentStamina;


    public Image manaBar;
    private float maxMana;
    private float currentMana;


    /// <summary>
    /// Update each frame the values to display it. Open the Death screen when the player have no healthpoint.
    /// </summary>
    void Update()
    {
        maxHealth = player.maxHealth;
        currentHealth = player.currentHealth;
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth == 0) {
            Debug.Log("tot");
            screenManager.OpenDeathUi();
        }

        maxStamina = player.maxStamina;
        currentStamina = player.currentStamina;
        staminaBar.fillAmount = currentStamina / maxStamina;

        maxMana = player.maxMana;
        currentMana = player.currentMana;
        manaBar.fillAmount = currentMana / maxMana;

    }
}
