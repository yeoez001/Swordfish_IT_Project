using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

// This class is for creaeting a dynamic dropdown list in the Unity Editor at runtime.
// Used by the Filtering script.

public class FilteringEditor : PropertyAttribute
{
    public Type list;
    public string listName;

    public FilteringEditor(Type _list, string _listName)
    {
        list = _list;
        listName = _listName;
    }
}

// Creates a dropdown list UI component in the Unity Editor
[CustomPropertyDrawer(typeof(FilteringEditor))]
public class ListPopupDrawer : PropertyDrawer
{
    int selectedIndex = 0;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        FilteringEditor attr = attribute as FilteringEditor;
        List<string> stringList = null;

        if (attr.list.GetField(attr.listName) != null)
        {
            stringList = attr.list.GetField(attr.listName).GetValue(attr.list) as List<string>;
        }

        if (stringList != null && stringList.Count != 0)
        {
            selectedIndex = EditorGUI.Popup(position, property.name, selectedIndex, stringList.ToArray());
            property.stringValue = stringList[selectedIndex];
        }
        else EditorGUI.PropertyField(position, property, label);
    }
}