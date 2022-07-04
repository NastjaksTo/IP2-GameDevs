using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Task = System.Threading.Tasks.Task;

public class PlayMode_Tests {
    private GameObject player;
    private PlayerAttributes playerAtr;
    private CombatSystem playerComb;
    private PlayerSkillsystem playerSkillsystem;
    private SkillTree skillTree;


    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("GameScene");
        //await Task.Delay(1);
        //GameObject.Find("Bosses").SetActive(false);
    }

    [UnityTest]
    public IEnumerator IsPlayerInScene_Test() {
        player = GameObject.Find("PlayerArmature");
        yield return null;
        Assert.AreNotEqual(player, null);
    }


    [UnityTest]
    public IEnumerator HasPlayerHealth_Test()
    {
        playerAtr = GameObject.Find("PlayerArmature").GetComponent<PlayerAttributes>();
        var currentHealth = playerAtr.currentHealth;
        yield return null;
        Assert.AreNotEqual(currentHealth, 0);
    }

    [UnityTest]
    public IEnumerator PlayerLoseHealth_Test() {
        playerAtr = GameObject.Find("PlayerArmature").GetComponent<PlayerAttributes>();
        playerComb = GameObject.Find("PlayerArmature").GetComponent<CombatSystem>();

        var StartHealth = playerAtr.currentHealth;
        playerComb.LoseHealth(10);
        var CurrentHealth = playerAtr.currentHealth;

        yield return null;
        Assert.IsTrue(StartHealth > CurrentHealth);
    }
    
    [UnityTest]
    public IEnumerator PlayerLoseHealthWithArmor_Test() {
        playerAtr = GameObject.Find("PlayerArmature").GetComponent<PlayerAttributes>();
        playerComb = GameObject.Find("PlayerArmature").GetComponent<CombatSystem>();

        playerAtr.currentArmor = 50;
        var StartHealth = playerAtr.currentHealth;
        playerComb.LoseHealth(50);
        var CurrentHealth = StartHealth - (50 * 0.5f);
        playerAtr.currentArmor = 0;
        
        yield return null;
        Assert.AreEqual(playerAtr.currentHealth, CurrentHealth);
    }
    
    [UnityTest]
    public IEnumerator PlayerLoseHealthWithEarthSpell_Test() {
        playerAtr = GameObject.Find("PlayerArmature").GetComponent<PlayerAttributes>();
        playerComb = GameObject.Find("PlayerArmature").GetComponent<CombatSystem>();
        playerSkillsystem = GameObject.Find("PlayerArmature").GetComponent<PlayerSkillsystem>();
        
        playerSkillsystem.CastEarth();
        yield return null;
        var StartHealth = playerAtr.currentHealth;
        playerComb.LoseHealth(10);
        var CurrentHealth = playerAtr.currentHealth;
        float dmgredcution = Earth1.dmgredcution;

        yield return null;
        Assert.AreEqual(StartHealth-(10*dmgredcution), CurrentHealth);
    }
    
    [UnityTest]
    public IEnumerator PlayerUsePoison_Test()
    {
        Time.timeScale = 1f;
        playerAtr = GameObject.Find("PlayerArmature").GetComponent<PlayerAttributes>();
        playerComb = GameObject.Find("PlayerArmature").GetComponent<CombatSystem>();

        var StartHealth = playerAtr.currentHealth;
        playerComb.LoseHealth(30);
  
        yield return null;
        var NewCurrentHealth = playerAtr.currentHealth;

        playerComb.applypotion(200);
        yield return new WaitForSeconds(10);
        var regHealth = playerAtr.currentHealth;

        Debug.Log(StartHealth +" "+ NewCurrentHealth +" "+ regHealth);
        Assert.IsTrue(regHealth > NewCurrentHealth);
    }

    [UnityTest]
    public IEnumerator PlayerIsInvincible_Test() {
        playerAtr = GameObject.Find("PlayerArmature").GetComponent<PlayerAttributes>();
        playerComb = GameObject.Find("PlayerArmature").GetComponent<CombatSystem>();

        var StartHealth = playerAtr.currentHealth;
        playerComb.Dodging();

        playerComb.LoseHealth(10);
        var NewCurrentHealth = playerAtr.currentHealth;
        
        playerComb.invincible = false;
        Debug.Log(StartHealth + " " + NewCurrentHealth);

        yield return null;
        Assert.IsTrue(StartHealth == NewCurrentHealth);
    }
    
    [UnityTest]
    public IEnumerator PlayerRevive_Test() {
        playerAtr = GameObject.Find("PlayerArmature").GetComponent<PlayerAttributes>();
        playerComb = GameObject.Find("PlayerArmature").GetComponent<CombatSystem>();
        playerSkillsystem = GameObject.Find("PlayerArmature").GetComponent<PlayerSkillsystem>();
        skillTree = GameObject.Find("SkillTree").GetComponent<SkillTree>();

        Debug.Log(playerAtr.currentHealth);
        skillTree.skillLevels[15] = 1;
        Debug.Log(skillTree.skillLevels[15]);

        yield return null;

        playerComb.LoseHealth(99);
        Debug.Log(playerAtr.currentHealth);

        yield return null;
        Assert.AreEqual(playerAtr.currentHealth, playerAtr.maxHealth);
    }
    
    [UnityTest]
    public IEnumerator zPlayerCanDie_Test() {
        playerAtr = GameObject.Find("PlayerArmature").GetComponent<PlayerAttributes>();
        playerComb = GameObject.Find("PlayerArmature").GetComponent<CombatSystem>();
        
        Debug.Log(playerAtr.currentHealth);
        playerComb.justrevived = true;
        playerAtr.currentHealth = 100;
        Debug.Log(playerAtr.currentHealth);
        playerComb.LoseHealth(200);
        playerComb.justrevived = false;
        Debug.Log(playerAtr.currentHealth);
        
        yield return null;
        Assert.IsTrue(UiScreenManager._deathUiOpen);
    }
}
