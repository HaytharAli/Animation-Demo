using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{

    public float speed = 1;
    public int currentPoint = 0;
    Rigidbody body;
    GameObject[] waypoints;
    GameObject closestWaypoint;
    GameObject targetWaypoint;
    
    private void ApproachPoint(GameObject target)
    {
        Vector3 direction = Vector3.Normalize(target.transform.position - body.gameObject.transform.position);
        body.AddForce(direction * speed, ForceMode.Force);
    }

    private int GetCurrentTargetIndex()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == targetWaypoint)
            {
                return i;
            }
        }
        return 0;
    }

    private int GetNextTargetIndex()
    {
        //loop through all waypoints
        for (int i = 0; i < waypoints.Length; i++)
        {
            //if we get the current target
            if (waypoints[i] == targetWaypoint)
            {
                //if the current target is at the end of the array, return the start of the array
                if (i == waypoints.Length - 1)
                {
                    return 0;
                }
                //return the next element of the array
                return i + 1;
            }
        }
        //if something messes up, return the start
        return 0;
    }

    //check all the tagged waypoints for which is closest
    private GameObject ScanClosestPoint()
    {
        foreach (var waypoint in waypoints)
        {
            //distance from the waypoint to the cube
            float distance = (waypoint.transform.position - transform.position).magnitude;
            //if new distance is less than current closest waypoint, set that as closest
            if (distance < closestWaypoint.transform.position.magnitude && distance > 2)
            {
                closestWaypoint = waypoint;
            }
        }
        return closestWaypoint;
    }
    private void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        body = this.GetComponent<Rigidbody>();
        closestWaypoint = waypoints[0];
        targetWaypoint = ScanClosestPoint();
    }

    private void Update()
    {
        //if we are not at the target, move towards it
        if ((targetWaypoint.transform.position - body.transform.position).magnitude > 2)
        {
            ApproachPoint(targetWaypoint);
        }
        //if we are at the target, go to the next
        else
        {
            targetWaypoint = waypoints[GetNextTargetIndex()];
        }
    }
}