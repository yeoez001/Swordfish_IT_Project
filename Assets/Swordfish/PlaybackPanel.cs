using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackPanel : MonoBehaviour
{
    public RocketTrajectory rocket;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Plays/resumes the rocket animation
    [SerializeField]
    public void playAnimation()
    {
        rocket.playing = true;
    }

    // Pause the rocket animation
    [SerializeField]
    public void pauseAnimation()
    {
        rocket.playing = false;
    }

    // Reset the animation to the beginning
    [SerializeField]
    public void resetAnimation()
    {
        rocket.ResetAnim();
    }
}
