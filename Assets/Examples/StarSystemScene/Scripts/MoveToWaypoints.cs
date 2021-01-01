using UnityEngine;
using System.Collections;

public class MoveToWaypoints : MonoBehaviour
{

    //This array is used to set which points are the waypoints
    public GameObject[] wayPoints;

    //Speed and distance. Distance is used to determine how close you
    //want the object to get before going to next point.
    public float speed = 1f;
    public float distance = .01f;

    //Holds the current waypoint it is going to
    public int currentPoint = 1;

    //Holds the position of the waypoint it's heading towards.
    private Vector3 targetPosition;
    private bool ascending = true;

    void Start()
    {
        if (speed < 0)
        {
            ascending = false;
            currentPoint = wayPoints.Length - 1;
        }
        else
        {
            currentPoint = 1;
        }
        //Sets  the first waypointPosition
        targetPosition = wayPoints[currentPoint].transform.position;
        transform.position = wayPoints[0].transform.position;
        // Debug.Log("Current Position = " + currentPoint + " Distance = " + Vector3.Distance(transform.position, targetPosition));
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if the distance from the object to the way point is less than distance given
        //Once true it will increment to the next point.
        if ((Vector3.Distance(transform.position, targetPosition) < distance))
        {
            if (ascending)
                nextPointAscending();
            else
                nextPointDescending();
        }
        else
        {
            //float dir = 0;
            //Moving and turning
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, Mathf.Abs(speed) * .001f);

        }
    }

    private void nextPointAscending()
    {
        //Checks to see if it reached the max waypoint possible
        if (currentPoint >= wayPoints.Length - 1)
            //If true sets it to 0 so array doesn't go out of bounds.
            currentPoint = 0;
        else
            currentPoint++;
        //Sets the the targetPosition to the next waypoint.
        targetPosition = wayPoints[currentPoint].transform.position;

    }

    private void nextPointDescending()
    {
        //Checks to see if it reached the max waypoint possible
        if (currentPoint == 0)
            //If true sets it to 0 so array doesn't go out of bounds.
            currentPoint = wayPoints.Length - 1;
        else
            currentPoint--;
        //Sets the the targetPosition to the next waypoint.
        targetPosition = wayPoints[currentPoint].transform.position;

    }

    public void UpdateTargetPosition()
    {
        targetPosition = wayPoints[currentPoint].transform.position;
    }
}