using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraOrbit : MonoBehaviour
{
    private Transform target;       // Target which the object orbits around.
    
    /// <summary>
    /// Get the reference to the target(Player). 
    /// </summary>
    private void Awake()
    {
        target = GameObject.Find("Pandora").transform;
    }


    /// <summary>
    /// Perfom the rotation around the target.
    /// </summary>
    void Update()
    {
        Vector3 pos = target.transform.position + transform.up * 2f;
        transform.position = pos;
    }
}
