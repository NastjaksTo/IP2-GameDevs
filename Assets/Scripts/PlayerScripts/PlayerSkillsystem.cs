using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using TMPro;
using static SkillTree;
using static SpellCooldown;
using static PlayerAttributes;


public class PlayerSkillsystem : MonoBehaviour
{
    public LevelSystem playerlevel;                         // Reference to the LevelSystem script.
    public static PlayerSkillsystem playerskillsystem;      // Making static reference of the class.

    public GameObject fire1;                                // Reference of first firespell.
    public GameObject fire2;                                // Reference of second firespell.
    public GameObject fire3;                                // Reference of third firespell.
    
    public GameObject ice1;                                 // Reference of first icespell.
    public GameObject ice2;                                 // Reference of second icespell.
    public GameObject ice3;                                 // Reference of third icespell.
    
    public GameObject earth1;                               // Reference of first earthspell.
    public GameObject earth2;                               // Reference of second earthspell.
    public GameObject earth3;                               // Reference of third earthspell.
    
    public Transform spawner;                               // Reference to the position where spells spawn.
    
    //private int spellCooldown = 5;                       // Float to save the time for the spell cooldown.
    //private bool cooldown = true;                           // Boolean to save the status of the current cooldown.

    public GameObject lvlupeffect;                          // Reference to the level up visual effect.
    public TextMeshProUGUI textCurrentLevel;                // Reference to the UI text element for the current level.

    private Animator anim;
    private CharacterController controller;

    public AudioClip[] spellsounds;
    [Range(0, 1)] public float SpellAudioVolume = 0.5f;
    
    /// <summary>
    /// When the script instance is loaded, create a new levelsystem from the LevelSystem script.
    /// Assign the static playerskillsystem this instance.
    /// </summary>
    private void Awake()
    {
        playerlevel = new LevelSystem(); // Create new LevelSystem for the player
        playerskillsystem = this;
        controller = transform.GetComponent<CharacterController>();
        updateLevelUI();
    }

    /// <summary>
    /// At the start set the LevelUI and the ExperienceUI to its correct values.
    /// </summary>
    private void Start() {
        textCurrentLevel.text = playerlevel.GetLevel().ToString();
        anim = transform.GetComponent<Animator>();
    }

    /// <summary>
    /// Updates the LevelUI and the ExperienceUI.
    /// </summary>
    public void updateLevelUI()
    {
        textCurrentLevel.text = playerlevel.GetLevel().ToString();

    }

    /// <summary>
    /// Returns the current amount of skillpoints.
    /// </summary>
    /// <returns>The current amount of skillpoints.</returns>
    public int ReturnSp() // Return Skillpoints
    {
        return playerlevel.GetSp();
    }

    /// <summary>
    /// Plays the visual effect for level up at the players position.
    /// </summary>
    public void PlayLvlUpEffect()
    {
        AudioSource.PlayClipAtPoint(spellsounds[8],spawner.position, SpellAudioVolume);
        var newLvlUpEffect = Instantiate(lvlupeffect, transform.position + (Vector3.up * 0.35f), transform.rotation * Quaternion.Euler (-90f, 0f, 0f));
        newLvlUpEffect.transform.parent = gameObject.transform;
    }

    /// <summary>
    /// Increases the mana regeneration speed when called.
    /// </summary>
    public void ManageMana2()
    {
        playerAttributesScript.manaRegenerationSpeed += 0.5f;
    }

    /// <summary>
    /// Increases the stamina regeneration speed when called.
    /// </summary>
    public void ManageStamina2()
    {
        playerAttributesScript.staminaRegenerationSpeed += 1.5f;
    }
    
    public void ManageStamina3()
    {
        ThirdPersonController.thirdPersonController.moveSpeed += 4;
        ThirdPersonController.thirdPersonController.SprintSpeed += 5;
    }
    
    /// <summary>
    /// Checks what level the fire skills currently have and instantiates the according fire spell.
    /// </summary>
    private void CastFire()
    {
        if (skillTree.skillLevels[12] > 0)
        {
            if (!(playerAttributesScript.currentMana >= 25)) return;
            playerAttributesScript.currentMana -= 25;
            AudioSource.PlayClipAtPoint(spellsounds[2],transform.position + (transform.forward * 10), SpellAudioVolume);
            anim.SetTrigger("castGroundSpell");
            var newfireball3 = Instantiate(fire3, transform.position + (transform.forward * 10),
                transform.rotation * Quaternion.Euler(0f, 0f, 0f));
            Destroy(newfireball3, 10);
            spellcooldown.UseSpell(15f * (1f - 0.5f * skillTree.skillLevels[16]));
        }
        else if (skillTree.skillLevels[6] > 0)
        {
            if (!(playerAttributesScript.currentMana >= 20)) return;
            playerAttributesScript.currentMana -= 20;
            AudioSource.PlayClipAtPoint(spellsounds[1],transform.position+(transform.forward*2), SpellAudioVolume);
            anim.SetTrigger("castGroundSpell");
            var newfireball2 = Instantiate(fire2,transform.position+(transform.forward*2), transform.rotation);
            Destroy(newfireball2, 2);
            spellcooldown.UseSpell(5f * (1f - 0.5f * skillTree.skillLevels[16]));
        }
        else
        {
            if (!(playerAttributesScript.currentMana >= 15)) return;
            playerAttributesScript.currentMana -= 15;
            AudioSource.PlayClipAtPoint(spellsounds[0],spawner.position, SpellAudioVolume);
            anim.SetTrigger("castTargetSpell");
            var newfireball1 = Instantiate(fire1, spawner.position, transform.rotation);
            newfireball1.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 20f; //* (2 * skillTree.SkillLevels[0]);
            Destroy(newfireball1, 2);
            spellcooldown.UseSpell(5f * (1f - 0.5f * skillTree.skillLevels[16]));
        }
    }

