using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameUI.Scripts
{
    /// <summary>
    /// For controlling Popup-menus and the Quit button
    /// </summary>
    public class MenuController : MonoBehaviour
    {
        [Header("Volume Settings")] 
        [SerializeField] private TMP_Text volumeTextValue;       //reference for the volume Text value in the UI
        [SerializeField] private Slider volumeSlider;            //reference for the volume slider
        [SerializeField] private float defaultVolume = 0.5f;

        [Header("Levels To Load")] 
        public string newGameLevel;                                      //reference for the new game level

        public GameObject menu;
   
        
        [SerializeField] private GameObject noSavedGameDialog;          //reference for the no saved game popup
        
        /// <summary>
        /// if the Player hits the "Yes"-Button, it will automatically load a new Game level
        /// </summary>
        public void NewGameDialogYes()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            menu.SetActive(false);
            
        }

        /// <summary>
        /// setting the volume of the music in the game
        /// </summary>
        /// <param name="volume"></param>
        public void SetVolume(float volume)
        {
            AudioListener.volume = volume;
            volumeTextValue.text = volume.ToString("0.0");
        }

        /// <summary>
        /// if the Player hits the Apply button, the volume will be saved as masterVolume
        /// </summary>
        public void VolumeApply()
        {
            PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        }

        /// <summary>
        /// resets the volume value to the default value
        /// </summary>
        public void ResetButton(string menuType)
        {
            if (menuType == "Audio")
            {
                AudioListener.volume = defaultVolume;
                volumeSlider.value = defaultVolume;
                volumeTextValue.text = defaultVolume.ToString("0.0");
                VolumeApply();
            }
        }
    }
}