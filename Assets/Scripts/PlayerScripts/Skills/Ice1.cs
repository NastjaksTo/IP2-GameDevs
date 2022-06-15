using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;

public class Ice1 : MonoBehaviour
{
    public Rigidbody rb;
    public float damage;

    private void Awake()
    {
        damage = 10 * (1 + skillTree.skillLevels[1]);
    }
    private void OnTriggerEnter(Collider other)
    {
        rb.velocity = Vector3.zero;
    }
    
}
