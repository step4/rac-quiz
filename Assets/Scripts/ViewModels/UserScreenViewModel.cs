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
    private GameObject ParseClientGO;
    [SerializeField]
    private GameObject AvatarClientGO;
    [SerializeField]
    private GameObject NavigationGO;

    private IParseClient _parseClient;
    private INavigation _navigation;
    private IAvatarClient _avatarClient;


    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IParseClient>();
        _navigation = NavigationGO.GetComponent<INavigation>();
        _avatarClient = AvatarClientGO.GetComponent<IAvatarClient>();

        Username = "Chris";
        SetAvatar();
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

    [Binding]
    public async void SetAvatar()
    {
        var imgData = await _avatarClient.GetAvatar(1000);
        var width = 1000;
        var height = imgData.Length / width;
        Texture2D tex = new Texture2D(width, height);
        ImageConversion.LoadImage(tex, imgData);
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        AvatarSprite = sprite;
    }

    [Binding]
    public void OpenSettings()
    {
        _navigation.Push("SettingsScreen");
    }
    

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
