using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    private InputField focusedInputField;

    void Start()
    {
        focusedInputField = GetComponent<InputField>();
    }

    public void AppendValue(string buttonNum)
    {
        focusedInputField.text += buttonNum;
    }

    public void Backspace()
    {
        focusedInputField.text = focusedInputField.text.Substring(0, focusedInputField.text.Length - 1);
    }

    public float GetValue()
    {
        return float.Parse(focusedInputField.text);
    }

    public void SetFocus(InputField inputField)
    {
        focusedInputField = inputField;
    }

}
