using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static PlayerInventory;
using static CombatSystem;
using static UiScreenManager;

public class PlayerQuests : MonoBehaviour
{
    public static PlayerQuests playerQuests;            // Creates a public static reference to this script.
    public QuestSystem quest;                           // Reference to QuestSystem script.
    public TextMeshProUGUI titleText;                   // Reference to the TitleText UI.
    public TextMeshProUGUI descText;                    // Reference to the DescriptionText UI.
    public TextMeshProUGUI rewardText;                  // Reference to the RewardText UI.

    public GameObject newQuestAlert;                    // Reference to the QuestAlert UI.
    public TextMeshProUGUI newQuestAlertText;           // Reference to the QuestAlertText UI.

    public GameObject completionUI;                     // Reference to the Completion UI.
    public TextMeshProUGUI completionText;              // Reference to the CompletionText UI.
    
    public GameObject questGiverUI;                     // Reference to the current QuestGiver UI.
    public TextMeshProUGUI questGiverTitel;             // Reference to the QuestGiverTitle UI.
    public TextMeshProUGUI questGiverDescr;             // Reference to the QuestGiverDescription UI.
    
    public GameObject lootbags;                         // Reference to the Lootbags.
    public ItemObject[] books;                          // Array of itemobjects to save all spellbooks.
    public int currentQuestID = 1;                      // Integer gets initialized with the starting QuestID.

    public AudioClip questDone;                         // AudioClip when the quest is done.
    public AudioClip newQuest;                          // AudioClip when a new quest is accepted.
    public AudioSource writingSound;                    // AudioSource for the writing of the text.

    public GameObject closeDialogBtn;                   // Reference to the CloseButton UI.
    public GameObject rayaEntrance;                     // Reference to Rayas entrance.
    
    public bool dialogueIsOpen;                         // Bool to check whether or not the dialogue UI is open.

    /// <summary>
    /// Assigns values.
    /// </summary>
    private void Awake()
    {
        playerQuests = this;
        currentQuestID = 1;
    }

    /// <summary>
    /// Checks if Player is colliding with a QuestGiver.
    /// Checks the questname and questid.
    /// If thats the case, complete the old quest and assign the new quest.
    /// Sets all UI elements according to the new quest.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("QuestGiver"))
        {
            QuestGiver currentQuestGiver = other.gameObject.GetComponent<QuestGiver>();
            if (currentQuestID == currentQuestGiver.quest.questID && other.name == "Quest1")
            {
                SetQuestGiverUI("You", "What was this dream? This dude wants me to kill the titans? And he talked about magic? I should ask my friend the librarian about this.  \n  \n" + "Move - W A S D  \nJump - SPACE \nSprint - SHIFT  \nDodge - C");

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
                SetQuestGiverUI("Priest", "Hello my son. Did you pray already today? \nWe need to please the gods so they help us with the titans.  \n\nMay the gods be with you!");
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
                SetQuestGiverUI("Smith", "You need a sword? I have a old rusty one. It is in front of my house. You can take it. \n \nUnfortunately, I don't have any armor for you, but you'll find one.");
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
                                                 " \n \nBy pressing left mouse button you can attack.");
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
                combatSystem.maxpotions = 1;
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
                SetQuestGiverUI("Priest", "Raya? She is here? You have to defeat here or else we our beautiful world will be doomed! People talked about a loud noise coming from the entrance to the ravine. You should check this out first.");
                SetQuest(other.gameObject);
                currentQuestID++;
                rayaEntrance.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Sets the Text and UI elements of the QuestGiver gameobject.
    /// </summary>
    /// <param name="title">Gets the title of the quest</param>
    /// <param name="descr">Gets the description of the quest.</param>
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

    /// <summary>
    /// Types the description of the text. To create an immersive effect.
    /// </summary>
    /// <param name="text">Gets the text to write.</param>
    /// <returns></returns>
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

    /// <summary>
    /// If all titans are defeated set the new quest.
    /// </summary>
    public void TitanQuest()
    {
        quest.Complete();
        AudioSource.PlayClipAtPoint(questDone, transform.position, 1);
        completionText.text = "Quest complete: Find and defeat the titans.";
        completionUI.SetActive(true);
        StartCoroutine(closeCompletionUI());
        SetQuestGiverUI("Raya", "You fool! You thought you could save the world by defeating my titans? You will never be a hero. And now... feel my wrath.");
        playerQuests.titleText.text = "Go back to the priest.";
        playerQuests.descText.text = "Ask the priest for informations about Raya.";
        playerQuests.rewardText.text = "1000";
        currentQuestID++;
    }

    /// <summary>
    /// Closes all UI elements and sets the timescale to 1 (unfreeze the game).
    /// </summary>
    public void CloseQuestGiverUI() {
        Cursor.lockState = CursorLockMode.Locked;
        closeDialogBtn.SetActive(false);
        questGiverUI.SetActive(false);
        dialogueIsOpen = false;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Sets the new quest and assigns values.
    /// </summary>
    /// <param name="other"></param>
    public void SetQuest(GameObject other) {
        QuestGiver currentQuest = other.GetComponent<QuestGiver>();
        currentQuest.AcceptQuest();
        Destroy(other.gameObject);
        playerQuests.titleText.text = currentQuest.quest.title;
        playerQuests.descText.text = currentQuest.quest.descrption;
        playerQuests.rewardText.text = currentQuest.quest.expReward.ToString();
    }

    /// <summary>
    /// Creates UI Alert for the new quest.
    /// </summary>
    private void newQuestAltertOpen(){
        AudioSource.PlayClipAtPoint(newQuest, transform.position, 1);
        newQuestAlertText.text = "New Quest: " + playerQuests.titleText.text + " ( J )";
        newQuestAlert.SetActive(true);
        StartCoroutine(closeNewQuestAlert());
    }

    /// <summary>
    /// Closes the completion alert after a set amount of time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator closeCompletionUI() {
        yield return new WaitForSecondsRealtime(5);
        completionUI.SetActive(false);
    }

    /// <summary>
    /// Closes the new quest alert after a set amount of time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator closeNewQuestAlert()
    {
        yield return new WaitForSecondsRealtime(5);
        newQuestAlert.SetActive(false);
    }

    /// <summary>
    /// If Escape is pressed, close all UI elements.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_deathUiOpen && closeDialogBtn.activeSelf)
        {
            CloseQuestGiverUI();
        }
    }
}
