using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static PlayerInventory;
using static CombatSystem;
using static UiScreenManager;
using static SaveData;

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
                SetQuestGiverUI("You", "Our world has been infested with these titans for so many years and Raya seems to be responsible for that.\n \nThis voice tells me that our world can be liberated. And it said something about magic? I should ask the librarian if the can tell me more about it. \n\nPerhaps this horror will soon finally come to an end.\n\nMove - W A S D  \nJump - SPACE \nSprint - SHIFT  \nDodge - C");

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
                SetQuestGiverUI("The Librarian", "Hello. You want to know more about magic? \n\nSomeone tried to research the powers of the three titans and wrote these books. So far, no one has been able to use these abilities. I don't know if it really works but you can have the books, maybe they will help you \n\nWith the help of your divine powers you are able to cast magic. Equip a book in your inventory to use the powers. \n\nOpen Inventory - I  \nCast Spell - right mouse button");


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
                SetQuestGiverUI("The Priest", "Hello my son.\n\nDid you pray already today? \n\nSince we stopped praying to the gods, we have been plagued by these titans. \n\nWe have to pray to them again, only they can give us the strength to get through this.  \n\nMay the gods be with you!");
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
                SetQuestGiverUI("The Runestone", "At the runestone you can pray to the gods. \n\nIf you die or leave the game you will be reborn at the last runestone you prayed. \n\nYour powers will be replenished here.\n\nIf you have skillpoints, you can use them to upgrade your skills. \n\nInteract - E");
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
                SetQuestGiverUI("The Priest", "You want to.. WHAT?\n\nYou want to kill the three titans? You are just a ordinary human. Haha.. \n\nThose who tried burned in the lava, froze from the cold, or fell into the ravines before they even saw a titan. \n\nGood luck.\nMaybe the smith will give you a sword. \n\nMay the gods be with you!");
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
                SetQuestGiverUI("The Smith", "Do you know the legend of the magic mole?\n\nOh.. you need a sword?\nI have a old rusty one. It is in front of my house. You can take it. \n\nUnfortunately, I don't have any armor for you, but you'll find one.\n\nAlways take a good look around.");
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
                SetQuestGiverUI("The Equipment", "You can pick up bags by pressing E. \nYou will find the items in your inventory.\n\nYou can equip them by dropping it in your equipment. \nThe items will give you different buffs.\n\nAttack - left mouse button");
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
                SetQuestGiverUI("The Doctor", "Hello Dion.\n\nThe priest told me you want to kill the titans. Take these health potion, they will help you. \n\nYou can find new potion bottles around the world and they fill if you pray at a runestone. \n\nUse potion - G ");
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
                SetQuestGiverUI("The Priest", "Raya? She is here?\n\nYou have to defeat here or else we our beautiful world will be doomed! \n\nPeople talked about a loud noise coming from the entrance to the ravine.\nYou should check this out first.");
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
        uiScreenManager.ClosePlayerStatsUi();
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
            yield return new WaitForSecondsRealtime(0.035f);
        }
        closeDialogBtn.SetActive(true);
        writingSound.Stop();
        //newQuestAltertOpen();
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
        SetQuestGiverUI("Raya", "You fool! \n\nYou thought you could save the world by defeating my titans? \nYou will never be a hero. \n\nAnd now... feel my wrath.");
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
        uiScreenManager.ShowPlayerStatsUi();
        dialogueIsOpen = false;
        Time.timeScale = 1f;
        newQuestAltertOpen();
        saveData.SaveGame();
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
        yield return new WaitForSecondsRealtime(6);
        completionUI.SetActive(false);
    }

    /// <summary>
    /// Closes the new quest alert after a set amount of time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator closeNewQuestAlert()
    {
        yield return new WaitForSecondsRealtime(6);
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
