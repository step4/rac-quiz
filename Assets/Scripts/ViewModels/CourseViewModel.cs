
using UnityWeld.Binding;
using UnityEngine;
using System;
using UnityEngine.UI;

[Binding]
public class CourseViewModel
{
    [Binding]
    public string CourseText
    {
        get;
        set;
    }

    [Binding]
    public Action<string,bool,Toggle> CoursePressed
    {
        get;
        set;
    }
    [Binding]
    public string CourseId
    {
        get;
        set;
    }
    [Binding]
    public bool Selected
    {
        get;
        set;
    }
    [Binding]
    public Toggle ToggleComponent
    {
        get;
        set;
    }
    [Binding]
    public void OnCoursePressed()
    {
        CourseText = "blub";
        CoursePressed(CourseId,Selected, ToggleComponent);
    }
}
