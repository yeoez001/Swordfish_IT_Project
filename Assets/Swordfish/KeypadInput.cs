using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadInput : MonoBehaviour
{
    private InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<InputField>();
    }

    public void AppendValue(string buttonNum)
    {
        inputField.text += buttonNum;
    }

    public void Backspace()
    {
        inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
    }

    public float GetValue()
    {
        return float.Parse(inputField.text);
    }
}
