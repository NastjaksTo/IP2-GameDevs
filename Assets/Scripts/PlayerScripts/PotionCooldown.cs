using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionCooldown : MonoBehaviour
{
    [SerializeField] private Image imageCooldown;
    [SerializeField] private TMP_Text textCooldown;

    public bool isCooldown;
    [SerializeField] private float cooldownTime = 10f;
    [SerializeField] private float cooldownTimer = 5f;

    public static PotionCooldown potioncooldown;

    /// <summary>
    /// Assign values when this script is instantiated.
    /// </summary>
    private void Awake()
    {
        isCooldown = false;
        potioncooldown = this;
    }

    /// <summary>
    /// Reset values at the start of the game.
    /// </summary>
    void Start()
    {
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0f;
    }

    /// <summary>
    /// If potion should be on cooldown, apply the cooldown effect.
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
    /// Uses the potion.
    /// </summary>
    /// <param name="cooldown">Gets the cooldown of the potion.</param>
    public void UsePotion(int cooldown)
    {
        cooldownTime = cooldown;
        isCooldown = true;
        textCooldown.gameObject.SetActive(true);
        cooldownTimer = cooldownTime;
    }
}
