
using UnityWeld.Binding;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.ComponentModel;
using System.Runtime.CompilerServices;

[Binding]
public class CourseViewModel : INotifyPropertyChanged
{
    private string _courseText;
    [Binding]
    public string CourseText
    {
        get => _courseText;
        set
        {
            if (value != _courseText)
            {
                _courseText = value;
                OnPropertyChanged();
            }
        }
    }

    private Action<string, bool> _coursePressed;
    [Binding]
    public Action<string, bool> CoursePressed
    {
        get => _coursePressed;
        set
        {
            if (value != _coursePressed)
            {
                _coursePressed = value;
                OnPropertyChanged();
            }
        }
    }

    public Color UnselectedTextColor
    {
        get;
        set;
    }

    public Color SelectedTextColor
    {
        get;
        set;
    }

    private Color _textColor;
    [Binding]
    public Color TextColor
    {
        get => _textColor;
        set
        {
            if (value != _textColor)
            {
                _textColor = value;
                OnPropertyChanged();
            }
        }
    }

    private string _courseId;
    [Binding]
    public string CourseId
    {
        get => _courseId;
        set
        {
            if (value != _courseId)
            {
                _courseId = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _selected;
    [Binding]
    public bool Selected
    {
        get => _selected;
        set
        {
            if (value != _selected)
            {
                _selected = value;
                OnPropertyChanged();
            }
        }
    }

    [Binding]
    public void OnCoursePressed()
    {

        TextColor = Selected ? SelectedTextColor : UnselectedTextColor;

        CoursePressed(CourseId, Selected);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
