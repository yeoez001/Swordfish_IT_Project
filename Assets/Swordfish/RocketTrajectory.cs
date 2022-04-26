using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTrajectory : MonoBehaviour
{
    public GameObject trajectory;
    public LineRenderer lineRenderer;
    int index = 1;
    private List<Vector3> points;

    [SerializeField]
    [Range(0f, 1f)]
    private float lerpPct = 0.0f;
    public float lerpIncrease = 0.1f;

    public float speed = 10.0f;

    public bool playing = false;
    private bool load = false;

    private float zOffset = -90.0f;

    // Start is called before the first frame update
    void Start()
    {
        //transform.localRotation = Quaternion.Euler(90, 0, 0);    // I can't remember why this is here. will look into it.
        transform.localRotation = Quaternion.Euler(-90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // During first frame, load trajectory points from line renderer and set rocket starting position.
        if (load == false)
        {
            // Get the positions from the lineRenderer.
            points = new List<Vector3>();
            Vector3[] pos = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(pos);

            points.AddRange(pos);
            transform.localPosition = new Vector3(points[0].x, points[0].y, points[0].z);

            Vector3 startTarget = points[index] - transform.localPosition;
            transform.rotation = Quaternion.LookRotation(startTarget);

            //transform.rotation = Quaternion.AngleAxis(zOffset, Vector3.forward) * Quaternion.LookRotation(startTarget);
            load = true;
        }

        // If the rocket hasn't reached final data point, keep moving along trajectory.
        if (index < points.Count)// && playing == true)   TAKE AWAY THE COMMENT FOR BUTTONS TO WORK
        {
            //Check if the rocket has reached the next point destination. Move destination to next point in list.
            if (Vector3.Distance(transform.localPosition, points[index]) < 0.00001f)
            {
                index++;
                //reset the movement percentage
                lerpPct = 0.0f;
            }

            if (index < points.Count)
            {
                var step = speed * Time.deltaTime;

                //Interpolate the rocket between the current position and destination.
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, points[index], step);

                // Rotate the rocket to be along the current gradient of the line
                Vector3 targetDir = points[index+1] - transform.localPosition;
                //transform.LookAt(targetDir, Vector3.forward);
                //Vector3 newDir = Vector3.RotateTowards(transform.position, targetDir, step, 0.0f);
                //transform.rotation = Quaternion.LookRotation(newDir);


                transform.rotation = Quaternion.LookRotation(targetDir);

                // TESTING BELOW

                //Quaternion rotate = new Quaternion();
                //if (Quaternion.LookRotation(targetDir).x > 0)
                //{
                //    rotate = Quaternion.AngleAxis(-zOffset, new Vector3(1, 0, 0)) * Quaternion.LookRotation(targetDir);
                //}
                //if (Quaternion.LookRotation(targetDir).x < 0)
                //{
                //    rotate = Quaternion.AngleAxis(zOffset, Vector3.forward) * Quaternion.LookRotation(targetDir);
                //}



                // TESTING ABOVE


                // To get it back to it was, remove the block above and uncomment line below.
                //Quaternion rotate = Quaternion.AngleAxis(zOffset, Vector3.forward) * Quaternion.LookRotation(targetDir);

                //transform.rotation = rotate;

                lerpPct += lerpIncrease;
            }
        }
    }

    // Reset the rocket animation back to the beginning data point
    public void ResetAnim()
    {
        index = 1;
        lerpPct = 0.0f; // Might be able to delete

        // Reset the rocket position and rotation.
        transform.localPosition = new Vector3(points[0].x, points[0].y, points[0].z);
        Vector3 startTarget = points[index] - transform.localPosition;
        transform.localRotation = Quaternion.LookRotation(startTarget);

        // Pause animation.
        playing = false;
    }
}
