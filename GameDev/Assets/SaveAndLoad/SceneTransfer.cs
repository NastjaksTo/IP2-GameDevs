using System.Collections;
using System.Collections.Generic;
using GameUI.Scripts;
using UnityEngine;
public class SceneTransfer : MonoBehaviour
{
    public GameObject menucontrol;          // Reference to the menucontroller.
    public bool loaded;                     // Boolean to save the value whether or not to load the game.

    /// <summary>
    /// Sets the loaded boolean at the start to false.
    /// Sets the gameobject to not get destroyed when loading diffrent scenes.
    /// </summary>
    void Start()
    {
        loaded = false;
        DontDestroyOnLoad(gameObject);
    }
}
