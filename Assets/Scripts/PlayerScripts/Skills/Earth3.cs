using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAttributes;

public class Earth3 : MonoBehaviour
{
    /// <summary>
    /// Starts the spell effect.
    /// </summary>
    void Start()
    {
        StartCoroutine(SpellStart());
    }
    
    /// <summary>
    /// Increases players attack power for 11 seconds.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpellStart()
    {
        playerAttributesScript.physicalDamage += 150;
        yield return new WaitForSeconds(11f);
        playerAttributesScript.physicalDamage -= 150;
        Destroy(gameObject, 1f);
    }
}
