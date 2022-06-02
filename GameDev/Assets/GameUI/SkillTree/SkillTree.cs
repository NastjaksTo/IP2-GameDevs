using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static LevelSystem;

public class SkillTree : MonoBehaviour
{
    public static SkillTree skillTree;
    private void Awake() => skillTree = this;

    public int[] skillLevels;
    public int[] skillCaps;
    public string[] skillNames;
    public string[] skillDescription;
    
    public TMP_Text SPUi;

    public List<Skill> skillList;
    public GameObject skillHolder;

    public List<GameObject> connectorList;
    public GameObject connectorHolder;

    private GameObject _playerSp;
    private PlayerSkillsystem playerskillsystem;

    public int skillPoints;

    private void Start()
    {
        
        _playerSp = GameObject.Find("PlayerArmature"); // Get Player reference
        playerskillsystem = _playerSp.GetComponent<PlayerSkillsystem>(); // Get Player Skillsystem reference

        skillPoints = playerskillsystem.ReturnSp(); // Get Player skillpoints

        skillLevels = new int[18]; // Array of Skills
        skillCaps = new[] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 1, 1, 1, 1, 1, 1 }; //Level Cap of each skill in order

        // Lists the Names of each Skill in order
        skillNames = new[] { "Fire 1", "Ice Spell 1", "Earth Spell 1", "Health 1", "Mana 1", "Stamina 1", 
            "Fire Spell 2", "Ice Spell 2", "Earth Spell 2", "Health 2", "Mana 2", "Stamina 2", 
            "Fire Spell 3", "Ice Spell 3", "Earth Spell 3", "Health 3", "Mana 3", "Stamina 3",};

        // Lists the Description of each Skill in order
        skillDescription = new[] 
        {
            "Increases Fire1 Damage.",
            "Increases Ice1 Damage.",
            "Increases Earth1 Damage.",
            "Increases Health1.",
            "Increases Mana1.",
            "Increases Stamina1.",
            "Increases Fire2 Damage.",
            "Increases Ice2 Damage.",
            "Increases Earth2 Damage.",
            "Increases Health2.",
            "Increases Mana2.",
            "Increases Stamina2.",
            "Increases Fire3 Damage.",
            "Increases Ice3 Damage.",
            "Increases Earth3 Damage.",
            "Increases Health3.",
            "Increases Mana3.",
            "Increases Stamina3."
        };

        foreach (var skill in skillHolder.GetComponentsInChildren<Skill>()) skillList.Add(skill); // Get Skills from SkillHolder object and add Skills in SkillList
        foreach (var connector in connectorHolder.GetComponentsInChildren<RectTransform>()) connectorList.Add(connector.gameObject); // Get Connectors from ConnectorHolder object and add Connector in ConnectorList
 
        for (var i = 0; i < skillList.Count; i++) skillList[i].id = i; // Sets ID to each Skill in order
        
        // Connects Skills to unlock eachother
        skillList[0].ConnectedSkills = new[] {6};
        skillList[1].ConnectedSkills = new[] {7};
        skillList[2].ConnectedSkills = new[] {8};
        skillList[3].ConnectedSkills = new[] {9};
        skillList[4].ConnectedSkills = new[] {10};
        skillList[5].ConnectedSkills = new[] {11};
        
        skillList[6].ConnectedSkills = new[] {12};
        skillList[7].ConnectedSkills = new[] {13};
        skillList[8].ConnectedSkills = new[] {14};
        skillList[9].ConnectedSkills = new[] {15};
        skillList[10].ConnectedSkills = new[] {16};
        skillList[11].ConnectedSkills = new[] {17};

        UpdateAllSkillUI(); // Update UI for each skill
    }


    public void UpdateAllSkillUI()
    {
        skillPoints = playerskillsystem.ReturnSp(); // Get Player skillpoints
        SPUi.text = $"Remaining Skillpoints: {skillPoints}";
        foreach (var skill in skillList) skill.UpdateUI(); //Update UI for each skill
    }

}
