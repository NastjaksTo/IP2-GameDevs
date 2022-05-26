using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage the in game Uis. Open and close them and defines the functions of the buttons in the menus.
/// </summary>
public class UiScreenManager : MonoBehaviour {

    [Header("InventoryUI")]
    public static bool inventoryUiOpen = false;
    public GameObject inventoryUI;                      //reference set in editor
    
    [Header("SkillUI")]
    public static bool skillUiOpen = false;
    public GameObject skillUi;                          //reference set in editor
    
    [Header("PauseMenuUI")]
    public static bool pauseMenuUiOpen = false;
    public GameObject pauseMenuUi;                           //reference set in editor

    [Header("OptionsMenuUI")]
    public static bool optionsMenuUIOpen = false;
    public GameObject optionsMenuUI;                           //reference set in editor

    [Header("ControlMenuUI")]
    public static bool controlMenuUIOpen = false;
    public GameObject controlMenuUI;                           //reference set in editor

    [Header("SoundMenuUI")]
    public static bool soundMenuUIOpen = false;
    public GameObject soundMenuUI;                           //reference set in editor

    [Header("DeathUI")]
    public static bool deathUiOpen = false;
    public GameObject deathUi;                          //reference set in editor

    public static bool isOneUiOpen = false;


    /// <summary>
    /// Opens the inventory UI. Makes the mouse pointer visible and freezes the game time. 
    /// </summary>
    private void OpenInventoryUi() {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        inventoryUI.SetActive(true);
        inventoryUiOpen = true;
        isOneUiOpen = true;
    }

    /// <summary>
    /// Close the inventory UI. Locked the Cursor and lets the game time continue.
    /// </summary>
    private void CloseInventoryUi() {
        Cursor.lockState = CursorLockMode.Locked;
        inventoryUI.SetActive(false);
        inventoryUiOpen = false;
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
        skillUiOpen = true;
        isOneUiOpen = true;
    }

    /// <summary>
    /// Close the skill UI. Locked the Cursor and lets the game time continue.
    /// </summary>
    private void CloseSkillUi() {
        Cursor.lockState = CursorLockMode.Locked;
        skillUi.SetActive(false);
        skillUiOpen = false;
        Time.timeScale = 1f;
        isOneUiOpen = false;
    }

    /// <summary>
    /// Opens the in game menu UI. Makes the mouse pointer visible and freezes the game time. 
    /// </summary>
    private void OpenPauseMenueUi() {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenuUi.SetActive(true);
        pauseMenuUiOpen = true;
    }

    /// <summary>
    /// Close the in game menu UI. Locked the Cursor and lets the game time continue if no other ui ist open.
    /// </summary>
    public void ClosePauseMenueUi() {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUi.SetActive(false);
        pauseMenuUiOpen = false;
        if (!isOneUiOpen) {
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// Opens the options UI
    /// </summary>
    public void OpenOptionsUi() {
        optionsMenuUI.SetActive(true);
        optionsMenuUIOpen = true;
        isOneUiOpen = true;
    }

    /// <summary>
    /// Close the options UI
    /// </summary>
    public void CloseOptionsUi() {
        optionsMenuUI.SetActive(false);
        optionsMenuUIOpen = false;
        isOneUiOpen = false;
    }
    /// <summary>
    /// Opens the control UI
    /// </summary>
    public void OpenControlUi() {
        controlMenuUI.SetActive(true);
        controlMenuUIOpen = true;
        isOneUiOpen = true;
    }

    /// <summary>
    /// Close the control UI
    /// </summary>
    public void CloseControlUi() {
        controlMenuUI.SetActive(false);
        controlMenuUIOpen = false;
        isOneUiOpen = false;
    }
    /// <summary>
    /// Opens the sound UI
    /// </summary>
    public void OpenSoundUi() {
        soundMenuUI.SetActive(true);
        soundMenuUIOpen = true;
        isOneUiOpen = true;
    }

    /// <summary>
    /// Close the sound UI
    /// </summary>
    public void CloseSoundUi() {
        soundMenuUI.SetActive(false);
        soundMenuUIOpen = false;
        isOneUiOpen = false;
    }
    
    /// <summary>
    /// Open the death UI. Makes the mouse pointer visible and freezes the game time. 
    /// </summary>
    public void OpenDeathUi() {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        deathUi.SetActive(true);
        deathUiOpen = true;
    }

    /// <summary>
    /// Close the death UI. Locked the Cursor and lets the game time continue.
    /// </summary>
    public void CloseDeathUi() {
        Cursor.lockState = CursorLockMode.Locked;
        deathUi.SetActive(false);
        deathUiOpen = false;
        Time.timeScale = 1f;
    }


    //------------- UI Button functions -------------

    /// <summary>
    /// Load the main manu of the game -> Scene change
    /// </summary>
    public void LoadMenu() {
        SceneManager.LoadScene("MainMenu"); //load Menu Scene 
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
        if (Input.GetKeyDown(KeyCode.I) && !deathUiOpen) {
            if (inventoryUiOpen ) {
                CloseInventoryUi();
            } else if (!pauseMenuUiOpen && !isOneUiOpen) {
                OpenInventoryUi();
            }
        }

        if (Input.GetKeyDown(KeyCode.K) && !deathUiOpen) {
            if (skillUiOpen) {
                CloseSkillUi();
            } else if (!pauseMenuUiOpen && !isOneUiOpen) {
                OpenSkillyUi();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !deathUiOpen) {
            if (pauseMenuUiOpen && !optionsMenuUI && !controlMenuUI && !soundMenuUI) {
                ClosePauseMenueUi();
            } else {
                OpenPauseMenueUi();
            }
        }

    }
}
