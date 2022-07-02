using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerDisplay;
using static PlayerQuests;
using static PlayerSkillsystem;
using static BossArena;
using static SaveData;


/// <summary>
/// Manage the in game Uis. Open and close them and defines the functions of the buttons in the menus.
/// </summary>
public class UiScreenManager : MonoBehaviour {

    public static UiScreenManager uiScreenManager;
    public GameObject playerStatsUI;                        //reference set in editor

    private static bool _inventoryUiOpen = false;
    public GameObject inventoryUI;                          //reference set in editor

    public static bool _skillUiOpen = false;
    public GameObject skillUi;                              //reference set in editor

    private static bool _questUiOpen = false;
    public GameObject questUi;                              //reference set in editor

    public static bool _deathUiOpen = false;
    public GameObject deathUi;                              //reference set in editor
    public AudioClip deathSound;

    public GameObject pauseMenuUi;                          //reference set in editor

    private static bool _pauseMenuContainerUiOpen = false;
    public GameObject pauseMenuContainerUi;                 //reference set in editor


    public GameObject optionsMenuUI;                        //reference set in editor
    public GameObject controlMenuUI;                        //reference set in editor

    public static bool _isOneIngameUiOpen = false;
    private static bool _isOneInMenueUiOpen = false;
    

    private void Awake() {
        ClosePauseContainerUi();
        uiScreenManager = this;
    }

    /// <summary>
    /// Opens the playerstats UI.
    /// </summary>
    private void ShowPlayerStatsUi() {
        playerStatsUI.SetActive(true);
    }

    /// <summary>
    /// Closes the playerstats UI.
    /// </summary>
    private void ClosePlayerStatsUi() {
        playerStatsUI.SetActive(false);
    }


    /// <summary>
    /// Opens the inventory UI. Makes the mouse pointer visible and freezes the game time. 
    /// </summary>
    private void OpenInventoryUi() {
        ClosePlayerStatsUi();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        inventoryUI.SetActive(true);
        _inventoryUiOpen = true;
        _isOneIngameUiOpen = true;
    }

    /// <summary>
    /// Closes the inventory UI. Locks the Cursor and let the game time continue.
    /// </summary>
    private void CloseInventoryUi() {
        ShowPlayerStatsUi();
        Cursor.lockState = CursorLockMode.Locked;
        inventoryUI.SetActive(false);
        _inventoryUiOpen = false;
        _isOneIngameUiOpen = false;
        Time.timeScale = 1f;
    }


    private void OpenQuestUi() {
        ClosePlayerStatsUi();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        questUi.SetActive(true);
        _questUiOpen = true;
        _isOneIngameUiOpen = true;
    }


    private void CloseQuestUi() {
        ShowPlayerStatsUi();
        Cursor.lockState = CursorLockMode.Locked;
        inventoryUI.SetActive(false);
        questUi.SetActive(false);
        _questUiOpen = false;
        Time.timeScale = 1f;
        _isOneIngameUiOpen = false;
    }


    /// <summary>
    /// Opens the skill UI. Makes the mouse pointer visible and freezes the game time. 
    /// </summary>
    public void OpenSkillUi() {
        ClosePlayerStatsUi();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        skillUi.SetActive(true);
        _skillUiOpen = true;
        _isOneIngameUiOpen = true;
    }

