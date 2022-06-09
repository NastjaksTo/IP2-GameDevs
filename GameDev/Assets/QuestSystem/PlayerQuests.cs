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
    public bool isQuestGiver;
    public QuestSystem quest;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI rewardText;
    public GameObject npcTalkUI;
    public GameObject questCompleteUI;
    public TextMeshProUGUI completionText;

    private void Awake()
    {
        playerQuests = this;
    }

    private IEnumerator DisableQuestCompletionUI()
    {
        yield return new WaitForSeconds(3f);
        questCompleteUI.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("QuestGiver"))
        {
            isQuestGiver = true;
            npcTalkUI.SetActive(true);
        }
        
        if (other.CompareTag("PleasureDoc"))
        {
            quest.goal.ObjectFound();
            if (quest.goal.IsReached())
            {
                completionText.text = quest.title + " COMPLETED!";
                playerskillsystem.playerlevel.exp += quest.expReward;
                quest.Complete();
                questCompleteUI.SetActive(true);
                StartCoroutine(DisableQuestCompletionUI());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("QuestGiver"))
        {
            isQuestGiver = false;
            questgiver.CloseQuestWindow();
            Cursor.lockState = CursorLockMode.Locked;
            npcTalkUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isQuestGiver)
        {
            questgiver.OpenQuestWindow();
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
