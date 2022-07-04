using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    [Range(0f, 2f)] [SerializeField] private float waypointSize = .5f;              // Float to save the size of each waypoint.
    
    /// <summary>
    /// Draws gizmos in the scene to better see the actual radius of certain values.
    /// </summary>
    private void OnDrawGizmos()
    {
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(t.position, waypointSize);
        }
        Gizmos.color = Color.blue;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i+1).position);
        }
        Gizmos.DrawLine(transform.GetChild(transform.childCount-1).position, transform.GetChild(0).position);
    }

    /// <summary>
    /// Get the reference to the next checkpoint.
    /// </summary>
    /// <param name="currentWaypoint">Gets the current checkpoint.</param>
    /// <returns>Returns the next checkpoint.</returns>
    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null) 
            return transform.GetChild(0);
        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1)
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        else
            return transform.GetChild(0);
    }
}
