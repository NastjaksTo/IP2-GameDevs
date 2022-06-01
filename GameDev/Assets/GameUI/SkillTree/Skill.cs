using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static SkillTree;


public class Skill : MonoBehaviour
{
    public int id; // Integer for the ID each Skill gets

    private GameObject PlayerSP; //Reference of Player Skillpoints (SP)
    private PlayerSkillsystem playerSP; //Reference of Player Skillpoints (SP)

    public TMP_Text TitleText; //Reference of Skill Title
    public TMP_Text DescText; //Reference of Skill Description

    public int[] ConnectedSkills; //Reference List of Skills, which are conntected to eachother



    private void Start()
    {
        PlayerSP = GameObject.Find("PlayerArmature"); // Get Player reference
        playerSP = PlayerSP.GetComponent<PlayerSkillsystem>(); // Get Player reference
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
        playerSP.playerlevel.skillpoints -= 1; // Reduce skillpoints by 1 (Price of upgrading a skill)
        skillTree.skillLevels[id]++; // Upgrade the SkillLevel
        skillTree.UpdateAllSkillUI(); // Update the SkillUI
    }

}
