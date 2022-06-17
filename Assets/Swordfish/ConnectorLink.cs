using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creates a line renderer between two GameObject points
[RequireComponent(typeof(LineRenderer))]
public class ConnectorLink : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();        
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, pointA.transform.position);
        line.SetPosition(1, pointB.transform.position);
    }
}
