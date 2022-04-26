using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;
using UnityEngine.Events;

public class Test : MonoBehaviour
{
    public UnityEvent interactEvent;
    
    void Start()
    {
    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // if trigger click
        interactEvent.Invoke();
    }
}
