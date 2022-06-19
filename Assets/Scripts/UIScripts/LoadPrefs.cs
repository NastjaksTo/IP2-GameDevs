using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    /// <summary>
    /// class for loading volume settings
    /// if the player has changed the options, they will be loaded,
    /// if not, they will be set back to default
    /// </summary>
    public class LoadPrefs : MonoBehaviour
    {
        [Header("General Setting")] 
        [SerializeField] private bool canUse = false; //reference if preferences should be load or not
        [SerializeField] private MenuController menuController; //reference for the Menu Controller
        
        [Header("Volume Settings")] 
        [SerializeField] private Slider volumeMusicSlider; //reference for the music volume slider
        private static readonly string MusicPref = "MusicPref";

        [SerializeField] private Slider volumeSoundSlider; //reference for the sounds volume slider
        private static readonly string SoundPref = "SoundPref";

        private float musicFloat, soundFloat;

        [Header("Audio Sources")] 
        public AudioSource musicAudio;
        public AudioSource[] soundEffectsAudio;

        private void Awake()
        {
            if (canUse)
            {
                //if the Player has the keys "MusicPref" and "SoundPref", the values will be loaded
                //if not, the volume Settings will be set to default
                if (PlayerPrefs.HasKey(MusicPref) && PlayerPrefs.HasKey(SoundPref))
                {
                    float localVolumeMusic = PlayerPrefs.GetFloat(MusicPref);
                    float localVolumeSound = PlayerPrefs.GetFloat(SoundPref);

                    volumeMusicSlider.value = localVolumeMusic;

                    volumeSoundSlider.value = localVolumeSound;

                    musicAudio.volume = localVolumeMusic;

                    for (int i = 0; i < soundEffectsAudio.Length; i++)
                    {
                        soundEffectsAudio[i].volume = localVolumeSound;
                    }
                }
                else
                {
                    menuController.ResetButton("Audio");
                }
            }
        }
    }
}