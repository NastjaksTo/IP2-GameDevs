using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float speed;
    private Transform target;

    private void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, speed, 0) * Time.deltaTime);
        Vector3 pos = target.transform.position;
        transform.position = pos;
    }

    
}
