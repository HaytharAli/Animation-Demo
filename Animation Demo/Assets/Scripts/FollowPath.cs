using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public float speed = 1;
    float t = 0;
    Rigidbody body;
    GameObject[] waypoints;
    GameObject closestWaypoint;
    GameObject targetWaypoint;
    int targetIndex = 0;
    
    private int GetWaypointIndex(GameObject selection)
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i].Equals(selection))
            {
                return i;
            }
        }
        return 0;
    }

    Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

        return pos;
    }

    void MoveOnCurve()
    {
        int p0 = 0, p1 = 0, p2 = 0, p3 = 0;

        if (targetIndex == 0)// edge case if initial target ends up as the first element
        {
            p0 = waypoints.Length - 2;
            p1 = waypoints.Length - 1;
            p2 = targetIndex;
            p3 = 1;
        }
        else if (targetIndex == 1)// if moving from fist to second
        {
            p0 = waypoints.Length - 1;
            p1 = 0;
            p2 = targetIndex;
            p3 = 2;
        }
        else if (targetIndex == waypoints.Length - 1)// if moving from second-last to last
        {
            p0 = targetIndex - 2;
            p1 = targetIndex - 1;
            p2 = targetIndex;
            p3 = 0;

        }
        else if (targetIndex == waypoints.Length)// if trying to move past the last
        {
            p0 = waypoints.Length - 2;
            p1 = waypoints.Length - 1;
            p2 = 0;
            p3 = 1;
            targetIndex = 0;
        }
        else
        {
            p0 = targetIndex - 2;
            p1 = targetIndex - 1;
            p2 = targetIndex;
            p3 = targetIndex + 1;
        }

        Vector3 curvePos = (GetCatmullRomPosition(
            t,
            waypoints[p0].transform.position,
            waypoints[p1].transform.position,
            waypoints[p2].transform.position,
            waypoints[p3].transform.position));

        Vector3 direction = curvePos - transform.position;

        body.AddForce(direction * speed, ForceMode.Force);
    }

    //check all the tagged waypoints for which is closest
    private GameObject ScanClosestPoint()
    {
        foreach (var waypoint in waypoints)
        {
            //distance from the waypoint to the cube
            float distance = (waypoint.transform.position - transform.position).magnitude;
            //if new distance is less than current closest waypoint, set that as closest
            if (distance < (closestWaypoint.transform.position - transform.position).magnitude)
            {
                closestWaypoint = waypoint;
            }
        }
        return closestWaypoint;
    }
    private void Start()
    {
        //fill the waypoint array, default the closest point to the first element, then scan through the array for the actual closest
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        body = this.GetComponent<Rigidbody>();
        closestWaypoint = waypoints[1];
        targetWaypoint = ScanClosestPoint();
        targetIndex = GetWaypointIndex(targetWaypoint) + 1;// +1 so that we start on the path going FROM the closest, not to the closest
    }

    private void Update()
    {
        t += Time.deltaTime / 4;
        if (t < 1)
        {
            MoveOnCurve();

        }
        else
        {
            targetIndex++;
            t = 0;
        }
    }
}