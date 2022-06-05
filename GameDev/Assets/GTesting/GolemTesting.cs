using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemTesting : MonoBehaviour
{
    public Transform playerObject;
    public Animator anim;

    public float health = 1000;

    // Start is called before the first frame update
    void Start()
    { 
        
        
    }


    

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerObject);
        if (Input.GetKeyDown(KeyCode.U))
        {
            anim.SetBool("golemtesting", true);
        } else anim.SetBool("golemtesting", false);
    }
}
