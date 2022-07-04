using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveScripts
{
    public class SceneTransfer : MonoBehaviour
    {
        public SceneManager instance;
        public GameObject menucontrol;          // Reference to the menucontroller.
        public bool loaded;                     // Boolean to save the value whether or not to load the game.

        /// <summary>
        /// Sets the loaded boolean at the start to false.
        /// Sets the gameobject to not get destroyed when loading diffrent scenes.
        /// </summary>
        void Start()
        {
            if (GameObject.FindGameObjectsWithTag("SceneTransfer").Length >= 2)
            {
                Destroy(gameObject);
            }

            loaded = false;
            DontDestroyOnLoad(gameObject);
        }
    }
    
}
