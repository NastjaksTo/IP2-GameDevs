using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;


public class PlayerSkillsystem : MonoBehaviour
{
    public LevelSystem playerlevel; // Get LevelSystem reference

    private bool _learnedFire1; // Boolean for unlocking FireSpell
    private bool _learnedIce1; // Boolean for unlocking IceSpell
    private bool _learnedEarth1; // Boolean for unlocking EarthSpell
    
    public GameObject fire1; // FireSpell 1 reference
    public GameObject fire2; // FireSpell 2 reference
    public GameObject fire3; // FireSpell 3 reference
    
    public GameObject ice1; // IceSpell 1 reference
    public GameObject ice2; // IceSpell 2 reference
    
    public GameObject earth1; // EarthSpell 1 reference
    
    public Transform spawner; // SpellSpawner reference
    public GameObject Skilltree; // SkillTreeUI reference
    
    private float spellCooldown = 1f; // Cooldown for spells
    private bool _cooldown = true; // Cooldown Boolean for spells
    
    private void Awake()
    {
        playerlevel = new LevelSystem(); // Create new LevelSystem for the player
    }

    private void OnTriggerEnter(Collider other)
    {
        // EXP Tiktak VORÜBERGEHENDER PLATZHALTER
        if (other.gameObject.tag == "exp") 
        {
            playerlevel.AddExp(50);
            Debug.Log("added 50 exp, your current Level is:" + playerlevel.getLevel());
            Debug.Log("You need" + playerlevel.getExpToLevelUp() + "EXP to level up");
        }
        // Unlocking FireSpell 1
        if (other.gameObject.CompareTag("Fire1"))
        {
            Debug.Log("Fire unlocked.");
            _learnedFire1 = true;
            Destroy(other.gameObject);
        }
        // Unlocking IceSpell 1
        if (other.gameObject.CompareTag("Ice1"))
        {
            Debug.Log("Ice unlocked.");
            _learnedIce1 = true;
            Destroy(other.gameObject);
        }
        // Unlocking EarthSpell 1
        if (other.gameObject.CompareTag("Earth1"))
        {
            Debug.Log("Earth unlocked.");
            _learnedEarth1 = true;
            Destroy(other.gameObject);
        }
    }

    public int ReturnSp() // Return Skillpoints
    {
        return playerlevel.getSP();
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
    
    private static void MouseLock() // Locking mouse when exiting SkillMenu
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private static void MouseUnlock() // Unlocking mouse when interacting with SkillMenu
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void castfire()
    {
        if (_learnedFire1)
        {
            if (skillTree.skillLevels[12] > 0)
            {
                if (!_cooldown) return;
                var newfireball3 = Instantiate(fire3, transform.position+(transform.forward*10), transform.rotation * Quaternion.Euler (0f, 180f, 0f));
                Destroy(newfireball3, 2);
                CooldownStart();
            } else if (skillTree.skillLevels[6] > 0)
            {
                if (!_cooldown) return;
                var newfireball2 = Instantiate(fire2, spawner.position, transform.rotation);
                newfireball2.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 20f; //* (2 * skillTree.SkillLevels[0]);
                Destroy(newfireball2, 2);
                CooldownStart();
            } else
            {
                if (!_cooldown) return;
                var newfireball1 = Instantiate(fire1, spawner.position, transform.rotation);
                newfireball1.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 20f; //* (2 * skillTree.SkillLevels[0]);
                Destroy(newfireball1, 2);
                CooldownStart();
            }
        } else Debug.Log("Skill not learned yet.");
    } // Cast FireSpells
    
    private void castice()
    {
        if (_learnedIce1)
        {
            if (skillTree.skillLevels[13] > 0)
            {
                if (!_cooldown) return;
                var newice1 = Instantiate(ice1, spawner.position, Camera.main.transform.rotation);
                newice1.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 10f; //* (2 * skillTree.SkillLevels[0]);
                Destroy(newice1, 2);
                CooldownStart();
            } else if (skillTree.skillLevels[7] > 0)
            {
                if (!_cooldown) return;
                var newice2 = Instantiate(ice2, transform.position+(transform.forward*2), transform.rotation);
                Destroy(newice2, 3);
                CooldownStart();
            } else
            {
                if (!_cooldown) return;
                var newice1 = Instantiate(ice1, spawner.position, Camera.main.transform.rotation);
                newice1.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 10f; //* (2 * skillTree.SkillLevels[0]);
                Destroy(newice1, 2);
                CooldownStart();
            }
        } else Debug.Log("Skill not learned yet.");
    } // Cast IceSpells
    
    private void castearth()
    {
        if (_learnedEarth1)
        {
            if (skillTree.skillLevels[14] > 0)
            {
                if (!_cooldown) return;
                var newearth3 = Instantiate(earth1, transform.position, transform.rotation);
                newearth3.transform.parent = gameObject.transform;
                Destroy(newearth3, 10);
                CooldownStart();
            } else if (skillTree.skillLevels[8] > 0)
            {
                if (!_cooldown) return;
                var newearth2 = Instantiate(earth1, transform.position, transform.rotation);
                newearth2.transform.parent = gameObject.transform;
                Destroy(newearth2, 10);
                CooldownStart();
            } else
            {
                if (!_cooldown) return;
                var newearth1 = Instantiate(earth1, transform.position, transform.rotation);
                newearth1.transform.parent = gameObject.transform;
                Destroy(newearth1, 10);
                CooldownStart();
            }
        } else Debug.Log("Skill not learned yet.");
    } // Cast EarthSpells

    private void Update()
    {
        if (Input.GetButtonDown("Fire2")) // Casting FireSpell when pressing RMB
        {
            castfire(); 
        }
        
        if (Input.GetKeyDown(KeyCode.E)) // Casting IceSpell when pressing E
        {
            castice(); // 
        }
        
        if (Input.GetKeyDown(KeyCode.Q)) // Casting EarthSpell when pressing Q
        {
           castearth();
        }
        
        if (Input.GetKeyDown(KeyCode.K)) // Opening and closing SkillMenu + Unlocking/Locking mouse accordingly
        {
            if (Skilltree.activeSelf)
            {
                MouseLock();
                Skilltree.SetActive(!Skilltree.activeSelf);
            } else
            {
                MouseUnlock();
                Skilltree.SetActive(!Skilltree.activeSelf);
            }
        }
    }
}
