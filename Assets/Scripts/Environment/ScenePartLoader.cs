using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// method to be used for loading the scenes
/// </summary>
public enum CheckMethod {
    Distince,
    Trigger
}

/// <summary>
/// Load and unload scenes depending on player position by check the distance to the gameobject or the trigger of the gameobject.
/// Put the script to a empty gameobject and name the object the same as the scene that is to be loaded.
/// </summary>
public class ScenePartLoader : MonoBehaviour
{

    public Transform playerPosition;
    public CheckMethod checkMethod;
    public float loadRange;

    private bool isLoaded;
    private bool shouldLoad;


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
    /// Checks whether the player is in the range or in the trigger of the gameobject and calls the respective function to load or unload the scene.
    /// </summary>
    void Update() {
        if(checkMethod == CheckMethod.Distince) {
            DistanceCheck();
        }
        else if (checkMethod == CheckMethod.Trigger) {
            TriggerCheck();
        }

    }

    /// <summary>
    /// check the distance to the gameobject and load or unload the respective scene if the player is in range.
    /// </summary>
    private void DistanceCheck() {
        if (Vector3.Distance(playerPosition.position, transform.position) < loadRange) {
            LoadScene();
        } else {
            UnloadScene();
        }
    }

    /// <summary>
    /// calls the 
    /// </summary>
    private void TriggerCheck() {
        if (shouldLoad) {
            LoadScene();
        }
        else {
            UnloadScene();
        }
    }

    /// <summary>
    /// Checks whether the player is in the collider of the gameobject.
    /// </summary>
    /// <param name="other">the object with which the player collides</param>
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            shouldLoad = true;
        }
    }

    /// <summary>
    /// checks whether the player has left the collider of the gameobject.
    /// </summary>
    /// <param name="other">the object with which the player collides</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) {
            shouldLoad = false;
        }
    }

    /// <summary>
    /// Load the respective scene additive
    /// gameobject.name is the name of the object on which the script is located. The name of the object have to be as the scene that is to be load/unload. 
    /// </summary>
    private void LoadScene() {
        if (!isLoaded) {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            isLoaded = true;
        }
    }


    /// <summary>
    /// Unload the respective scene additive
    /// gameobject.name is the name of the object on which the script is located. The name of the object have to be as the scene that is to be load/unload. 
    /// </summary>
    private void UnloadScene() {
        if (isLoaded) {
            SceneManager.UnloadSceneAsync(gameObject.name);
            isLoaded = false;
        }
    }
}