using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static QuestGiver;
using static PlayerSkillsystem;

public class PlayerQuests : MonoBehaviour
{
    public static PlayerQuests playerQuests;
    public QuestSystem quest;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI rewardText;

    private void Awake()
    {
        playerQuests = this;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Quest1")
        {
            Debug.Log(other.gameObject.ToString());
            QuestGiver currentQuest = other.GetComponent<QuestGiver>();
            currentQuest.AcceptQuest();
            Destroy(other.gameObject);
            playerQuests.titleText.text = currentQuest.quest.title;
            playerQuests.descText.text = currentQuest.quest.descrption;
            playerQuests.rewardText.text = currentQuest.quest.expReward.ToString();
        }
        if (other.name == "Quest2")
        {
            if (quest.title != "Find information.") return;
            quest.Complete();
            QuestGiver currentQuest = other.GetComponent<QuestGiver>();
            currentQuest.AcceptQuest();
            Destroy(other.gameObject);
            playerQuests.titleText.text = currentQuest.quest.title;
            playerQuests.descText.text = currentQuest.quest.descrption;
            playerQuests.rewardText.text = currentQuest.quest.expReward.ToString();
        }
        if (other.name == "Quest3")
        {
            if (quest.title != "Speak to the priest.") return;
            quest.Complete();
            Debug.Log(other.gameObject.ToString());
            QuestGiver currentQuest = other.GetComponent<QuestGiver>();
            currentQuest.AcceptQuest();
            Destroy(other.gameObject);
            playerQuests.titleText.text = currentQuest.quest.title;
            playerQuests.descText.text = currentQuest.quest.descrption;
            playerQuests.rewardText.text = currentQuest.quest.expReward.ToString();
        }
        if (other.name == "Quest4")
        {
            if (quest.title != "Pray to the gods.") return;
            quest.Complete();
            Debug.Log(other.gameObject.ToString());
            QuestGiver currentQuest = other.GetComponent<QuestGiver>();
            currentQuest.AcceptQuest();
            Destroy(other.gameObject);
            playerQuests.titleText.text = currentQuest.quest.title;
            playerQuests.descText.text = currentQuest.quest.descrption;
            playerQuests.rewardText.text = currentQuest.quest.expReward.ToString();
        }
        if (other.name == "Quest5")
        {
            if (quest.title != "Speak to the priest again.") return;
            quest.Complete();
            Debug.Log(other.gameObject.ToString());
            QuestGiver currentQuest = other.GetComponent<QuestGiver>();
            currentQuest.AcceptQuest();
            Destroy(other.gameObject);
            playerQuests.titleText.text = currentQuest.quest.title;
            playerQuests.descText.text = currentQuest.quest.descrption;
            playerQuests.rewardText.text = currentQuest.quest.expReward.ToString();
        }
        if (other.name == "Quest6")
        {
            if (quest.title != "Get your sword.") return;
            quest.Complete();
            Debug.Log(other.gameObject.ToString());
            QuestGiver currentQuest = other.GetComponent<QuestGiver>();
            currentQuest.AcceptQuest();
            Destroy(other.gameObject);
            playerQuests.titleText.text = currentQuest.quest.title;
            playerQuests.descText.text = currentQuest.quest.descrption;
            playerQuests.rewardText.text = currentQuest.quest.expReward.ToString();
        }
        if (other.name == "Quest7")
        {
            if (quest.title != "Pick up the sword.") return;
            quest.Complete();
            Debug.Log(other.gameObject.ToString());
            QuestGiver currentQuest = other.GetComponent<QuestGiver>();
            currentQuest.AcceptQuest();
            Destroy(other.gameObject);
            playerQuests.titleText.text = currentQuest.quest.title;
            playerQuests.descText.text = currentQuest.quest.descrption;
            playerQuests.rewardText.text = currentQuest.quest.expReward.ToString();
        }
        if (other.name == "Quest8")
        {
            if (quest.title != "Visit the doctor.") return;
            quest.Complete();
            Debug.Log(other.gameObject.ToString());
            QuestGiver currentQuest = other.GetComponent<QuestGiver>();
            currentQuest.AcceptQuest();
            Destroy(other.gameObject);
            playerQuests.titleText.text = currentQuest.quest.title;
            playerQuests.descText.text = currentQuest.quest.descrption;
            playerQuests.rewardText.text = currentQuest.quest.expReward.ToString();
        }
    }
}
