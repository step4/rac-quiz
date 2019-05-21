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
    private GameObject ParseClientGO = default;
    [SerializeField]
    private GameObject NavigationGO = default;

    private IParseClient _parseClient;
    private INavigation _navigation;

    [SerializeField]
    private PlayerConfigSO _playerConfig = default;

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

    [Binding]
    public async void SetStudyProgram(string id, string name,string shortName, Sprite sprite)
    {
        _playerConfig.StudyProgramShort = shortName;
        _playerConfig.StudyProgram = name;
        _playerConfig.StudyProgramId = id;
        _playerConfig.StudyProgramSprite = sprite;
        await _parseClient.SetStudyProgram(id);
        _navigation.SetRoot("UserScreen");
    }
    

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
