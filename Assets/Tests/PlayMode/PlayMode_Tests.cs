using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayMode_Tests {
    private GameObject player;
    private PlayerAttributes playerAtr;
    private CombatSystem playerComb;

    [OneTimeSetUp]
    public void LoadScene() {
        SceneManager.LoadScene("GameScene");
    }

    //// A Test behaves as an ordinary method
    //[Test]
    //public void PlayMode_TestsSimplePasses()     {

    //}

    [UnityTest]
    public IEnumerator IsPlayerInScene_Test() {
        player = GameObject.Find("PlayerArmature");
        yield return null;
        Assert.AreNotEqual(player, null);
    }


    [UnityTest]
    public IEnumerator HasPlayerHealth_Test() {
        playerAtr = GameObject.Find("PlayerArmature").GetComponent<PlayerAttributes>();
        var currentHealth = playerAtr.currentHealth;
        yield return null;
        Assert.AreNotEqual(currentHealth, 0);
    }

    [UnityTest]
    public IEnumerator PlayerLoosHealth_Test() {
        playerAtr = GameObject.Find("PlayerArmature").GetComponent<PlayerAttributes>();
        playerComb = GameObject.Find("PlayerArmature").GetComponent<CombatSystem>();

        var StartHealth = playerAtr.currentHealth;
        playerComb.LoseHealth(10);
        var NewCurrentHealth = playerAtr.currentHealth;

        yield return null;
        Assert.IsTrue(StartHealth > NewCurrentHealth);
    }

    [UnityTest]
    public IEnumerator PlayerUsePoison_Test() {
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
        playerComb.StartCoroutine(playerComb.BecomeTemporarilyInvincible());

        playerComb.LoseHealth(10);
        var NewCurrentHealth = playerAtr.currentHealth;

        playerComb.invincible = false;
        Debug.Log(StartHealth + " " + NewCurrentHealth);

        yield return null;
        Assert.IsTrue(StartHealth == NewCurrentHealth);
    }
}
