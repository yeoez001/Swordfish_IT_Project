using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IATK;

public class RocketAnimation : MonoBehaviour
{
    // DataFiles of the associated visualisation
    public DataFiles dataObjects;

    // Lines of all trajectories within the visualisation
    public List<LineRenderer> lineList;
    private LineRenderer lineRenderer;

    // Points of the current selected trajectory
    private List<Vector3> points;
    // Current index the rocket is at within the points variable
    private int index = 1;
    private List<GameObject> pointsGOs;   

    // Animation settings
    private float speed = 1.0f;     
    private bool playing = false;

    // Position of the rocket in terms of the entire trajectory as a percentage
    [Range(0, 1)]
    private float percent = 0;   
    private float pctChange = 0;

    // UI Component manager
    private RocketAnimationUI animationUI;

    private bool load = false;

    void Start()
    {
        // Offset the initial local rotation of object
        transform.localRotation = Quaternion.Euler(-90, 0, 0);        
        points = new List<Vector3>();
        pointsGOs = new List<GameObject>();
        animationUI = GetComponent<RocketAnimationUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // During first frame, load trajectory points from line renderer and set rocket starting position.
        if (load == false)
        {
            // Set the selected trajectory to the first data source
            setSelectedTrajectory(0);
            load = true;
        }

        // If the rocket hasn't reached final data point, keep moving along trajectory.
        if (index < points.Count && playing == true)                                   
        {
            float change = ((float)index / (float)points.Count);
            if (animationUI.getPercentSlider() && change > 0)
            {
                animationUI.getPercentSlider().value = change;
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
                if (index != points.Count - 1)
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
            sliderChanged(percent);
            pctChange = percent;
        }

    }

    // Reset the rocket animation back to the beginning data point
    public void resetAnimation()
    {
        // Only stop animation if rocket is at the end of the trajectory.  Else, keep it as it was previously. 
        if (index >= points.Count)
        {
            playing = false;
        }

        index = 1;

        // Reset the rocket position and local rotation.
        transform.localPosition = new Vector3(points[0].x, points[0].y, points[0].z);
        Vector3 startTarget = points[index] - transform.localPosition;
        transform.localRotation = Quaternion.LookRotation(startTarget);

        // Slider is not null
        if (animationUI.getPercentSlider())
        {
            animationUI.getPercentSlider().value = 0;
        }
    }

    // When the user moves along the slider, move the rocket accordingly.
    public void sliderChanged(float pct)
    {
        // Calculate the point closest to percentage
        // Change index to correct value relative to slider percentage
        index = (int)(points.Count * pct);
        // Slider percentage is 1
        if (pct == 1)
        {
            // Move rocket to the last trajectory point
            transform.localPosition = points[points.Count - 1];
            index = points.Count - 1;
        }
        // Else move and rotate rocket to face the next point
        else if (index < points.Count - 1)
        {
            transform.localPosition = points[index];
            Vector3 startTarget = points[index + 1] - transform.localPosition;
            transform.localRotation = Quaternion.LookRotation(startTarget);
        }
    }

    // Get the current DataPoint object that the trajectory is at
    public DataPoint getCurrentDataPoint()
    {        
        if (index < pointsGOs.Count)
        {
            return pointsGOs[index].GetComponent<DataPoint>();
        }
        return null;
    }

    // Set the trajectory that the rocket is animated across based on its index of data source in the editor
    public void setSelectedTrajectory(int selectedIndex)
    {
        // Get the positions from the lineRenderer.
        points.Clear();
        // Set the rocket to the first trajectory.
        lineRenderer = lineList[selectedIndex];
        Vector3[] pos = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(pos);

        // Set the rocket's starting position and rotation
        points.AddRange(pos);
        resetAnimation();

        pointsGOs = dataObjects.GetFiles()[selectedIndex].GetComponentInChildren<VisualisationPoints>().DataPoints();
        
        // Trajectory source dropdown is not null
        if (animationUI.getDropdown())
            animationUI.getDropdown().value = selectedIndex;
    }

    // Set the trajectory that the rocket is animated across based on the user selecting a trajectory's data point
    public void setSelectedTrajectory(GameObject trajectoryObject)
    {
        // Get the data source that the trajectoryObject derives from. trajectoryObject is either the line or data point
        // and thus, would be child of the data source.
        CSVDataSource dataSource = trajectoryObject.GetComponentInParent(typeof(CSVDataSource)) as CSVDataSource;

        if (dataSource && dataObjects)
        {
            int dataSourceIndex = dataObjects.GetFiles().IndexOf(dataSource);
            setSelectedTrajectory(dataSourceIndex);
        }
    }

    // Set whether the animation is playing 
    public void setPlaying(bool play)
    {
        playing = play;
    }
}