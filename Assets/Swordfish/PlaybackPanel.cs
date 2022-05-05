using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaybackPanel : MonoBehaviour
{
    public RocketTrajectory rocket;

    private TextMeshPro textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text = rocket.GetCurrentDataPoint().GetValuesAsString();
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

    public void test()
    {
        Debug.Log("prsesed");
    }
}
