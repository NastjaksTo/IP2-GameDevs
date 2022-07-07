using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PandoraAttack1 : MonoBehaviour
{
    private Transform player;                   // Reference to the transform of the player.
    private CombatSystem combatSystem;          // Reference to the CombatSystem script.
    public AudioClip sounds;                    // Audioclip to play a sound.

    /// <summary>
    /// Gets the player transform.
    /// Gets the combatsystem script.
    /// </summary>
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        combatSystem = player.GetComponent<CombatSystem>();
    }


    /// <summary>
    /// Updates the direction and the speed of the gameobject.
    /// </summary>
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position + transform.up * 1.5f, 10 * Time.deltaTime);
        transform.LookAt(player.position);
    }

    /// <summary>
    /// Checks if the gameobject collides with the player. If thats the case, deal damage to the player and destroy this gameobject.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(sounds, transform.position, 1);
            combatSystem.LoseHealth(30);
            Destroy(gameObject, 2f);
        }
        if(other.gameObject.layer == 3 || other.gameObject.layer == 8) Destroy(gameObject);
    }
}