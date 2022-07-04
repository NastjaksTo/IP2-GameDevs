using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCharacterCollision : MonoBehaviour
{
    public Collider characterCollider;
    public CapsuleCollider characterBlockerCollider;

    /// <summary>
    /// the two given Collider are not able to collide with each other.
    /// </summary>
    private void Start()
    {
        Physics.IgnoreCollision(characterCollider, characterBlockerCollider, true);
    }
}
