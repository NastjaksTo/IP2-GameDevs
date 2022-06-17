using UnityEngine;

namespace SaveScripts
{
    public class CheckpointSystem : MonoBehaviour
    {
        public GameObject checkpointUI;
        public static bool checkpointactive;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                checkpointUI.SetActive(true);
                checkpointactive = true;
            }
        }

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
