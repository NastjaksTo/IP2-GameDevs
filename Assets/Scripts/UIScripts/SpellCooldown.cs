using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellCooldown : MonoBehaviour
{
    [SerializeField] private Image imageCooldown;           // Reference to the image.
    [SerializeField] private TMP_Text textCooldown;         // Reference to the text.

    public bool isCooldown;                                 // Bool to check whether or not the spell is on cooldown.
    [SerializeField] private float cooldownTime = 10f;      // Float to save the cooldown time.
    [SerializeField] private float cooldownTimer = 5f;      // Float to save the timer for the cooldown.

    public static SpellCooldown spellcooldown;              // Creates a static reference to this script.

    /// <summary>
    /// Assigns each value accordingly.
    /// </summary>
    private void Awake()
    {
        isCooldown = false;
        spellcooldown = this;
    }

    /// <summary>
    /// Sets cooldown to 0 when game starts.
    /// </summary>
    void Start()
    {
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0f;
    }

    /// <summary>
    /// Checks if spell should be on cooldown, if it is then apply the cooldown.
    /// </summary>
    void Update()
    {
        if (isCooldown)
        {
            ApplyCooldown();
        }
    }

    /// <summary>
    /// Applies the cooldown effect.
    /// </summary>
    void ApplyCooldown()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0f)
        {
            isCooldown = false;
            textCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0f;
        }
        else
        {
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = cooldownTimer / cooldownTime;
        }
        
    }
    
    /// <summary>
    /// Use the spell and assign its cooldown.
    /// </summary>
    /// <param name="cooldown">Gets the cooldown time.</param>
    public void UseSpell(float cooldown)
    {
        cooldownTime = cooldown;
        isCooldown = true;
        textCooldown.gameObject.SetActive(true);
        cooldownTimer = cooldownTime;
    }
}
