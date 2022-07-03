using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoVScript : MonoBehaviour
{
    [SerializeField]
    private float radius;                                                                       //FoV Range of the Enemy
    [SerializeField]
    [Range(0,360)]
    private float angle;                                                                        //FoV Angle of the Enemy

    [SerializeField]
    private GameObject playerRef;                                                               //Target (Player) Reference
    
    [SerializeField]
    private bool canSeePlayer;                                                                  //When the Player is inside the Range and Angle this is true

    [SerializeField]
    private LayerMask targetMask;                                                               //LayerMask of the Player (only the Player)

    [SerializeField]
    private LayerMask obstructionMask;                                                          //LayerMask of Obstacles Enemies should not see through

    public bool CanSeePlayer { get => canSeePlayer; set => canSeePlayer = value; }
    public float Angle { get => angle; set => angle = value; }
    public float Radius { get => radius; set => radius = value; }

    /// <summary>
    /// Find Player and Start Coroutine for Player Detection
    /// </summary>
    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    /// <summary>
    /// Coroutine updates the Bool 5 times a second, instead of every Frame (for Performance)
    /// </summary>
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    /// <summary>
    /// rangeChecks[]: if something on the targetMask enters the radius of the Enemy the rangeCheck Array gets an Item (cause the Player is the only one on this Mask its always the Player)
    /// 
    /// directionToTarget: direction between Enemy and Target (its cut in Half cause half of the Angle is right Sight, half left Sight)
    /// 
    /// Raycast: direct Line between Enemy and Target if there is nothing between on the obstructionMask
    /// 
    /// 
    /// so if some Collider on the targetMask is in the rangeCheck (always the Player) and this Object is inside of the View Angle and the Radius, the Enemy canSeePlayer = true
    /// if the Player is not inside of the ViewAngle, but in the Range, the canSeePlayer will be false.
    /// if the Distance between the Target and the Enemy is getting bigger than its Range, the Enemy will not be able to see the Player anymore.
    /// 
    /// This is solved through the Distance Vector, so that when the Enemy once saw the Player, it is not possible to hide behind the Enemy.
    /// </summary>
    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if(rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
            }
            else if (canSeePlayer && distanceToTarget <= radius)
            {
                canSeePlayer = true;
            }
            else
                canSeePlayer = false;
        }
        else if(Vector3.Distance(transform.position, playerRef.transform.position) > radius )
        {
            canSeePlayer = false;
        }
    }
}
