using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUI.Scripts
{
    /// <summary>
    /// for loading the game-scene after the game into is been played
    /// </summary>
    public class NextSceneScript : MonoBehaviour
    {
        /// <summary>
        /// once the activation track is called, the function will set the cursor locked and load the game-scene
        /// </summary>
        void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }

        /// <summary>
        /// once the skipBtn is selected, the function will set the cursor locked and load the game-scene
        /// </summary>
        public void NextScene()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            SceneManager.LoadScene("GameScene");
        }
    }
}
