using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUI
{
    /// <summary>
    /// Manage the in game Uis. Open and close them and defines the functions of the buttons in the menus.
    /// </summary>
    public class UiScreenManager : MonoBehaviour {

        [Header("InventoryUI")]
        private static bool _inventoryUiOpen = false;
        public GameObject inventoryUI;                      //reference set in editor
    
        [Header("SkillUI")]
        private static bool _skillUiOpen = false;
        public GameObject skillUi;                          //reference set in editor
    
        [Header("PauseMenuUI")]
        private static bool _pauseMenuUiOpen = false;
        public GameObject pauseMenuUi;                           //reference set in editor

        [Header("OptionsMenuUI")]
        private static bool _optionsMenuUIOpen = false;
        public GameObject optionsMenuUI;                           //reference set in editor

        [Header("ControlMenuUI")]
        private static bool _controlMenuUIOpen = false;
        public GameObject controlMenuUI;                           //reference set in editor

        [Header("SoundMenuUI")]
        private static bool _soundMenuUIOpen = false;
        public GameObject soundMenuUI;                           //reference set in editor

        [Header("DeathUI")]
        private static bool _deathUiOpen = false;
        public GameObject deathUi;                          //reference set in editor

        private static bool _isOneUiOpen = false;


        /// <summary>
        /// Opens the inventory UI. Makes the mouse pointer visible and freezes the game time. 
        /// </summary>
        private void OpenInventoryUi() {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            inventoryUI.SetActive(true);
            _inventoryUiOpen = true;
            _isOneUiOpen = true;
        }

        /// <summary>
        /// Close the inventory UI. Locked the Cursor and lets the game time continue.
        /// </summary>
        private void CloseInventoryUi() {
            Cursor.lockState = CursorLockMode.Locked;
            inventoryUI.SetActive(false);
            _inventoryUiOpen = false;
            Time.timeScale = 1f;
            _isOneUiOpen = false;
        }

        /// <summary>
        /// Opens the skill UI. Makes the mouse pointer visible and freezes the game time. 
        /// </summary>
        private void OpenSkillUi() {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            skillUi.SetActive(true);
            _skillUiOpen = true;
            _isOneUiOpen = true;
        }

        /// <summary>
        /// Close the skill UI. Locked the Cursor and lets the game time continue.
        /// </summary>
        private void CloseSkillUi() {
            Cursor.lockState = CursorLockMode.Locked;
            skillUi.SetActive(false);
            _skillUiOpen = false;
            Time.timeScale = 1f;
            _isOneUiOpen = false;
        }

        /// <summary>
        /// Opens the in game menu UI. Makes the mouse pointer visible and freezes the game time. 
        /// </summary>
        private void OpenPauseMenuUi() {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            pauseMenuUi.SetActive(true);
            _pauseMenuUiOpen = true;
        }

        /// <summary>
        /// Close the in game menu UI. Locked the Cursor and lets the game time continue if no other ui ist open.
        /// </summary>
        public void ClosePauseMenuUi() {
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenuUi.SetActive(false);
            _pauseMenuUiOpen = false;
            if (!_isOneUiOpen) {
                Time.timeScale = 1f;
            }
        }

        /// <summary>
        /// Opens the options UI
        /// </summary>
        public void OpenOptionsUi() {
            optionsMenuUI.SetActive(true);
            _optionsMenuUIOpen = true;
            _isOneUiOpen = true;
        }

        /// <summary>
        /// Close the options UI
        /// </summary>
        public void CloseOptionsUi() {
            optionsMenuUI.SetActive(false);
            _optionsMenuUIOpen = false;
            _isOneUiOpen = false;
        }
        /// <summary>
        /// Opens the control UI
        /// </summary>
        public void OpenControlUi() {
            controlMenuUI.SetActive(true);
            _controlMenuUIOpen = true;
            _isOneUiOpen = true;
        }

        /// <summary>
        /// Close the control UI
        /// </summary>
        public void CloseControlUi() {
            controlMenuUI.SetActive(false);
            _controlMenuUIOpen = false;
            _isOneUiOpen = false;
        }
        /// <summary>
        /// Opens the sound UI
        /// </summary>
        public void OpenSoundUi() {
            soundMenuUI.SetActive(true);
            _soundMenuUIOpen = true;
            _isOneUiOpen = true;
        }

        /// <summary>
        /// Close the sound UI
        /// </summary>
        public void CloseSoundUi() {
            soundMenuUI.SetActive(false);
            _soundMenuUIOpen = false;
            _isOneUiOpen = false;
        }
    
        /// <summary>
        /// Open the death UI. Makes the mouse pointer visible and freezes the game time. 
        /// </summary>
        public void OpenDeathUi() {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            deathUi.SetActive(true);
            _deathUiOpen = true;
        }

        /// <summary>
        /// Close the death UI. Locked the Cursor and lets the game time continue.
        /// </summary>
        public void CloseDeathUi() {
            Cursor.lockState = CursorLockMode.Locked;
            deathUi.SetActive(false);
            _deathUiOpen = false;
            Time.timeScale = 1f;
        }


        //------------- UI Button functions -------------

        /// <summary>
        /// Load the main menu of the game -> Scene change
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
        /// Checks each frame the input for the keys I and Esc
        /// On I and if the DeathUi is closed: close the inventoryUI when its open, or open it if its closed and the menu is not opened.
        /// On R and if the DeathUi is closed: close the skillUi when its open, or open it if its closed and the menu is not opened.
        /// On Esc and if the DeathUi is closed: close the menuUI when its open, or open it if its closed.
        /// </summary>
        public void Update() {
            if (Input.GetKeyDown(KeyCode.I) && !_deathUiOpen) {
                if (_inventoryUiOpen ) {
                    CloseInventoryUi();
                } else if (!_pauseMenuUiOpen && !_isOneUiOpen) {
                    OpenInventoryUi();
                }
            }

            if (Input.GetKeyDown(KeyCode.K) && !_deathUiOpen) {
                if (_skillUiOpen) {
                    CloseSkillUi();
                } else if (!_pauseMenuUiOpen && !_isOneUiOpen) {
                    OpenSkillUi();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !_deathUiOpen) {
                if (_pauseMenuUiOpen && !optionsMenuUI && !controlMenuUI && !soundMenuUI) {
                    ClosePauseMenuUi();
                } else {
                    OpenPauseMenuUi();
                }
            }

        }
    }
}
