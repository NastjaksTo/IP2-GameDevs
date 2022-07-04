using UnityEngine;

namespace SaveScripts
{
    public class CheckpointSystem : MonoBehaviour
    {
        public GameObject checkpointUI;             // Reference to the Checkpoint UI.
        public static bool checkpointactive;        // Bool to check whether or not the player is standing next to a runestone/savespot.

        
        /// <summary>
        /// Checks if the player enters the collider of a runestone.
        /// If thats the case, set the value of ceckpointactive to true.
        /// Aswell as activate the checkpoint UI.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                checkpointUI.SetActive(true);
                checkpointactive = true;
            }
        }

        /// <summary>
        /// Checks if the player exists the collider of a runestone.
        /// If thats the case, set the value of ceckpointactive to false.
        /// Aswell as deactivates the checkpoint UI.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                checkpointUI.SetActive(false);
                checkpointactive = false;
            }
        }
    }
}
