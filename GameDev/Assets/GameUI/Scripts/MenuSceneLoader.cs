using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUI.Scripts
{
    /// <summary>
    /// for loading the mainMenu-scene after the scrolling credits have been played
    /// </summary>
    public class MenuSceneLoader : MonoBehaviour
    {
        /// <summary>
        /// once the activation track is called, the function will load the mainMenu-scene
        /// </summary>
        void OnEnable()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        /// <summary>
        /// once the skipBtn is selected, the function will load the mainMenu-scene
        /// </summary>
        public void NextScene()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}