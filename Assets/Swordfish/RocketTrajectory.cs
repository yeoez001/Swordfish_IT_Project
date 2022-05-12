using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RocketTrajectory : MonoBehaviour
{
    public DataFiles dataObjects;
    private LineRenderer lineRenderer;
    private List<Vector3> points;
    private List<GameObject> pointsGOs;

    public List<LineRenderer> lineList;
    [SerializeField]
    private int selected = 0; // Which trajectory from 'lineList' the rocket is connected to

    public float speed = 1.0f;
    int index = 1;
    int indexChange = 1;
    public bool playing = true;
    private bool load = false;

    [Range(0,1)]
    public float percent = 0;
    public Slider percentSlider;
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

            // Set the rocket to the first trajectory.
            lineRenderer = lineList[selected];
            Vector3[] pos = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(pos);

            // Set the rocket's starting position
            points.AddRange(pos);
            transform.localPosition = new Vector3(points[0].x, points[0].y, points[0].z);

            // Set the rocket's starting rotation
            Vector3 startTarget = points[index+1] - transform.localPosition;
            transform.localRotation = Quaternion.LookRotation(startTarget);

            pointsGOs = new List<GameObject>();

            // TODO FIX NEXT WEEK
            pointsGOs = dataObjects.transform.Find("DataSource1").GetComponentInChildren<VisualisationPoints>().DataPoints();

            load = true;
        }

        // If the rocket hasn't reached final data point, keep moving along trajectory.
        if (index < points.Count && playing == true)                                                                        //  TAKE AWAY THE COMMENT FOR BUTTONS TO WORK
        {
            float change = ((float)index / (float)points.Count);
            if (percentSlider)
            {
                percentSlider.value = change;
            }
            
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
                Vector3 targetDir;
                if (index != points.Count-1)
                {
                    targetDir = points[index + 1] - transform.localPosition;
                    transform.localRotation = Quaternion.LookRotation(targetDir);
                }
            }
        }

        // If the user has moved the slider, move rocket accordingly
        else if (pctChange != percent)
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

        // Reset the rocket position and local rotation.
        transform.localPosition = new Vector3(points[0].x, points[0].y, points[0].z);
        Vector3 startTarget = points[index] - transform.localPosition;
        transform.localRotation = Quaternion.LookRotation(startTarget);


        // Pause animation.
        playing = false;

        if (percentSlider)
        {
            percentSlider.value = 0;
        }
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
        transform.localRotation = Quaternion.LookRotation(startTarget);

    }

    public DataPoint GetCurrentDataPoint()
    {
        return pointsGOs[index].GetComponent<DataPoint>();
    }
}
