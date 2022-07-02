using System.Collections.Generic;
using StarterAssets;

namespace SaveScripts
{
    [System.Serializable]
    public class PlayerData
    {
        public int level;                       // Integer to save the level.
        public float health;                    // Float to save the health.
        public float[] position;                // Float array to save the position.
        public int[] skilllevels;               // Integer array to save skilllevels.
        public int healthSkillvalue;            // Integer to save the value added to the health from skills.
        public int manaSkillvalue;              // Integer to save the value added to the mana from skills.
        public int staminaSkillvalue;           // Integer to save the value added to the stamina from skills.
        public float manaregenValue;            // Float to save the value added to the mana regeneration from skills.
        public float staminaregenValue;         // Float to save the value added to the stamina regeneration from skills.
        public int maxpotions;                  // Integer to save the maximum amount of potions carryable by the player.
        public float currentExp;                // Float to save the current experience.
        public float expToLvlUp;                // Float to save the experience needed to level up.
        public int currentQuestID;
        public string playerQuestTitle;
        public string playerQuestDesc;
        public string playerQuestReward;
        public bool fireTitanDead;
        public bool iceTitanDead;
        public bool earthTitanDead;
        public float sprintspeed;
        public float movespeed;
    
        public List<int> savedcollectedLootbags = new List<int>();

        /// <summary>
        /// Constructor to save all values.
        /// </summary>
        /// <param name="levelsystem">Gets data from LevelSystem</param>
        /// <param name="attributes">Gets data from PlayerAttributes</param>
        /// <param name="skillTree">Gets data from SkillTree</param>
        /// <param name="combatSystem">Gets data from CombatSystem</param>
        /// <param name="player">Gets data from SaveData</param>
        public PlayerData(LevelSystem levelsystem, PlayerAttributes attributes, SkillTree skillTree, CombatSystem combatSystem, 
            PlayerInventory playerInventory, SaveData player, PlayerQuests playerQuests, BossArena bossArena)
        {
            level = PlayerSkillsystem.playerskillsystem.playerlevel.GetLevel();
            currentExp = PlayerSkillsystem.playerskillsystem.playerlevel.GetExp();
            expToLvlUp = PlayerSkillsystem.playerskillsystem.playerlevel.GetExpToLevelUp();
            health = attributes.currentHealth;
            healthSkillvalue = skillTree.healthSkillvalue;
            manaSkillvalue = skillTree.manaSkillvalue;
            staminaSkillvalue = skillTree.staminaSkillvalue;
            manaregenValue = attributes.manaRegenerationSpeed;
            staminaregenValue = attributes.staminaRegenerationSpeed;
            sprintspeed = ThirdPersonController.thirdPersonController.SprintSpeed;
            movespeed = ThirdPersonController.thirdPersonController.moveSpeed;
            maxpotions = combatSystem.maxpotions;
            savedcollectedLootbags.AddRange(playerInventory.collectedLootbags);
            skilllevels = new int[18];
            currentQuestID = playerQuests.currentQuestID;
            playerQuestTitle = playerQuests.titleText.text;
            playerQuestDesc = playerQuests.descText.text;
            playerQuestReward = playerQuests.rewardText.text;
            iceTitanDead = bossArena.isIceTitanAlive;
            earthTitanDead = bossArena.isEarthTitanAlive;
            fireTitanDead = bossArena.isFireTitanAlive;
            
            for (int i = 0; i <= 17; i++)
            {
                skilllevels[i] = skillTree.skillLevels[i];
            }

            position = new float[3];
            position[0] = player.transform.position.x;
            position[1] = player.transform.position.y;
            position[2] = player.transform.position.z;
        }
    }
}
