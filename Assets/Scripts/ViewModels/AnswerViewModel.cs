
using UnityWeld.Binding;
using UnityEngine;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

[Binding]
public class AnswerViewModel : INotifyPropertyChanged
{
    private string _answerText;
    [Binding]
    public string AnswerText
    {
        get => _answerText;
        set
        {
            if (_answerText != value)
            {
                _answerText = value;
                OnPropertyChanged();
            }
        }
    }

    private Action<bool,int> _answerPressed;
    [Binding]
    public Action<bool,int> AnswerPressed
    {
        get => _answerPressed;
        set
        {
            if (_answerPressed != value)
            {
                _answerPressed = value;
                OnPropertyChanged();
            }
        }
    }

    private Sprite _toggleBG;
    [Binding]
    public Sprite ToggleBG
    {
        get => _toggleBG;
        set
        {
            if (_toggleBG != value)
            {
                _toggleBG = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _isRight;
    [Binding]
    public bool IsRight
    {
        get => _isRight;
        set
        {
            if (_isRight != value)
            {
                _isRight = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _clickable;
    [Binding]
    public bool Clickable
    {
        get => _clickable;
        set
        {
            if (_clickable != value)
            {
                _clickable = value;
                OnPropertyChanged();
            }
        }
    }

    [Binding]
    public int Index { get; set; }

    [Binding]
    public void OnAnswerPressed()
    {
        AnswerPressed(IsRight,Index);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
