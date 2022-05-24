using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schuss : MonoBehaviour
{

    private Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Start()
    {
        rb.AddForce(rb.transform.forward * 100);
    }
}
