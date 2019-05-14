using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityWeld.Binding;

[Binding]
public class StudyProgramScreenViewModel : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField]
    private GameObject ParseClientGO;
    [SerializeField]
    private GameObject NavigationGO;

    private IParseClient _parseClient;
    private INavigation _navigation;


    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IParseClient>();
        _navigation = NavigationGO.GetComponent<INavigation>();

    }

    //private string _username;
    //[Binding]
    //public string Username
    //{
    //    get => _username;
    //    set
    //    {
    //        if (value != _username)
    //        {
    //            _username = value;
    //            OnPropertyChanged();
    //        }
    //    }
    //}

    [Binding]
    public async void SetStudyProgram()
    {
        _navigation.SetRoot("UserScreen");
    }
    

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
