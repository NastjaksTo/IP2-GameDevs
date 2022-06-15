using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float speed;             // Float to manage the speed of the orbit.
    private Transform target;       // Target which the object orbits around.
    
    /// <summary>
    /// Get the reference to the target(Player). 
    /// </summary>
    private void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
    }


    /// <summary>
    /// Perfom the rotation around the target.
    /// </summary>
    void Update()
    {
        transform.Rotate(new Vector3(0, speed, 0) * Time.deltaTime);
        Vector3 pos = target.transform.position;
        transform.position = pos;
    }

    
}
