using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PandoraAttack1 : MonoBehaviour
{
    private Transform player;
    private CombatSystem combatSystem;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        combatSystem = player.GetComponent<CombatSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position + transform.up * 1.5f, 10 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            combatSystem.LoseHealth(10);
            Destroy(gameObject, .5f);
        }
    }
}