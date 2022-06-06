using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the "call to actions" for the player.
/// </summary>
public class PlayeInteractionAlert : MonoBehaviour {

    public static bool ClollectAlertOpen = false;
    public GameObject ClollectAlertUI;                       //referenz set in editor
    
    /// <summary>
    /// Close the collect alert.
    /// </summary>
    public void CloseCollectAlertUi() {
        ClollectAlertUI.SetActive(false);
        ClollectAlertOpen = false;
    }

    /// <summary>
    /// Open the collect alert.
    /// </summary>
    public void OpenCollectAlertUi() {
        ClollectAlertUI.SetActive(true);
        ClollectAlertOpen = true;
    }
    
}
