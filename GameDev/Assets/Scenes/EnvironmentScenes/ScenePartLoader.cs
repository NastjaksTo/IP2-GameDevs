using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// choose the check method
/// </summary>
public enum CheckMethod {
    Distince,
    Trigger
}

/// <summary>
/// check the Distence or the trigge to a scene an loade id (async)
/// </summary>
public class ScenePartLoader : MonoBehaviour {

    public Transform playerPosition;
    public CheckMethod checkMethod;
    public float loadRange;
    private bool isLoaded;

    // Start is called before the first frame update
    void Start() {

        if (SceneManager.sceneCount > 0) {
            for (int i = 0; i < SceneManager.sceneCount; i++) {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == gameObject.name) {
                    isLoaded = true;
                }
            }
        }
    }


    /// <summary>
    /// call the distance check once per frame
    /// </summary>
    void Update() {
        DistanceCheck();
    }

    /// <summary>
    /// check the distence to the scene (just for the "checkpoint" for the scene)
    /// </summary>
    private void DistanceCheck() {
        //Debug.Log(Vector3.Distance(playerPosition.position, transform.position));
        if (Vector3.Distance(playerPosition.position, transform.position) < loadRange) {
            LoadScene();
        } else {
            UnloadScene();
        }
    }

    /// <summary>
    /// load the scene
    /// </summary>
    private void LoadScene() {
        if (!isLoaded) {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            isLoaded = true;
        }
    }


    /// <summary>
    /// unload the scene
    /// </summary>
    private void UnloadScene() {
        if (isLoaded) {
            SceneManager.UnloadSceneAsync(gameObject.name);
            isLoaded = false;
        }
    }
}