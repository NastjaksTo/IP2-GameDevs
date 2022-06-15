using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public static QuestGiver questgiver;
    public QuestSystem quest;
    public PlayerQuests player;
    public GameObject questWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI rewardText;

    private void Awake()
    {
        questgiver = this;
    }

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;
        descText.text = quest.descrption;
        rewardText.text = quest.expReward.ToString();
    }

    public void CloseQuestWindow()
    {
        questWindow.SetActive(false);
        titleText.text = "";
        descText.text = "";
        rewardText.text = "";
    }

    public void AcceptQuest()
    {
        questWindow.SetActive(false);
        quest.isActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        player.quest = quest;
        player.titleText.text = titleText.text;
        player.descText.text = descText.text;
        player.rewardText.text = rewardText.text;
    }
}
