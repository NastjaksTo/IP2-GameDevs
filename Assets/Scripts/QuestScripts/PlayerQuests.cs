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

    public GameObject newQuestAlert;
    public TextMeshProUGUI newQuestAlertText;

    public GameObject completionUI;
    public TextMeshProUGUI completionText;  
    
    public GameObject questGiverUI;
    public TextMeshProUGUI questGiverTitel;
    public TextMeshProUGUI questGiverDescr;
    
    public GameObject lootbags;
    public ItemObject[] books;
    public int currentQuestID = 1;

    public AudioClip questDone;
    public AudioClip newQuest;
    public AudioSource writingSound;

    public GameObject closeDialogBtn;
    public GameObject rayaEntrance;
    
    public bool dialogueIsOpen;

    private void Awake()
    {
        playerQuests = this;
        currentQuestID = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("QuestGiver"))
        {
            QuestGiver currentQuestGiver = other.gameObject.GetComponent<QuestGiver>();
            if (currentQuestID == currentQuestGiver.quest.questID && other.name == "Quest1")
            {
                SetQuestGiverUI("You", "What just happend? What was this dream? This dude wants me to kill the titans? And he talked about magic? I should ask my friend the librarian about this.  \n  \n" + "Move - W A S D  \nJump - SPACE \nSprint - SHIFT  \nDodge - C");

                SetQuest(other.gameObject);
                currentQuestID++;
            }
            if (currentQuestID == currentQuestGiver.quest.questID && other.name == "Quest2")
            {
                quest.Complete();
                AudioSource.PlayClipAtPoint(questDone, transform.position, 1);
                completionText.text = "Quest complete: Find the librarian";
                completionUI.SetActive(true);
                StartCoroutine(closeCompletionUI());
                SetQuestGiverUI("Librarian", "Hey my friend. You want to know more about magic? Here you can have my spellbooks. You can use them to create magic. You will find the items in your inventory (I).  \n  \nDrag a book in the book slot to equip. With right mouse button you can cast magic.");
                
                p_Inventory.CollectItem(books[0]);
                p_Inventory.CollectItem(books[1]);
                p_Inventory.CollectItem(books[2]);

                SetQuest(other.gameObject);
                currentQuestID++;
            }
            if (currentQuestID == currentQuestGiver.quest.questID && other.name == "Quest3")
            {
                quest.Complete();
                AudioSource.PlayClipAtPoint(questDone, transform.position, 1);
                completionText.text = "Quest complete: Find the priest";
                completionUI.SetActive(true);
                StartCoroutine(closeCompletionUI());
                SetQuestGiverUI("Priest", "Hello my son. Did you pray already today? <br>We need to please the gods so they help us with the titans.  <br><br>May the gods be with you!");
                SetQuest(other.gameObject);
                currentQuestID++;
            }
            if (currentQuestID == currentQuestGiver.quest.questID && other.name == "Quest4")
            {
                quest.Complete();
                AudioSource.PlayClipAtPoint(questDone, transform.position, 1);
                completionText.text = "Quest complete: Prayed to the gods.";
                completionUI.SetActive(true);
                StartCoroutine(closeCompletionUI());
                SetQuestGiverUI("The Runestone", "At the runestone you can pray to the gods. If you die or leave the game you will be reborn at the last runestone you prayed. \n  \nIf you have skillpoints, you can use them to upgrade your skills.");
                SetQuest(other.gameObject);
                currentQuestID++;
            }
            if (currentQuestID == currentQuestGiver.quest.questID && other.name == "Quest5")
            {
                quest.Complete();
                AudioSource.PlayClipAtPoint(questDone, transform.position, 1);
                completionText.text = "Quest complete: Spoken to the priest.";
                completionUI.SetActive(true);
                StartCoroutine(closeCompletionUI());
                SetQuestGiverUI("Priest", "You want to.. WHAT? You want to kill the three titans? You are just a ordinary human. Haha.. good luck. Maybe the smith will give you a sword. " +
                                          " \n  \nMay the gods be with you!");
                SetQuest(other.gameObject);
                currentQuestID++;
            }
            if (currentQuestID == currentQuestGiver.quest.questID && other.name == "Quest6")
            {
                quest.Complete();
                AudioSource.PlayClipAtPoint(questDone, transform.position, 1);
                completionText.text = "Quest complete: Find the smith.";
                completionUI.SetActive(true);
                StartCoroutine(closeCompletionUI());
                SetQuestGiverUI("Smith", "You need a sword? I have a old rusty one. It is in front of my house. You can take it. Unfortunately, I don't have any armour for you, but you'll find one.");
                lootbags.SetActive(true);
                SetQuest(other.gameObject);
                currentQuestID++;
            }
            if (currentQuestID == currentQuestGiver.quest.questID && other.name == "Quest7")
            {
                quest.Complete();
                AudioSource.PlayClipAtPoint(questDone, transform.position, 1);
                completionText.text = "Quest complete: Find a sword.";
                completionUI.SetActive(true);
                StartCoroutine(closeCompletionUI());
                SetQuestGiverUI("The Equipment", "You can pick up bags by pressing E. You will find the items in your inventory." +
                                                 " \n  \nBy pressing left mouse button you can attack.");
                SetQuest(other.gameObject);
                currentQuestID++;
            }
            if (currentQuestID == currentQuestGiver.quest.questID && other.name == "Quest8")
            {
                quest.Complete();
                AudioSource.PlayClipAtPoint(questDone, transform.position, 1);
                completionText.text = "Quest complete: Find the doctor.";
                completionUI.SetActive(true);
                StartCoroutine(closeCompletionUI());
                SetQuestGiverUI("Doctor", "Hey traveler the priest told me you want to kill the titans. Take these, they will help you. \n  \nYou can use potions by pressing G. They will heal you over time.  \nYou can find new potions around the world and they reset if you pray at a runestone.");
                SetQuest(other.gameObject);
                combatSystem.maxpotions = 3;
                combatSystem.refillPotions();
                currentQuestID++;
            }
            if (currentQuestID == currentQuestGiver.quest.questID && other.name == "Quest9")
            {
                quest.Complete();
                AudioSource.PlayClipAtPoint(questDone, transform.position, 1);
                completionText.text = "Quest complete: Go back to the priest.";
                completionUI.SetActive(true);
                StartCoroutine(closeCompletionUI());
                SetQuestGiverUI("Priest", "brrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");
                SetQuest(other.gameObject);
                currentQuestID++;
                rayaEntrance.SetActive(false);
            }
        }
    }

    private void SetQuestGiverUI(String title, String descr) {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        questGiverUI.SetActive(true);
        dialogueIsOpen = true;
        questGiverTitel.text = "";
        questGiverDescr.text = "";
        questGiverTitel.text = title;
        StartCoroutine(TypeLine(descr));
    }

    IEnumerator TypeLine(string text) {
        writingSound.Play();
        foreach (char c in text.ToCharArray()) {
            questGiverDescr.text += c;
            yield return new WaitForSecondsRealtime(0.025f);
        }
        closeDialogBtn.SetActive(true);
        writingSound.Stop();
        newQuestAltertOpen();
    }

    public void TitanQuest()
    {
        quest.Complete();
        AudioSource.PlayClipAtPoint(questDone, transform.position, 1);
        completionText.text = "Quest complete: Find and defeat the titans.";
        completionUI.SetActive(true);
        StartCoroutine(closeCompletionUI());
        SetQuestGiverUI("YOU", "grrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");
        playerQuests.titleText.text = "Go back to the priest.";
        playerQuests.descText.text = "BLABLABLABLA";
        playerQuests.rewardText.text = "1000";
        currentQuestID++;
    }

    public void CloseQuestGiverUI() {
        Cursor.lockState = CursorLockMode.Locked;
        closeDialogBtn.SetActive(false);
        questGiverUI.SetActive(false);
        dialogueIsOpen = false;
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

    private void newQuestAltertOpen(){
        AudioSource.PlayClipAtPoint(newQuest, transform.position, 1);
        newQuestAlertText.text = "New Quest: " + playerQuests.titleText.text + " ( J )";
        newQuestAlert.SetActive(true);
        StartCoroutine(closeNewQuestAlert());
    }

    private IEnumerator closeCompletionUI() {
        yield return new WaitForSecondsRealtime(3);
        completionUI.SetActive(false);
    }

    private IEnumerator closeNewQuestAlert()
    {
        yield return new WaitForSecondsRealtime(3);
        newQuestAlert.SetActive(false);
    }
}
