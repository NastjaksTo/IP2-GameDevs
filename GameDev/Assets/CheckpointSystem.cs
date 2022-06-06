using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    public GameObject checkpointUI;
    public static bool checkpointactive;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            checkpointUI.SetActive(true);
            checkpointactive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            checkpointUI.SetActive(false);
            checkpointactive = false;
        }
    }
}
