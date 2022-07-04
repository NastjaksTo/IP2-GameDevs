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
    public static CombatSystem combatSystem;                        // Creates public static reference to this script.
    public Animator _anim;                                          // Reference to the animator of the player.
    public ThirdPersonController playerMovement;                    // Reference to the ThirdPersonController script.
    private CharacterController controller;                         // Reference to the CharacterController of the player.
    public GameObject potionUI;                                     // Reference to the PotionUI.
    public int maxpotions;                                          // Integer to save the max amount of potions a player has.
    public int potions;                                             // Integer to save the current amount of potions a player has.
    public bool potionlootable = false;                             // Bool to check if a potion is lootable or not.
    public GameObject currentPotion;                                // Reference to the gameobject of the currentpotion the player is interacting with.
    public float regenerationTimer;                                 // Float to save the regeneration timer.
    public GameObject potioneffect;                                 // Reference to the gameobject of the effect of the potion.
    private bool canusepotion;                                      // Bool to check whether or not a potion can be used or not.
    public TextMeshProUGUI potionsUI;                               // Reference to the PotionsUI.
    public bool invincible = false;                                 // Bool to check whether or not the player should be invincible.
    public bool justrevived;                                        // Bool to check whether or not the player just revived.
    public bool isAttacking;                                        // Bool to check whether or not the player is currently attacking.    
    public bool shouldPandoraBlock;                                 // Bool to check whether or not the pandora should block this next attack.
    private bool inAnimation;                                       // Bool to check whether or not the player is in an animation.
    private bool canDodge = true;                                   // Bool to check whether or not the player can dodge.
    
    public List<float> potionTickTimer = new List<float>();         // Float list to save the ticks of the potion.
    
    public AudioClip[] spellsounds;                                 // Array of audioclips.
    [Range(0, 1)] public float SpellAudioVolume = 0.5f;             // Volume of the audioclips.
    public AudioSource HeartBeat;                                   // Reference to the audiosource.
    private bool isHeartBeating;                                    // Bool to check whether or not the "Heartbeating" sound is being played or not.
    
    /// <summary>
    /// Asssigns values and gets the character controller.
    /// </summary>
    private void Awake()
    {
        combatSystem = this;
        controller = transform.GetComponent<CharacterController>();
    }

    /// <summary>
    /// Resets the Potion values.
    /// </summary>
    private void Start()
    {
        canusepotion = true;
        maxpotions = 0;
        refillPotions();
    }
    
    /// <summary>
    /// Check if player is colliding with a lootable potion.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Potion"))
        {
            currentPotion = other.gameObject;
            potionUI.SetActive(true);
            potionlootable = true;
        }
    }

    /// <summary>
    /// Check if player is exiting the collider of a lootable potion.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Potion"))
        {
            currentPotion = null;
            potionUI.SetActive(false);
            potionlootable = false;
        }
    }

    /// <summary>
    /// Sets the current potion to the value of the max amount of potions.
    /// </summary>
    public void refillPotions()
    {
        potions = maxpotions;
        potionsUI.text = $"{potions}/{maxpotions}";
    }

    /// <summary>
    /// Creates a cooldown when dodging, so the player cant dodge again.
    /// </summary>
    /// <returns></returns>
    IEnumerator dodgeCooldown()
    {
        canDodge = false;
        yield return new WaitForSecondsRealtime(3);
        canDodge = true;
    }
    
    /// <summary>
    /// Creates a cooldown when reviving, so the player cant revive again.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ReviveCooldown()
    {
        justrevived = true;
        yield return new WaitForSeconds(240f);
        justrevived = false;
    }
    
    /// <summary>
    /// Increases the health of the player over time.
    /// </summary>
    /// <returns></returns>
    public IEnumerator regeneratingHealth()
    {
        regenerationTimer = 1 * (0.125f - (0.025f * skillTree.skillLevels[9]));
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

    /// <summary>
    /// Calls the coroutine to regenerate health over time, for each tick.
    /// </summary>
    /// <param name="ticks">The amount of ticks.</param>
    public void applypotion(float ticks)
    {
        if (potionTickTimer.Count <= 0)
        {
            potionTickTimer.Add(ticks);
            StartCoroutine(regeneratingHealth());
        }
        else potionTickTimer.Add(ticks);
    }
    
    /// <summary>
    /// Plays the potion effect when a potion is used.
    /// </summary>
    public void PlayPotionEffect()
    {
        var newPotionEffect = Instantiate(potioneffect, transform.position + (Vector3.up * 0.35f), transform.rotation * Quaternion.Euler (-90f, 0f, 0f));
        newPotionEffect.transform.parent = gameObject.transform;
        Destroy(newPotionEffect, 5f);
    }
    
    /// <summary>
    /// Calculates the damage the player recives.
    /// Checks if the player is invincible.
    /// Checks if player has earthspell active.
    /// Checks if player has armor.
    /// Reduces the damage according to these values (Armor, Earthspell).
    /// If player reaches below 0 HP, check if he can revive himself.
    /// If not then the player dies.
    /// </summary>
    /// <param name="amount">Amount of damage the player should recive.</param>
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
        if (playerAttributesScript.currentArmor == 0)
        {
            damage = amount * spellreduction;
        }
        else
        {
            damage = amount - (amount * (playerAttributesScript.currentArmor / 100)) * spellreduction;
        }
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

    /// <summary>
    /// Animation event, which gets called when the player is hitting an enemy.
    /// Play sound and set player is attacking to true.
    /// </summary>
    /// <param name="animationEvent"></param>
    public void LightAttack(AnimationEvent animationEvent)
    {
        AudioSource.PlayClipAtPoint(spellsounds[2],transform.position, SpellAudioVolume);
        isAttacking = true;
    }
    
    /// <summary>
    /// Animation event, which gets called when the player starts attacking.
    /// Set pandoras block to true.
    /// Remove some stamina.
    /// Set the inAnimation bool to true.
    /// </summary>
    /// <param name="animationEvent"></param>
    public void StartAttack(AnimationEvent animationEvent)
    {
        shouldPandoraBlock = true;
        playerAttributesScript.currentStamina -= 8;
        inAnimation = true;
    }
    
    /// <summary>
    /// Animation event, which gets called when the player stops attacking.
    /// Reset all values.
    /// </summary>
    /// <param name="animationEvent"></param>
    public void StopAttack()
    {
        shouldPandoraBlock = false;
        isAttacking = false;
        playerMovement._canMove = true;
        inAnimation = false;
    }
    
    /// <summary>
    /// Player dodges and gets invincible of a short amount of time.
    /// Play Sound.
    /// Make player invincible.
    /// Remove some stamina.
    /// Set inAnimation to true.
    /// </summary>
    public void Dodging()
    {
        AudioSource.PlayClipAtPoint(spellsounds[0],transform.position, SpellAudioVolume);
        if (!invincible)
        {
            invincible = true;
        }
        StartCoroutine(dodgeCooldown());
        playerAttributesScript.currentStamina -= 20;
        inAnimation = true;
    }

    /// <summary>
    /// Animation event, which gets called when the player starts dodging.
    /// Remove the inviniblity effect.
    /// </summary>
    /// <param name="animationEvent"></param>
    public void StopDodging(AnimationEvent animationEvent)
    {
        if (invincible)
        {
            invincible = false;
        }
        inAnimation = false;
    }

    /// <summary>
    /// Check for key press.
    /// On LMB(Left mouse button) start attacking.
    /// On E loot a potion
    /// On C start dodging.
    /// On G use a potion.
    /// Check for HP and play the HeartSound if below 20 HP.
    /// </summary>
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
            AudioSource.PlayClipAtPoint(spellsounds[1],transform.position, SpellAudioVolume);
            potions--;
            potionsUI.text = $"{potions}/{maxpotions}";
            applypotion(100 * (1 + (skillTree.skillLevels[9]/2f)));
            potioncooldown.UsePotion(5);
        }

        if (playerAttributesScript.currentHealth <= 20 && !isHeartBeating)
        {
            HeartBeat.Play();
            isHeartBeating = true;
        } else if (playerAttributesScript.currentHealth >= 21 && isHeartBeating)
        {
            HeartBeat.Stop();
            isHeartBeating = false;
        }
    }
}
