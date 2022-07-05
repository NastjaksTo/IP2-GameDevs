using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static LevelSystem;


public class SkillTree : MonoBehaviour
{
    public static SkillTree skillTree;              // Creates public static skilltree.
    
    public int[] skillLevels;                       // Integer array of all skill levels.
    public int[] skillCaps;                         // Integer array of maximum level of skills.

    public TMP_Text spUI;                           // Reference to the skillpoint interface.
    public TMP_Text skillPointsInventory;           // Reference to the skillpoint in inventory.

    public List<Skill> skillList;                   // Creates list which safes all skills.
    public GameObject skillHolder;                  // Reference gameobject where each skill reference is stored.

    private GameObject playerSp;                    // Reference to skillpoints.
    private PlayerSkillsystem playerskillsystem;    // Reference to the PlayerSkillsystem script.
    
    public int skillPoints;                         // Integer for skillpoints.
    public int healthSkillvalue;                    // Integer to modify the health value.
    public int manaSkillvalue;                      // Integer to modify the mana value.
    public int staminaSkillvalue;                   // INteger to modify the stamina value.
    
    /// <summary>
    /// When the script instance is loaded, assign this instance to the static skilltree variable.
    /// </summary>
    private void Awake() => skillTree = this;  
    
    /// <summary>
    /// At the start get the reference to the player and the skillsystem.
    /// Saves the current skillpoints.
    /// Sets up length of the SkillTree.
    /// Sets maximum levels of each skill.
    /// Gets GameObjects of each skill.
    /// Sets an ID to each skill.
    /// Connects skills.
    /// Updates the UI.
    /// </summary>
    private void Start()
    {
        playerSp = GameObject.Find("PlayerArmature");
        playerskillsystem = playerSp.GetComponent<PlayerSkillsystem>();

        skillPoints = playerskillsystem.ReturnSp();

        skillLevels = new int[18];
        skillCaps = new[] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 1, 1, 1, 1, 1, 1 };
        
        foreach (var skill in skillHolder.GetComponentsInChildren<Skill>()) skillList.Add(skill);
      
        for (var i = 0; i < skillList.Count; i++) skillList[i].id = i; 
        
        skillList[0].connectedSkills = new[] {6};
        skillList[1].connectedSkills = new[] {7};
        skillList[2].connectedSkills = new[] {8};
        skillList[3].connectedSkills = new[] {9};
        skillList[4].connectedSkills = new[] {10};
        skillList[5].connectedSkills = new[] {11};
        
        skillList[6].connectedSkills = new[] {12};
        skillList[7].connectedSkills = new[] {13};
        skillList[8].connectedSkills = new[] {14};
        skillList[9].connectedSkills = new[] {15};
        skillList[10].connectedSkills = new[] {16};
        skillList[11].connectedSkills = new[] {17};

        UpdateAllSkillUI(); 
    }
    
    /// <summary>
    /// Updates the values inside the SkillUI.
    /// </summary>
    public void UpdateAllSkillUI()
    {
        skillPoints = playerskillsystem.ReturnSp();
        spUI.text = $"Remaining Skillpoints: {skillPoints}";
        skillPointsInventory.text = skillPoints.ToString();
        foreach (var skill in skillList) skill.UpdateUI();
    }

}
