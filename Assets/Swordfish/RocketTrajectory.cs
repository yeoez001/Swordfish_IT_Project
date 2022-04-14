using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTrajectory : MonoBehaviour
{
    public GameObject trajectory;
    public LineRenderer lineRenderer;
    int index = 1;
    private List<Vector3> points;// = new List<Vector3>();

    [SerializeField]
    [Range(0f, 1f)]
    private float lerpPct = 0.0f;
    public float lerpIncrease = 0.1f;

    public float speed = 1.0f;

    private bool load = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.Euler(180, 0, 180);
        //// Get the positions from the lineRenderer.
        //points = new List<Vector3>();
        //Vector3[] pos = new Vector3[lineRenderer.positionCount];
        //lineRenderer.GetPositions(pos);
        //Debug.Log(pos.Length);

        //points.AddRange(pos);
        //Debug.Log(points.Count);
        //transform.localPosition = new Vector3(points[0].x, points[0].y, points[0].z);
    }

    // Update is called once per frame
    void Update()
    {
        if (load == false)
        {
            // Get the positions from the lineRenderer.
            points = new List<Vector3>();
            Vector3[] pos = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(pos);
            Debug.Log(pos.Length);

            points.AddRange(pos);
            Debug.Log(points.Count);
            transform.localPosition = new Vector3(points[0].x, points[0].y, points[0].z);

            Vector3 startTarget = points[index] - transform.localPosition;
            transform.localRotation = Quaternion.LookRotation(startTarget);

            load = true;
        }

        if (index < points.Count)
        {
            // Check if the rocket has reached the next point destination. Move destination to next point in list.
            if (Vector3.Distance(transform.localPosition, points[index]) < 0.001f)
            {
                index++;
                // reset the movement percentage
                lerpPct = 0.0f;
            }

            if (index < points.Count)
            {
                var step = speed * Time.deltaTime;

                // Interpolate the rocket between the current position and destination.
                //transform.position = Vector3.Lerp(transform.position, points[index], lerpPct);

                transform.localPosition = Vector3.MoveTowards(transform.localPosition, points[index], step);

                // Rotate the rocket to be along the current gradient of the line 
                Vector3 targetDir = points[index] - transform.localPosition;
                Vector3 newDir = Vector3.RotateTowards(transform.position, targetDir, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);

                //transform.position = Vector3.Lerp(transform.position, endPoint.position, lerpPct);
                lerpPct += lerpIncrease;
            }
        }
    }
}
