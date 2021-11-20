using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public float speed = 1;
    public float lineFactor = 1;
    float t = 0;
    public float currentSpeed;
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

    Vector3 GetCurvePosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
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
        int p0, p1, p2, p3;

        if (targetIndex == 0)
        {
            // edge case if initial target ends up as the first element,
            // this can't actually happen because of the start function but i'll leave it in
            p0 = waypoints.Length - 2;
            p1 = waypoints.Length - 1;
            p2 = targetIndex;
            p3 = 1;
        }
        else if (targetIndex == 1)// if moving from first to second
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

        float length = SegmentLength(
            waypoints[p0].transform.position,
            waypoints[p1].transform.position,
            waypoints[p2].transform.position,
            waypoints[p3].transform.position);

        //basic speed control, longer segments take longer to cross
        t += (Time.deltaTime / (length / 5)) * lineFactor;

        Vector3 curvePos = GetCurvePosition(
            t,
            waypoints[p0].transform.position,
            waypoints[p1].transform.position,
            waypoints[p2].transform.position,
            waypoints[p3].transform.position);

        Vector3 direction = curvePos - transform.position;

        body.AddForce(direction * speed, ForceMode.Force);
        transform.forward = direction; // turn towards where we're going
    }

    float SegmentLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float totalLength = 0;
        Vector3 lastPoint, newPoint;
        lastPoint = p1;
        //sample the current segment 4 times (0.25, 0.5, 0.75, 1)
        for (int i = 1; i <= 4; i++)
        {
            newPoint = GetCurvePosition(i/4, p0, p1, p2, p3);

            totalLength += (newPoint - lastPoint).magnitude;

            lastPoint = newPoint;
        }
        return totalLength;
    }

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
        if (t < 1)
        {
            MoveOnCurve();
            currentSpeed = body.velocity.magnitude;
        }
        else
        {
            targetIndex++;
            t = 0;
        }
    }
}