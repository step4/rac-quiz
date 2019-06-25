using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
using System.Linq;
using System.Threading.Tasks;
using System;

[Binding]
public class SettingsScreenViewModel : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField]
    private GameObject ParseClientGO = default;
    [SerializeField]
    private GameObject NavigationGO = default;

    private IParseClient _parseClient;
    private INavigation _navigation;

    [SerializeField]
    private PlayerConfigSO _playerConfig = default;
    [SerializeField]
    private ConfigSO _config = default;


    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IParseClient>();
        _navigation = NavigationGO.GetComponent<INavigation>();

    }



    [Binding]
    public void GoBack()
    {
        _navigation.Pop(ScreenAnimation.Close);
    }

    [Binding]
    public void OpenStudyPrograms()
    {
        _navigation.Push("StudyProgramScreen", ScreenAnimation.Fade);
    }

    [Binding]
    public void Logout()
    {
        _config.Logout();
        _playerConfig.Reset();
        _navigation.Push("StartScreen", ScreenAnimation.Fade);
    }

    [Binding]
    public async void Save()
    {
        try
        {

        }
        catch (System.Exception ex)
        {
            _navigation.PushModal("Fehler beim Speichern des Avatars!", "Ok", ModalIcon.Error);
        }

    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
