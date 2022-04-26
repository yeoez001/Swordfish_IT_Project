using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class playAnim : MonoBehaviour
{
    [SerializeField]
    public UnityEvent playAnimationEvent;

    private void OnCollisionEnter(Collision collision)
    {
        playAnimationEvent.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
