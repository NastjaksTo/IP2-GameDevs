using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;


public class PlayerSkillsystem : MonoBehaviour
{
    public LevelSystem playerlevel; // Get LevelSystem reference

    public static PlayerSkillsystem playerskillsystem;
    
    public GameObject fire1; // FireSpell 1 reference
    public GameObject fire2; // FireSpell 2 reference
    public GameObject fire3; // FireSpell 3 reference
    
    public GameObject ice1; // IceSpell 1 reference
    public GameObject ice2; // IceSpell 2 reference
    public GameObject ice3; // IceSpell 3 reference
    
    public GameObject earth1; // EarthSpell 1 reference
    public GameObject earth2; // EarthSpell 2 reference
    public GameObject earth3; // EarthSpell 3 reference
    
    public Transform spawner; // SpellSpawner reference
    
    private float spellCooldown = 1f; // Cooldown for spells
    private bool _cooldown = true; // Cooldown Boolean for spells

    public PlayerAttributes playerattributes;
    
    public GameObject lvlupeffect;
    
    private void Awake()
    {
        playerlevel = new LevelSystem(); // Create new LevelSystem for the player
        playerskillsystem = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        // EXP Tiktak VORÜBERGEHENDER PLATZHALTER
        if (other.gameObject.tag == "exp") 
        {
            playerlevel.AddExp(150);
            Debug.Log("added 150 exp, your current Level is:" + playerlevel.getLevel());
            Debug.Log("You need" + playerlevel.getExpToLevelUp() + "EXP to level up");
        }
    }

    public int ReturnSp() // Return Skillpoints
    {
        return playerlevel.getSP();
    }
    
    public void PlayLvlUpEffect()
    {
        var newLvlUpEffect = Instantiate(lvlupeffect, transform.position + (Vector3.up * 0.35f), transform.rotation * Quaternion.Euler (-90f, 0f, 0f));
        newLvlUpEffect.transform.parent = gameObject.transform;
        Debug.Log("levelup effect");
    }

    
    private void CooldownStart() // Starting the cooldown of spells
    {
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine() // Cooldown of spells
    {
        _cooldown = false;
        yield return new WaitForSeconds(spellCooldown);
        _cooldown = true;
    }

    private void castfire() // Cast FireSpells
    {
        if (skillTree.skillLevels[12] > 0)
        {
            if (!_cooldown) return;
            if (!(playerattributes.currentMana >= 25)) return;
            playerattributes.currentMana -= 25;
            var newfireball3 = Instantiate(fire3, transform.position + (transform.forward * 10),
                transform.rotation * Quaternion.Euler(0f, 180f, 0f));
            Destroy(newfireball3, 2);
            CooldownStart();
        }
        else if (skillTree.skillLevels[6] > 0)
        {
            if (!_cooldown) return;
            if (!(playerattributes.currentMana >= 20)) return;
            playerattributes.currentMana -= 20;
            var newfireball2 = Instantiate(fire2,transform.position+(transform.forward*2), transform.rotation);
            Destroy(newfireball2, 2);
            CooldownStart();
        }
        else
        {
            if (!_cooldown) return;
            if (!(playerattributes.currentMana >= 15)) return;
            playerattributes.currentMana -= 15;
            var newfireball1 = Instantiate(fire1, spawner.position, transform.rotation);
            newfireball1.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 20f; //* (2 * skillTree.SkillLevels[0]);
            Destroy(newfireball1, 2);
            CooldownStart();
        }
    } 
    
    private void castice() // Cast IceSpells
    {
        if (skillTree.skillLevels[13] > 0)
        {
            if (!_cooldown) return;
            if (!(playerattributes.currentMana >= 25)) return;
            playerattributes.currentMana -= 25;
            var newice3 = Instantiate(ice3, transform.position+(transform.forward*10)+(Vector3.up*10f), transform.rotation * Quaternion.Euler (90f, 0f, 0f));
            Destroy(newice3, 6);
            CooldownStart();
        }
        else if (skillTree.skillLevels[7] > 0)
        {
            if (!_cooldown) return;
            if (!(playerattributes.currentMana >= 20)) return;
            playerattributes.currentMana -= 20;
            var newice2 = Instantiate(ice2, transform.position + (transform.forward * 2), transform.rotation);
            Destroy(newice2, 3);
            CooldownStart();
        }
        else
        {
            if (!_cooldown) return;
            if (!(playerattributes.currentMana >= 15)) return;
            playerattributes.currentMana -= 15;
            var newice1 = Instantiate(ice1, spawner.position, Camera.main.transform.rotation);
            newice1.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 30f; //* (2 * skillTree.SkillLevels[0]);
            Destroy(newice1, 2);
            CooldownStart();
        }
    }

    private void castearth() // Cast EarthSpells
    {
        if (skillTree.skillLevels[14] > 0)
        {
            if (!_cooldown) return;
            if (!(playerattributes.currentMana >= 25)) return;
            playerattributes.currentMana -= 25;
            var newearth3 = Instantiate(earth3, transform.position, transform.rotation);
            newearth3.transform.parent = gameObject.transform;
            var newearth2 = Instantiate(earth2, transform.position, transform.rotation);
            Destroy(newearth3, 10);
            Destroy(newearth2, 20);
            CooldownStart();
        }
        else if (skillTree.skillLevels[8] > 0)
        {
            if (!_cooldown) return;
            if (!(playerattributes.currentMana >= 20)) return;
            playerattributes.currentMana -= 20;
            var newearth2 = Instantiate(earth2, transform.position, transform.rotation);
            Destroy(newearth2, 20);
            CooldownStart();
        }
        else
        {
            if (!_cooldown) return;
            if (!(playerattributes.currentMana >= 15)) return;
            playerattributes.currentMana -= 15;
            var newearth1 = Instantiate(earth1, transform.position, transform.rotation);
            Destroy(newearth1, 20);
            CooldownStart();
        }
    } 

    private void Update()
    {
        if (Input.GetButtonDown("Fire2")) // Casting FireSpell when pressing RMB
        {
            if (playerattributes.fireKnowladgeEquiped)
            {
                castfire(); 
            }
            if (playerattributes.iceKnowladgeEquiped)
            {
                castice();
            }
            if (playerattributes.earthKnowladgeEquiped)
            {
                castearth();
            }
        }
    }
}
