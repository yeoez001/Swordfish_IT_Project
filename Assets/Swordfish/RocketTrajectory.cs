using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTrajectory : MonoBehaviour
{
    public DataFiles dataObjects;
    private LineRenderer lineRenderer;
    private List<Vector3> points;

    public float speed = 1.0f;
    int index = 1;
    public bool playing = true;
    private bool load = false;

    [Range(0,1)]
    public float percent = 0;
    private float pctChange = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Offset the initial local rotation of object
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

            // TODO
            // THIS IS A TERRIBLE IMPLEMENTATION.
            // Create a new script and link to each "DataSource". When the point and line objects
            // have been created in DataFiles script, add them to that data source and then access it properly
            // from RocketTrajectory.
            lineRenderer = dataObjects.transform.Find("DataSource1").Find("LineRenderer0").GetComponent<VisualisationLine>().line;


            Vector3[] pos = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(pos);

            // Set the rocket's starting position
            points.AddRange(pos);
            transform.localPosition = new Vector3(points[0].x, points[0].y, points[0].z);

            // Set the rocket's starting rotation
            Vector3 startTarget = points[index+1] - transform.localPosition;
            transform.rotation = Quaternion.LookRotation(startTarget);
            load = true;
        }

        // If the rocket hasn't reached final data point, keep moving along trajectory.
        if (index < points.Count)// && playing == true) //  TAKE AWAY THE COMMENT FOR BUTTONS TO WORK
        {
            //Check if the rocket has reached the next point destination. Move destination to next point in list.
            if (Vector3.Distance(transform.localPosition, points[index]) < 0.00001f)
            {
                index++;
            }

            if (index < points.Count)
            {
                var step = speed * Time.deltaTime;

                //Interpolate the rocket between the current position and destination.
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, points[index], step);

                // Rotate the rocket to be along the current gradient of the line
                Vector3 targetDir = points[index+1] - transform.localPosition;
                transform.rotation = Quaternion.LookRotation(targetDir);
            }
        }

        // If the user has moved the slider, move rocket accordingly
        if (pctChange != percent)
        {
            playing = false;
            Slider(percent);
            pctChange = percent;
        }
    }

    // Reset the rocket animation back to the beginning data point
    public void ResetAnim()
    {
        index = 1;

        // Reset the rocket position and rotation.
        transform.localPosition = new Vector3(points[0].x, points[0].y, points[0].z);
        Vector3 startTarget = points[index] - transform.localPosition;
        transform.localRotation = Quaternion.LookRotation(startTarget);

        // Pause animation.
        playing = false;
    }

    // When the user moves along the slider, move the rocket accordingly.
    public void Slider(float pct)
    {
        // Calculate the point closest to percentage
        // Change index to correct value
        index = (int)(points.Count * pct);

        // Move and rotate rocket
        transform.localPosition = points[index];
        Vector3 startTarget = points[index+1] - transform.localPosition;
        transform.rotation = Quaternion.LookRotation(startTarget);
    }
}
