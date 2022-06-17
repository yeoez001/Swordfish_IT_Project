using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI Keypad 
// Functions called by UI component
public class Keypad : MonoBehaviour
{
    private InputField lastFocusedInputField;

    // Add button value to the input field
    public void appendValue(string buttonNum)
    {
        lastFocusedInputField.text += buttonNum;
    }

    // Backspace input field
    public void backspace()
    {
        lastFocusedInputField.text = lastFocusedInputField.text.Substring(0, lastFocusedInputField.text.Length - 1);
    }

    // Get value from the input field as a float
    public float getValue()
    {
        return float.Parse(lastFocusedInputField.text);
    }

    // Set the last focused input field
    public void setFocus(InputField inputField)
    {
        lastFocusedInputField = inputField;
    }

}
