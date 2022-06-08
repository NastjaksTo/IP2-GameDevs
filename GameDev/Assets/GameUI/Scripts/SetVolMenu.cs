using UnityEngine;
using UnityEngine.Audio;

namespace GameUI.Scripts
{
    /// <summary>
    /// method for volume regulation
    /// </summary>
    public class SetVolMenu : MonoBehaviour
    {
        public AudioMixer mixer;

        /// <summary>
        /// turns value from slider into a logarithmic value
        /// </summary>
        /// <param name="sliderValue"></param>
        public void SetLevel(float sliderValue)
        {
            mixer.SetFloat("MusicVolMenu", Mathf.Log10(sliderValue) * 20);
        }
    }
}
