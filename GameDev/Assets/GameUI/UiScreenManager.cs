using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage the ingame Uis. Open and close them and defines the functions of the buttons in the menus.
/// </summary>
public class UiScreenManager : MonoBehaviour {

    public static bool InventoryUiOpen = false;
    public GameObject inventoryUI;                      //referenz set in editor

    public static bool SkillUiOpen = false;
    public GameObject skillUi;                          //referenz set in editor

    public static bool MenuUiOpen = false;
    public GameObject MenuUi;                           //referenz set in editor

    public static bool DeathUiOpen = false;
    public GameObject DeathUi;                          //referenz set in editor

    public static bool isOneUiOpen = false;


    /// <summary>
    /// Opens the inventory UI. Makes the mouse pointer visible and freezes the game time. 
    /// </summary>
    private void OpenInventoryUi() {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        inventoryUI.SetActive(true);
        InventoryUiOpen = true;
        isOneUiOpen = true;
    }

    /// <summary>
    /// Close the inventory UI. Locked the Cursor and lets the game time continue.
    /// </summary>
    private void CloseInventoryUi() {
        Cursor.lockState = CursorLockMode.Locked;
        inventoryUI.SetActive(false);
        InventoryUiOpen = false;
        Time.timeScale = 1f;
        isOneUiOpen = false;
    }

    /// <summary>
    /// Opens the skill UI. Makes the mouse pointer visible and freezes the game time. 
    /// </summary>
    private void OpenSkillyUi() {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        skillUi.SetActive(true);
        SkillUiOpen = true;
        isOneUiOpen = true;
    }

    /// <summary>
    /// Close the skill UI. Locked the Cursor and lets the game time continue.
    /// </summary>
    private void CloseSkillUi() {
        Cursor.lockState = CursorLockMode.Locked;
        skillUi.SetActive(false);
        SkillUiOpen = false;
        Time.timeScale = 1f;
        isOneUiOpen = false;
    }

    /// <summary>
    /// Opens the ingame menu UI. Makes the mouse pointer visible and freezes the game time. 
    /// </summary>
    private void OpenMenueUi() {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        MenuUi.SetActive(true);
        MenuUiOpen = true;
    }

    /// <summary>
    /// Close the ingame menu UI. Locked the Cursor and lets the game time continue if no other ui ist open.
    /// </summary>
    public void CloseMenueUi() {
        Cursor.lockState = CursorLockMode.Locked;
        MenuUi.SetActive(false);
        MenuUiOpen = false;
        if (!isOneUiOpen) {
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// Open the death UI. Makes the mouse pointer visible and freezes the game time. 
    /// </summary>
    public void OpenDeathUi() {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        DeathUi.SetActive(true);
        DeathUiOpen = true;
    }

    /// <summary>
    /// Close the death UI. Locked the Cursor and lets the game time continue.
    /// </summary>
    public void CloseDeathUi() {
        Cursor.lockState = CursorLockMode.Locked;
        DeathUi.SetActive(false);
        DeathUiOpen = false;
        Time.timeScale = 1f;
    }


    //------------- UI Button functions -------------

    /// <summary>
    /// Loade the main manu of the game -> Sene change
    /// </summary>
    public void LoadMenu() {
        Debug.Log("TODO: LOAD THE GAME MENU");
        //SceneManager.LoadScene("Menu"); //läd Menue Szene 
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Load the last saved game state 
    /// </summary>
    public void LoadLastGameState() { //TODO: LOAD LAST SAVED GAME STATE
        Debug.Log("TODO: LOAD LAST SAVED GAME STATE");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Close the application
    /// </summary>
    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quit");
    }


    /// <summary>
    /// Checks each frame the input for the keys I and Esc
    /// On I and if the DeathUi is closed: close the inventoryUI when its open, or open it if its closed and the menu is not opend.
    /// On R and if the DeathUi is closed: close the skillUi when its open, or open it if its closed and the menu is not opend.
    /// On Esc and if the DeathUi is closed: close the menuUI when its open, or open it if its closed.
    /// </summary>
    void Update() {
        if (Input.GetKeyDown(KeyCode.I) && !DeathUiOpen) {
            if (InventoryUiOpen ) {
                CloseInventoryUi();
            } else if (!MenuUiOpen && !isOneUiOpen) {
                OpenInventoryUi();
            }
        }

        if (Input.GetKeyDown(KeyCode.K) && !DeathUiOpen) {
            if (SkillUiOpen) {
                CloseSkillUi();
            } else if (!MenuUiOpen && !isOneUiOpen) {
                OpenSkillyUi();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !DeathUiOpen) {
            if (MenuUiOpen) {
                CloseMenueUi();
            } else {
                OpenMenueUi();
            }
        }

    }
}
