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

    [SerializeField]
    private PlayerSettingsSO PlayerSettings;

    private bool popupCreated=false;

    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IParseClient>();
        _navigation = NavigationGO.GetComponent<INavigation>();
        
    }

    void OnEnable() {
        if (!popupCreated)
        {
            populateScrollView();
            popupCreated = true;
        }
    }

    async void populateScrollView()
    {
        var faculties = await _parseClient.GetStudyPrograms();
        var popup = GetComponentInChildren<StudyProgramPopup>();
        popup.Create(faculties);
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
    public void SetStudyProgram(string id, string name, Sprite sprite)
    {
        PlayerSettings.StudyProgram = name;
        PlayerSettings.StudyProgramId = id;
        PlayerSettings.StudyProgramSprite = sprite;
        _navigation.SetRoot("UserScreen");
    }
    

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
