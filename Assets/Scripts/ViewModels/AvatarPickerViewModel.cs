
using UnityWeld.Binding;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

[Binding]
public class AvatarPickerViewModel : INotifyPropertyChanged
{


    private string _typeText;
    [Binding]
    public string TypeText
    {
        get => _typeText;
        set
        {
            if (value != _typeText)
            {
                _typeText = value;
                OnPropertyChanged();
            }
        }
    }

    private string _selectedText;
    [Binding]
    public string SelectedText
    {
        get => _selectedText;
        set
        {
            if (value != _selectedText)
            {
                _selectedText = value;
                OnPropertyChanged();
            }
        }
    }

    public Action<string, string> PickerPressed
    {
        get;set;
    }

    private int selectedIndex = 0;
    public List<string> Selectables { get; set; }

   

    [Binding]
    public void OnLeftClick()
    {
        selectedIndex = selectedIndex == 0 ? Selectables.Count - 1 : selectedIndex - 1;
        SelectedText = Selectables[selectedIndex];

        PickerPressed(TypeText, SelectedText);
    }

    [Binding]
    public void OnRightClick()
    {
        selectedIndex = (selectedIndex + 1) % Selectables.Count;
        SelectedText = Selectables[selectedIndex];

        PickerPressed(TypeText, SelectedText);
    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
