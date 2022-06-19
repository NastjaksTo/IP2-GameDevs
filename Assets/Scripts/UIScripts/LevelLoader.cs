using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIScripts
{
    /// <summary>
    /// loads the game Scene in the background.
    /// Meanwhile the Player will see a Slider on an Loading Screen, that represents the progress to load.
    /// </summary>
    public class LevelLoader : MonoBehaviour
    {
        public GameObject loadingScreen;            //reference for the Loading Screen to set active
        public Slider slider;                       //reference for the slider to manipulate the value

        /// <summary>
        /// method for loading asynchronously a scene
        /// </summary>
        /// <param name="sceneIndex">BuildIndex of which scene should be loaded asynchronously</param>
        public void LoadLevel(int sceneIndex)
        {
            StartCoroutine(LoadAsynchronously(sceneIndex));
        }

        /// <summary>
        /// private method for asynchronously loading an scene
        /// while the loadingScreen is visible
        /// and the progress value of the bar changes to the progress od loading the scene in the background
        /// </summary>
        /// <param name="sceneIndex">in the method "LoadLevel" given sceneIndex</param>
        private IEnumerator LoadAsynchronously(int sceneIndex)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            loadingScreen.SetActive(true);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);

                slider.value = progress;

                yield return null;
            }
        }
    }
}