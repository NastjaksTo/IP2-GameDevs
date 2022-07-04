using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveScripts
{
    public static class SaveSystem
    {
        /// <summary>
        /// Locally saves the data from gathered from PlayerData in binary files.
        /// </summary>
        /// <param name="levelsystem">Gets data from LevelSystem.</param>
        /// <param name="attributes">Gets data from PlayerAttributes.</param>
        /// <param name="skills">Gets data from SkillTree.</param>
        /// <param name="combatSystem">Gets data from CombatSystem.</param>
        /// <param name="player">Gets data from SaveData.</param>
        public static void SavePlayer(LevelSystem levelsystem, PlayerAttributes attributes, SkillTree skills, CombatSystem combatSystem, 
            PlayerInventory playerInventory, SaveData player, PlayerQuests playerQuests, BossArena bossArena)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player.save";
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(levelsystem, attributes, skills, combatSystem, playerInventory, player, playerQuests, bossArena);

        
            formatter.Serialize(stream, data);
            stream.Close();
        }

        /// <summary>
        /// Loads the locally saved binary files.
        /// </summary>
        /// <returns>Returns data when a file exists, otherwise returns null.</returns>
        public static PlayerData LoadPlayer()
        {
            string path = Application.persistentDataPath + "/player.save";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();
            
                return data;
            }
            else
            {
                Debug.LogError("Save file not found in" + path);
                return null;
            }
        }
    
    }
}
