using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellCooldown : MonoBehaviour
{
    [SerializeField] private Image imageCooldown;
    [SerializeField] private TMP_Text textCooldown;

    public bool isCooldown;
    [SerializeField] private float cooldownTime = 10f;
    [SerializeField] private float cooldownTimer = 5f;

    public static SpellCooldown spellcooldown;

    private void Awake()
    {
        isCooldown = false;
        spellcooldown = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooldown)
        {
            ApplyCooldown();
        }
    }

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
    
    public void UseSpell(float cooldown)
    {
        cooldownTime = cooldown;
        isCooldown = true;
        textCooldown.gameObject.SetActive(true);
        cooldownTimer = cooldownTime;
    }
}
