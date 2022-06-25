using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using static SkillTree;
using static PotionCooldown;
using static UiScreenManager;
using static PlayerAttributes;

public class CombatSystem : MonoBehaviour
{
    public static CombatSystem combatSystem;
    public Animator _anim;
    public ThirdPersonController playerMovement;
    private CharacterController controller;

    public GameObject potionUI;
    public int maxpotions;
    public int potions;
    public bool potionlootable = false;
    public GameObject currentPotion;
    public float regenerationTimer;
    public GameObject potioneffect;
    private bool canusepotion;
    public TextMeshProUGUI potionsUI;
    public bool invincible = false;
    public bool justrevived;
    public bool isAttacking;

    private bool inAnimation;
    private bool canDodge = true;
    
    public List<int> potionTickTimer = new List<int>();

    private void Awake()
    {
        combatSystem = this;
        controller = transform.GetComponent<CharacterController>();
    }

    private void Start()
    {
        canusepotion = true;
        maxpotions = 0;
        refillPotions();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Potion"))
        {
            currentPotion = other.gameObject;
            potionUI.SetActive(true);
            potionlootable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Potion"))
        {
            currentPotion = null;
            potionUI.SetActive(false);
            potionlootable = false;
        }
    }

    public void refillPotions()
    {
        potions = maxpotions;
        potionsUI.text = $"{potions}/{maxpotions}";
    }

    IEnumerator dodgeCooldown()
    {
        canDodge = false;
        yield return new WaitForSecondsRealtime(3);
        canDodge = true;
    }
    
    public IEnumerator ReviveCooldown()
    {
        justrevived = true;
        yield return new WaitForSeconds(10);
        justrevived = false;
    }
    
    public IEnumerator regeneratingHealth()
    {
        if (skillTree.skillLevels[9] == 0) regenerationTimer = 0.4f;
        else regenerationTimer = 0.5f - skillTree.skillLevels[9] * 0.19f;
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
    
    public void PlayPotionEffect()
    {
        var newPotionEffect = Instantiate(potioneffect, transform.position + (Vector3.up * 0.35f), transform.rotation * Quaternion.Euler (-90f, 0f, 0f));
        newPotionEffect.transform.parent = gameObject.transform;
        Destroy(newPotionEffect, 5f);
    }
    
    public void LoseHealth(float amount)
    {
        if (invincible) return;

        float damage;
        float spellreduction = 1f;
        bool playerhasrevive = skillTree.skillLevels[15] == 1;
        
        if (Earth1.earth1IsActive) spellreduction = Earth1.dmgredcution;

        if (Earth2.earth2IsActive)
        {
            spellreduction = Earth2.dmgredcution;
            applypotion(100 * (1 + skillTree.skillLevels[8]));
        }

        damage = (amount - playerAttributesScript.currentArmor) * spellreduction;
        if (damage > 0)
        {
            playerAttributesScript.currentHealth -= damage;
        }
        else return;

        if (playerAttributesScript.currentHealth <= 0)
        {
            if (playerhasrevive && !justrevived)
            {
                playerAttributesScript.currentHealth = playerAttributesScript.maxHealth;
                StartCoroutine(ReviveCooldown());
            } else uiScreenManager.OpenDeathUi();
        }
    }

    public void LightAttack(AnimationEvent animationEvent)
    {
        Debug.Log("Lightattack");
        isAttacking = true;
        Invoke(nameof(StopAttack), 0.025f);
    }

    public void StartAttack(AnimationEvent animationEvent)
    {
        Debug.Log("Startattack");
        playerAttributesScript.currentStamina -= 8;
        inAnimation = true;
    }
    
    public void StopAttack()
    {
        Debug.Log("Stopattack");
        isAttacking = false;
        playerMovement._canMove = true;
        inAnimation = false;
    }

    public void Dodging()
    {
        Debug.Log("Startdodging");
        if (!invincible)
        {
            invincible = true;
        }
        StartCoroutine(dodgeCooldown());
        playerAttributesScript.currentStamina -= 20;
        inAnimation = true;
    }

    public void StopDodging(AnimationEvent animationEvent)
    {
        Debug.Log("Stopdodging");
        if (invincible)
        {
            invincible = false;
        }
        inAnimation = false;
    }

    private void Update() 
    {
        if (Input.GetButtonDown("Fire1") && controller.isGrounded && playerAttributesScript.currentStamina >= 8 && playerAttributesScript.hasWeaponEquiped && !_anim.GetCurrentAnimatorStateInfo(0).IsName("dodge") && !_isOneIngameUiOpen && !inAnimation) 
        {
            playerMovement._canMove = false;
            _anim.Play("lightattack");
        }

        if (Input.GetKeyDown(KeyCode.E) && potionlootable && currentPotion != null)
        {
            Destroy(currentPotion);
            maxpotions++;
            potionsUI.text = $"{potions}/{maxpotions}";
            currentPotion = null;
            potionUI.SetActive(false);
            potionlootable = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && canDodge && !inAnimation && controller.isGrounded && playerAttributesScript.currentStamina >= 25 && !_isOneIngameUiOpen) 
        {
            _anim.Play("dodge");
        }

        if (Input.GetKeyDown(KeyCode.G) && potions > 0 && !potioncooldown.isCooldown)
        {
            PlayPotionEffect();
            potions--;
            potionsUI.text = $"{potions}/{maxpotions}";
            applypotion(100 * (1 + skillTree.skillLevels[9]));
            potioncooldown.UsePotion(5);
        }
    }
}