    /// <summary>
    /// Checks what level the ice skills currently have and instantiates the according ice spell.
    /// </summary>
    private void CastIce() // Cast IceSpells
    {
        if (skillTree.skillLevels[13] > 0)
        {
            if (!(playerAttributesScript.currentMana >= 25)) return;
            playerAttributesScript.currentMana -= 25;
            anim.SetTrigger("castGroundSpell");
            AudioSource.PlayClipAtPoint(spellsounds[5],transform.position+(transform.forward*10), SpellAudioVolume);
            var newice3 = Instantiate(ice3, transform.position+(transform.forward*10)+(Vector3.up*10f), transform.rotation * Quaternion.Euler (90f, 0f, 0f));
            Destroy(newice3, 15);
            spellcooldown.UseSpell(15f * (1f - 0.5f * skillTree.skillLevels[16]));
        }
        else if (skillTree.skillLevels[7] > 0)
        {
            if (!(playerAttributesScript.currentMana >= 20)) return;
            playerAttributesScript.currentMana -= 20;
            anim.SetTrigger("castGroundSpell");
            AudioSource.PlayClipAtPoint(spellsounds[4],transform.position+(transform.forward*2), SpellAudioVolume);
            var newice2 = Instantiate(ice2, transform.position + (transform.forward * 2), transform.rotation);
            Destroy(newice2, 3);
            spellcooldown.UseSpell(5f * (1f - 0.5f * skillTree.skillLevels[16]));
        }
        else
        {
            if (!(playerAttributesScript.currentMana >= 15)) return;
            playerAttributesScript.currentMana -= 15;
            AudioSource.PlayClipAtPoint(spellsounds[3],spawner.position, SpellAudioVolume);
            anim.SetTrigger("castTargetSpell");
            var newice1 = Instantiate(ice1, spawner.position, Camera.main.transform.rotation);
            newice1.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 40f; //* (2 * skillTree.SkillLevels[0]);
            Destroy(newice1, 2);
            spellcooldown.UseSpell(5f * (1f - 0.5f * skillTree.skillLevels[16]));
        }
    }

    /// <summary>
    /// Checks what level the earth skills currently have and instantiates the according earth spell.
    /// </summary>
    public void CastEarth() // Cast EarthSpells
    {
        if (skillTree.skillLevels[14] > 0)
        {
            if (!(playerAttributesScript.currentMana >= 25)) return;
            playerAttributesScript.currentMana -= 25;
            anim.SetTrigger("castEarthSpell");
            AudioSource.PlayClipAtPoint(spellsounds[7],transform.position, SpellAudioVolume);
            var newearth3 = Instantiate(earth3, transform.position, transform.rotation);
            var newearth2 = Instantiate(earth2, transform.position, transform.rotation);
            Destroy(newearth3, 10);
            Destroy(newearth2, 18);
            spellcooldown.UseSpell(40f * (1f - 0.5f * skillTree.skillLevels[16]));
        }
        else if (skillTree.skillLevels[8] > 0)
        {
            if (!(playerAttributesScript.currentMana >= 20)) return;
            playerAttributesScript.currentMana -= 20;
            anim.SetTrigger("castEarthSpell");
            var newearth2 = Instantiate(earth2, transform.position, transform.rotation);
            Destroy(newearth2, 18);
            spellcooldown.UseSpell(40f * (1f - 0.5f * skillTree.skillLevels[16]));
        }
        else
        {
            if (!(playerAttributesScript.currentMana >= 15)) return;
            playerAttributesScript.currentMana -= 15;
            anim.SetTrigger("castEarthSpell");
            var newearth1 = Instantiate(earth1, transform.position, transform.rotation);
            Destroy(newearth1, 18);
            spellcooldown.UseSpell(40f * (1f - 0.5f * skillTree.skillLevels[16]));
        }
    }

    public void StartCasting()
    {
        ThirdPersonController.thirdPersonController._canMove = false;
    }

    public void StopCasting()
    {
        ThirdPersonController.thirdPersonController._canMove = true;
    }

    /// <summary>
    /// Whenever RMB (Right Mouse Button // Fire2) is pressed check which spellbook is equiped and cast the according spell.
    /// </summary>
    private void Update()
    {
        if (Input.GetButtonDown("Fire2")) 
        {
            if (playerAttributesScript.fireKnowladgeEquiped)
            {
                if (spellcooldown.isCooldown || !controller.isGrounded) return;
                transform.rotation = Quaternion.Euler( 0, Camera.main.transform.eulerAngles.y, 0);
                CastFire();
            }
            if (playerAttributesScript.iceKnowladgeEquiped)
            {
                if (spellcooldown.isCooldown || !controller.isGrounded) return;
                transform.rotation = Quaternion.Euler( 0, Camera.main.transform.eulerAngles.y, 0);
                CastIce();
            }
            if (playerAttributesScript.earthKnowladgeEquiped)
            {
                if (spellcooldown.isCooldown || !controller.isGrounded) return;
                transform.rotation = Quaternion.Euler( 0, Camera.main.transform.eulerAngles.y, 0);
                CastEarth();
            }
        }
    }
}
