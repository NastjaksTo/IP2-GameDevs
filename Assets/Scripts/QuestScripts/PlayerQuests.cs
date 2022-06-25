using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static QuestGiver;
using static PlayerSkillsystem;
using static PlayerInventory;
using static CombatSystem;

public class PlayerQuests : MonoBehaviour
{
    public static PlayerQuests playerQuests;
    public QuestSystem quest;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI rewardText;

    public GameObject completionUI;
    public TextMeshProUGUI completionText;  
    
    public GameObject questGiverUI;
    public TextMeshProUGUI questGiverTitel;
    public TextMeshProUGUI questGiverDescr;

    public GroundItem[] books;

    private void Awake()
    {
        playerQuests = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Quest1")
        {
            SetQuestGiverUI("YOU", "What just happend? What was this dream? This dude wants me to kill the titans? And he talked about magic? I should ask my friend the librarian about this. " +
                "<br> <br> <br> " + "<align=center>W A S D <br> to move");
            SetQuest(other.gameObject);
        }
        if (other.name == "Quest2")
        {
            if (quest.title != "Find information.") return;
            quest.Complete();
            completionText.text = "Found the librarian";
            completionUI.SetActive(true);
            StartCoroutine(closeCompletionUI());
            SetQuestGiverUI("Librarian", "Hey my friend. You want to know more about magic? Here you can have my spellbooks. You can use them to create magic. You will find the items in your inventory (I). <br> Drag a book in the book slot to equip. With right mouse button you can cast magic.");
            p_Inventory.CollectItem(books[0]);
            p_Inventory.CollectItem(books[1]);
            p_Inventory.CollectItem(books[2]);
            SetQuest(other.gameObject);
        }
        if (other.name == "Quest3")
        {
            if (quest.title != "Speak to the priest.") return;
            quest.Complete();
            completionText.text = "Found the priest";
            completionUI.SetActive(true);
            StartCoroutine(closeCompletionUI());
            SetQuestGiverUI("Priest", "Hello my son. Did you pray already today. We need to please the gods so they help us with the titans.  <br>May the gods be with you!");
            SetQuest(other.gameObject);
        }
        if (other.name == "Quest4")
        {
            if (quest.title != "Pray to the gods.") return;
            quest.Complete();
            completionText.text = "Prayed to the gods.";
            completionUI.SetActive(true);
            StartCoroutine(closeCompletionUI());
            SetQuestGiverUI("The Runestone", "At the runestone you can pray to the gods. If you die or leave the game you will be reborn at the last runestone you prayed. " +
                "If you have skillpoints, you can use them to upgrade your skills.");
            SetQuest(other.gameObject);
        }
        if (other.name == "Quest5")
        {
            if (quest.title != "Speak to the priest again.") return;
            quest.Complete();
            completionText.text = "Spoken to the priest.";
            completionUI.SetActive(true);
            StartCoroutine(closeCompletionUI());
            SetQuestGiverUI("Priest", "You want to.. WHAT? You want to kill the three titans? You are just a ordinary human. Haha.. good luck. Maybe the smith will give you a sword. " +
                "<br>May the gods be with you!");
            SetQuest(other.gameObject);
        }
        if (other.name == "Quest6")
        {
            if (quest.title != "Get your sword.") return;
            quest.Complete();
            completionText.text = "Found the smith.";
            completionUI.SetActive(true);
            StartCoroutine(closeCompletionUI());
            SetQuestGiverUI("Smith", "You need a sword? I have a old rusty one. It is in front of my house. You can take it.");
            SetQuest(other.gameObject);
        }
        if (other.name == "Quest7")
        {
            if (quest.title != "Pick up the sword.") return;
            quest.Complete();
            completionText.text = "Found a sword.";
            completionUI.SetActive(true);
            StartCoroutine(closeCompletionUI());
            SetQuestGiverUI("The Equipment", "You can pick up bags by pressing E. You will find the items in your inventory." +
                "<br> By pressing left mouse button you can attack.");
            SetQuest(other.gameObject);
        }
        if (other.name == "Quest8")
        {
            if (quest.title != "Visit the doctor.") return;
            quest.Complete();
            completionText.text = "Found the doctor.";
            completionUI.SetActive(true);
            StartCoroutine(closeCompletionUI());
            SetQuestGiverUI("Doctor", "Hey traveler the priest told me you want to kill the titans. Take these, they will help you. <br> <br>" +
                "You can use potions by pressing G. They will heal you over time. <br> You can find new potions around the world and they reset if you pray at a runestone.");
            SetQuest(other.gameObject);
            combatSystem.maxpotions = 3;
            combatSystem.refillPotions();
        }
    }

    private void SetQuestGiverUI(String title, String descr) {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        questGiverUI.SetActive(true);
        questGiverTitel.text = title;
        questGiverDescr.text = descr;
    }

    public void CloseQuestGiverUI() {
        Cursor.lockState = CursorLockMode.Locked;
        questGiverUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SetQuest(GameObject other) {
        QuestGiver currentQuest = other.GetComponent<QuestGiver>();
        currentQuest.AcceptQuest();
        Destroy(other.gameObject);
        playerQuests.titleText.text = currentQuest.quest.title;
        playerQuests.descText.text = currentQuest.quest.descrption;
        playerQuests.rewardText.text = currentQuest.quest.expReward.ToString();
    }

    private IEnumerator closeCompletionUI() {
        yield return new WaitForSeconds(5);
        completionUI.SetActive(false);
    }
}