    /// <summary>
    /// Closes the skill UI. Locks the Cursor and let the game time continue.
    /// </summary>
    public void CloseSkillUi() {
        ShowPlayerStatsUi();
        Cursor.lockState = CursorLockMode.Locked;
        skillUi.SetActive(false);
        _skillUiOpen = false;
        _isOneIngameUiOpen = false;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Open the death UI. Makes the mouse pointer visible and freezes the game time. 
    /// </summary>
    public void OpenDeathUi() {
        //alert.CloseCollectAlertUi();
        bossarenaScript.CloseAllArenas();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        deathUi.SetActive(true);
        _deathUiOpen = true;
        AudioSource.PlayClipAtPoint(deathSound, transform.position, 1);
    }

    /// <summary>
    /// Closes the death UI. Locks the Cursor and let the game time continue.
    /// </summary>
    public void CloseDeathUi() {
        Cursor.lockState = CursorLockMode.Locked;
        deathUi.SetActive(false);
        _deathUiOpen = false;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Opens the in pause menu UI. Makes the mouse pointer visible and freezes the game time. 
    /// </summary>
    private void OpenPauseContainerMenuUi() {
        ClosePlayerStatsUi();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        OpenMenuUi();
        pauseMenuContainerUi.SetActive(true);
        _pauseMenuContainerUiOpen = true;
        _isOneIngameUiOpen = true;
    }

    /// <summary>
    /// Closes the in pause menu UI. Locks the Cursor and let the game time continue if no other ui ist open.
    /// </summary>
    public void ClosePauseContainerUi() {

        CloseMenuUi();
        CloseControlUi();
        CloseOptionsUi();

        ShowPlayerStatsUi();
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuContainerUi.SetActive(false);
        _pauseMenuContainerUiOpen = false;
        Time.timeScale = 1f;
        _isOneIngameUiOpen = false;
    }

    //------------- IN menu UI -------------


    public void OpenMenuUi() {
        pauseMenuUi.SetActive(true);
    }

    /// <summary>
    /// Closes the options UI
    /// </summary>
    public void CloseMenuUi() {
        pauseMenuUi.SetActive(false);
    }

    /// <summary>
    /// Opens the options UI
    /// </summary>
    public void OpenOptionsUi() {
        CloseMenuUi();
        optionsMenuUI.SetActive(true);
    }

    /// <summary>
    /// Closes the options UI
    /// </summary>
    public void CloseOptionsUi() {
        optionsMenuUI.SetActive(false);
        OpenMenuUi();
    }

    /// <summary>
    /// Opens the control UI
    /// </summary>
    public void OpenControlUi() {
        CloseMenuUi();
        controlMenuUI.SetActive(true);
    }

    /// <summary>
    /// Closes the control UI
    /// </summary>
    public void CloseControlUi() {
        controlMenuUI.SetActive(false);
        OpenMenuUi();
    }


    //------------- UI Button functions -------------

    /// <summary>
    /// Loads the main menu of the game -> Scene change
    /// </summary>
    public void LoadMenu() {
        CloseDeathUi();
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Loads the last saved game state 
    /// </summary>
    public void LoadLastGameState() { //TODO: LOAD LAST SAVED GAME STATE
        saveData.Loadgame();
    }


    /// <summary>
    /// Checks each frame the input for the keys I and Esc
    /// On I and if the DeathUi is closed: close the inventoryUI when its open, or open it if its closed and the menu is not opened.
    /// On R and if the DeathUi is closed: close the skillUi when its open, or open it if its closed and the menu is not opened.
    /// On Esc and if the DeathUi is closed: close the menuUI when its open, or open it if its closed.
    /// </summary>
    public void Update() {

        if (Input.GetKeyDown(KeyCode.I) && !_deathUiOpen && !playerQuests.dialogueIsOpen) {
            if (_inventoryUiOpen) {
                CloseInventoryUi();
                playerDisplay.UpdateSpellUI();
                playerskillsystem.updateLevelUI();
            } else if (!_pauseMenuContainerUiOpen && !_isOneIngameUiOpen) {
                OpenInventoryUi();
                playerDisplay.UpdateSpellUI();
                playerskillsystem.updateLevelUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.J) && !_deathUiOpen && !playerQuests.dialogueIsOpen) {
            if (_questUiOpen) {
                CloseQuestUi();
            } else if (!_pauseMenuContainerUiOpen && !_isOneIngameUiOpen) {
                OpenQuestUi();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !_deathUiOpen && !playerQuests.dialogueIsOpen) {
            if (_pauseMenuContainerUiOpen) {
                ClosePauseContainerUi();
            } else {

                if (!_isOneIngameUiOpen && !_isOneInMenueUiOpen) {
                    OpenPauseContainerMenuUi();
                }

                if (_inventoryUiOpen) {
                    CloseInventoryUi();
                }

                if (_skillUiOpen) {
                    CloseSkillUi();
                    saveData.SaveGame();
                    SceneManager.LoadSceneAsync("EnemyScene", LoadSceneMode.Additive);
                }

                if (_questUiOpen) {
                    CloseQuestUi();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab) && (_inventoryUiOpen || _questUiOpen)) {

            if (_inventoryUiOpen) {
                CloseInventoryUi();
                OpenQuestUi();
            } else if (_questUiOpen) {
                CloseQuestUi();
                OpenInventoryUi();
            }
        }
    }
}
