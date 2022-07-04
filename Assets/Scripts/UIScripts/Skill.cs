using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static SkillTree;


public class Skill : MonoBehaviour
{
    public int id;                                      // Integer for the ID each Skill gets.

    private GameObject playerSp;                        // Reference of players skillpoints.
    
    private PlayerSkillsystem playerskillsystem;        // Reference of the PlayerSkillsystem script.
    public PlayerAttributes playerAttribute;            // Reference of the PlayerAttributes script.

    public TMP_Text titleText;                          // Reference of UI text element where the title gets stored.

    public int[] connectedSkills;                       // Integer list of skills, which are conntected to eachother.

    /// <summary>
    /// Gets the player reference and the skillsystem reference at the start.
    /// </summary>
    private void Start()
    {
        playerSp = GameObject.Find("PlayerArmature");
        playerskillsystem = playerSp.GetComponent<PlayerSkillsystem>();
    }

    /// <summary>
    /// Updates the SkillUI with skill level/skill cap and connections.
    /// </summary>
    public void UpdateUI() 
    {
        titleText.text = $"{skillTree.skillLevels[id]}/{skillTree.skillCaps[id]}";
        foreach (var connectedSkill in connectedSkills)
        {
            skillTree.skillList[connectedSkill].gameObject.SetActive(skillTree.skillLevels[id] > 4);
        }
    }

    /// <summary>
    /// Upgrades the skills.
    /// Unlocks next skills.
    /// Activates certain skill features.
    /// </summary>
    public void Buy() // Buying / Unlocking Skills
    {
        if (skillTree.skillPoints < 1 || skillTree.skillLevels[id] >= skillTree.skillCaps[id]) return; 
        playerskillsystem.playerlevel.skillpoints -= 1; 

        if (id == 10 & skillTree.skillLevels[10] <= 4) playerskillsystem.ManageMana2();
        if (id == 11 & skillTree.skillLevels[11] <= 4) playerskillsystem.ManageStamina2();
        if (id == 17 & skillTree.skillLevels[17] <= 0) playerskillsystem.ManageStamina3();

        skillTree.skillLevels[id]++; 
        if (id == 3 & skillTree.skillLevels[3] <= skillTree.skillCaps[3]) skillTree.healthSkillvalue += 10 * skillTree.skillLevels[3];
        if (id == 4 & skillTree.skillLevels[4] <= skillTree.skillCaps[4]) skillTree.manaSkillvalue += 10 * skillTree.skillLevels[4];
        if (id == 5 & skillTree.skillLevels[5] <= skillTree.skillCaps[5]) skillTree.staminaSkillvalue += 10 * skillTree.skillLevels[5];
        skillTree.UpdateAllSkillUI(); 
        playerAttribute.AttributeModified(); ;
    }

}
