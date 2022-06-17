using SaveScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIScripts
{
    /// <summary>
    /// For controlling Popup-menus, the volume and the Quit button
    /// </summary>
    public class MenuController : MonoBehaviour
    {
        [Header("Volume Settings")]
        [SerializeField] private Slider volumeMusicSlider;            //reference for the music volume slider
        private static readonly string MusicPref = "MusicPref";
        
        [SerializeField] private Slider volumeSoundSlider;            //reference for the sounds volume slider
        private static readonly string SoundPref = "SoundPref";
        
        private float musicFloat, soundFloat;
        [SerializeField] private float defaultVolume = 0.5f;

        [Header("Audio Sources")] 
        public AudioSource musicAudio;                                //reference for the music to play
        public AudioSource[] soundEffectsAudio;                       //reference for an array of soundeffects

        [Header("Levels To Load")] 
        public string introGame;                                      //reference for the intro
        public GameObject scenetransfer;
                  
        private string levelToLoad;  
        
        public void Start()
        {
            scenetransfer = GameObject.FindGameObjectWithTag("SceneTransfer");
            Cursor.lockState = CursorLockMode.None;
        }

        /// <summary>
        /// if the Player hits the "Yes"-Button, it will automatically load a new Game level
        /// </summary>
        public void NewGameDialogYes()
        {
            SceneManager.LoadScene(introGame);
            scenetransfer.GetComponent<SceneTransfer>().loaded = false;
        }

        /// <summary>
        /// any moment, that we choose to load the game, we check, if there is a file named "SavedLevel"
        /// </summary>
        public void LoadGameDialogYes()
        {
            scenetransfer.GetComponent<SceneTransfer>().loaded = true;
        }

        /// <summary>
        /// method for quitting the game
        /// </summary>
        public void ExitButton()
        {
            Application.Quit();
        }

        /// <summary>
        /// setting the volume of the music and sounds in the game
        /// the slider for the music controls one sound
        /// the slider for the sounds controls an array of soundEffects
        /// </summary>
        public void SetVolume()
        {
            musicAudio.volume = volumeMusicSlider.value;

            foreach (var soundEffect in soundEffectsAudio)
            {
                soundEffect.volume = volumeSoundSlider.value;
            }
        }

        /// <summary>
        /// if the Player hits the "Apply"-button, the volume value will be saved
        /// </summary>
        public void VolumeApply()
        {
            PlayerPrefs.SetFloat(MusicPref, volumeMusicSlider.value);
            PlayerPrefs.SetFloat(SoundPref, volumeSoundSlider.value);
        }

        /// <summary>
        /// resets the volume value to the default value
        /// </summary>
        public void ResetButton(string menuType)
        {
            if (menuType == "Audio")
            {
                AudioListener.volume = defaultVolume;
                
                //Music Slider
                volumeMusicSlider.value = defaultVolume; 

                //Sounds Slider
                volumeSoundSlider.value = defaultVolume;
                
                VolumeApply();
            }
        }
    }
}