using GameUI.Scripts;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")] 
    [SerializeField] private bool canUse = false; //reference if preferences should be load or not
    [SerializeField] private MenuController menuController; //reference for the Menu Controller

    [Header("Volume Setting")] 
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    private void Awake()
    {
        if (canUse)
        {
            //if the MasterVolume file is available, the file will be loaded
            //if not, the volume Settings will be set to default
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                menuController.ResetButton("Audio");
            }
        }
    }
}
