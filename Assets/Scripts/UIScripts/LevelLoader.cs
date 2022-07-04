using System.Collections;
using System.Collections.Generic;
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
        List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
        public Image prograssBar;

        /// <summary>
        /// method for loading asynchronously a scene
        /// </summary>
        /// <param name="sceneIndex">BuildIndex of which scene should be loaded asynchronously</param>
        public void LoadLevel(int sceneIndex)
        {
            loadingScreen.SetActive(true);
            scenesToLoad.Add(SceneManager.LoadSceneAsync(sceneIndex));
            StartCoroutine(LoadAsynchronously());
        }

        /// <summary>
        /// private method for asynchronously loading an scene while the loadingScreen is visible and the progress value of the bar changes to the progress of loading the scene in the background
        /// </summary>
        private IEnumerator LoadAsynchronously()
        {
            float totalProgress = 0;

            for (int i = 0; i < scenesToLoad.Count; i++)
            {
                while (!scenesToLoad[i].isDone)
                {

                    totalProgress += scenesToLoad[i].progress;
                    prograssBar.fillAmount = totalProgress / scenesToLoad.Count;
                    yield return null;
                }
            }
        }
    }
}