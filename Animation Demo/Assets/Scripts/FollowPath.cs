using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    //for hard "coded" points
    public Transform[] points;
    public int currentPoint = 0;
    public float speed = 1;
    Rigidbody body;

    //trying to get dynamic seeking
    GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    GameObject closestWaypoint;

    void Start()
    {
        body = this.GetComponent<Rigidbody>();
    }

    void Follow()
    {
        //if we are close to a waypoint, increment the current target
        if ((points[currentPoint].position - transform.position).magnitude > 2)
        {
        Vector3 direction = Vector3.Normalize(points[currentPoint].position - transform.position);
        body.AddForce(direction * speed, ForceMode.Force);

        }
        else
        {
            if (currentPoint < 3)
            {
                currentPoint++;
            }
            else
            {
                //reset when at the end of the list
                currentPoint = 0;
            }
        }
    }

    //check all the tagged waypoints for which is closest
    void ScanClosestPoint()
    {
        foreach (var waypoint in waypoints)
        {
            float distance = (waypoint.transform.position - transform.position).magnitude;

            if (distance > closestWaypoint.transform.position.magnitude)
            {
                closestWaypoint = waypoint;
            }
        }
    }

    void Update()
    {
         Follow();
    }
}
