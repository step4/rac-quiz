using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityWeld.Binding;

[Binding]
public class UserScreenViewModel : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField]
    private GameObject ParseClientGO = default;
    [SerializeField]
    private GameObject AvatarClientGO = default;
    [SerializeField]
    private GameObject NavigationGO = default;

    private IParseClient _parseClient;
    private INavigation _navigation;
    private IAvatarClient _avatarClient;

    [SerializeField]
    private PlayerConfigSO PlayerConfig = default;

    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IParseClient>();
        _navigation = NavigationGO.GetComponent<INavigation>();
        _avatarClient = AvatarClientGO.GetComponent<IAvatarClient>();
        if (PlayerConfig.Avatar==null)
        {
            SetAvatar();
        }
        
    }

    private void OnEnable()
    {
        _loadPlayerSettings();
    }

    private void _loadPlayerSettings()
    {
        AvatarSprite = PlayerConfig.Avatar;
        StudyProgramShort = PlayerConfig.StudyProgramShort;
        StudyProgramSprite = PlayerConfig.StudyProgramSprite;
    }

    private Sprite _avatarSprite;
    [Binding]
    public Sprite AvatarSprite
    {
        get => _avatarSprite;
        set
        {
            if (value!=_avatarSprite)
            {
                _avatarSprite = value;
                OnPropertyChanged();
            }
        }
    }

    private string _username;
    [Binding]
    public string Username
    {
        get => _username;
        set
        {
            if (value != _username)
            {
                _username = value;
                OnPropertyChanged();
            }
        }
    }

    private string _studyProgramShort;
    [Binding]
    public string StudyProgramShort
    {
        get => _studyProgramShort;
        set
        {
            if (value != _studyProgramShort)
            {
                _studyProgramShort = value;
                OnPropertyChanged();
            }
        }
    }

    private Sprite _studyProgramSprite;
    [Binding]
    public Sprite StudyProgramSprite
    {
        get => _studyProgramSprite;
        set
        {
            if (value != _studyProgramSprite)
            {
                _studyProgramSprite = value;
                OnPropertyChanged();
            }
        }
    }

    [Binding]
    public async void SetAvatar()
    {
        var width = 1000;
        var (imgData,url) = await _avatarClient.GetRandomAvatar(width);
        var height = imgData.Length / width;
        Texture2D tex = new Texture2D(width, height);
        ImageConversion.LoadImage(tex, imgData);
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        AvatarSprite = sprite;

        PlayerConfig.Avatar = sprite;
        PlayerConfig.AvatarUrl = url;
    }

    [Binding]
    public void OpenSettings()
    {
        _navigation.Push("StudyProgramScreen", ScreenAnimation.Fade);
    }

    [Binding]
    public void OpenGamePopup()
    {
        _navigation.PushPopup("GamePopup", ScreenAnimation.ModalCenter);
    }

    [Binding]
    public void OpenModal()
    {
        _navigation.PushModal("Fehler2", "yes", ModalIcon.Error);
    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
