using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using IATK;

public class PlaybackPanel : MonoBehaviour
{
    public RocketTrajectory rocket;

    private TextMeshPro textMeshPro;
    private Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
        dropdown = GetComponentInChildren<Dropdown>();
        
        // Setup dropdown UI with list of data source associated with the visualisation that the rocket is from
        if (dropdown)
        {
            dropdown.ClearOptions();

            // Get data files from the DataFiles component of the visualisation
            List<CSVDataSource> dataSourceList = rocket.gameObject.transform.parent.GetComponentInChildren<DataFiles>().GetFiles();

            // Get the names of those data files as a list
            List<string> dataSourceNames = new List<string>();
            foreach (CSVDataSource source in dataSourceList)
            {
                dataSourceNames.Add(source.name);
            }

            // Add names of data files as dropdown options
            dropdown.AddOptions(dataSourceNames);
        }
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

    // Update the trajectory that the rocket is on based on the dropdown value
    public void updateSelectedTrajectory()
    {
        rocket.SetSelectedTrajectory(dropdown.value);
    }

}
