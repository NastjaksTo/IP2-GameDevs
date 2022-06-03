using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static SkillTree;


public class Skill : MonoBehaviour
{
    public int id; // Integer for the ID each Skill gets

    private GameObject PlayerSP; //Reference of Player Skillpoints (SP)
    private PlayerSkillsystem playerskillsystem; //Reference of Player Skillpoints (SP)

    public TMP_Text TitleText; //Reference of Skill Title
    public TMP_Text DescText; //Reference of Skill Description

    public int[] ConnectedSkills; //Reference List of Skills, which are conntected to eachother



    private void Start()
    {
        PlayerSP = GameObject.Find("PlayerArmature"); // Get Player reference
        playerskillsystem = PlayerSP.GetComponent<PlayerSkillsystem>(); // Get Player reference
    }


    public void UpdateUI() // Update the SkillUI with Text and Descriptions
    {
        TitleText.text = $"{skillTree.skillLevels[id]}/{skillTree.skillCaps[id]}";
        //DescText.text = $"{skillTree.skillDescription[id]}";

        
        foreach (var connectedSkill in ConnectedSkills)
        {
            // SkillTree Skill Visuals
            skillTree.skillList[connectedSkill].gameObject.SetActive(skillTree.skillLevels[id] > 4);
            // SkillTree Connection Visuals
            //skillTree.connectorList[connectedSkill].SetActive(skillTree.skillLevels[id] > 0);
        }

    }

    public void Buy() // Buying / Unlocking Skills
    {
        if (skillTree.skillPoints < 1 || skillTree.skillLevels[id] >= skillTree.skillCaps[id]) return; // Check if skill is buyable
        playerskillsystem.playerlevel.skillpoints -= 1; // Reduce skillpoints by 1 (Price of upgrading a skill)

        
        if (id == 3 & skillTree.skillLevels[3] <= 4) playerskillsystem.ManageHealth1();
        if (id == 4 & skillTree.skillLevels[4] <= 4) playerskillsystem.ManageMana1();
        if (id == 10 & skillTree.skillLevels[10] <= 4) playerskillsystem.ManageMana2();
        if (id == 5 & skillTree.skillLevels[5] <= 4) playerskillsystem.ManageStamina1();
        if (id == 11 & skillTree.skillLevels[11] <= 4) playerskillsystem.ManageStamina2();
        
        
        skillTree.skillLevels[id]++; // Upgrade the SkillLevel
        skillTree.UpdateAllSkillUI(); // Update the SkillUI
    }

}
