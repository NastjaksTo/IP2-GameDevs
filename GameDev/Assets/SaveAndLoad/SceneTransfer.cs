using System.Collections;
using System.Collections.Generic;
using GameUI.Scripts;
using UnityEngine;
public class SceneTransfer : MonoBehaviour
{
    public GameObject menucontrol;
    public bool loaded;

    // Start is called before the first frame update
    void Start()
    {
        loaded = false;
        DontDestroyOnLoad(gameObject);
    }
}
