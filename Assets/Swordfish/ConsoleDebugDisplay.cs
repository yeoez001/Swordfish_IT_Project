// Source: https://www.youtube.com/watch?v=Pi4SHO0IEQY

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Shows Console messages as Text component of GameObject
// Used for Debugging purposes whilst in VR environment
public class ConsoleDebugDisplay : MonoBehaviour
{
    Dictionary<string, string> debugLogs = new Dictionary<string, string>();
    public Text display;

    private void OnEnable()
    {
        // On log messsage received event, execute HandleLog
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            string[] splitString = logString.Split(char.Parse(":"));
            display.text += "\n" + splitString[0];

        }
    }
}