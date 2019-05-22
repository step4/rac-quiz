
using UnityWeld.Binding;
using UnityEngine;
using System;

[Binding]
public class AnswerViewModel
{
    [Binding]
    public string AnswerText
    {
        get;
        set;
    }

    [Binding]
    public Action<bool> AnswerPressed
    {
        get;
        set;
    }
    [Binding]
    public Sprite ToggleBG
    {
        get;
        set;
    }
    [Binding]
    public bool IsRight
    {
        get;
        set;
    }
    [Binding]
    public void OnAnswerPressed()
    {
        AnswerPressed(IsRight);
    }
}
