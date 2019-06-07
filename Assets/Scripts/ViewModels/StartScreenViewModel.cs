﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityWeld.Binding;
using System.Threading.Tasks;

[Binding]
public class StartScreenViewModel : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField]
    private GameObject ParseClientGO = default;
    [SerializeField]
    private GameObject UniWebViewGO = default;
    [SerializeField]
    private GameObject NavigationGO = default;
    [SerializeField]
    private ConfigSO _config = default;
    [SerializeField]
    private PlayerConfigSO _playerConfig = default;

    private IParseClient _parseClient;
    private UniWebView _uniWebView;
    private INavigation _navigation;

    private bool loginTried = false;
    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IParseClient>();
        _uniWebView = UniWebViewGO.GetComponent<UniWebView>();
        _navigation = NavigationGO.GetComponent<INavigation>();


        _playerConfig.Reset();
        UniWebViewLogger.Instance.LogLevel = UniWebViewLogger.Level.Verbose;
        UniWebView.ClearCookies();

        _parseClient.SetSessionToken(_config.SessionToken);
        _uniWebView.OnMessageReceived += (webView, message) =>
        {

            _config.SessionToken = message.Args["token"];
            _uniWebView.Hide();
            _parseClient.SetSessionToken(_config.SessionToken);
            CheckSession();
        };

        
    }

    private void OnEnable()
    {
        if (_config.SessionToken != string.Empty)
        {
            CheckSession();
        }
    }

    private void Update()
    {
        var rectTrans = (RectTransform)transform;
        if (TouchScreenKeyboard.visible)
        {
            rectTrans.offsetMax = new Vector2 { y = TouchScreenKeyboard.area.height };
        }
        else
        {
            rectTrans.offsetMax = new Vector2 { y = 0 };
        }

    }

    private string _sesTok;
    [Binding]
    public string SesTok
    {
        get => _sesTok;
        set
        {
            if (value != _sesTok)
            {
                _sesTok = value;
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

    private string _password;
    [Binding]
    public string Password
    {
        get => _password;
        set
        {
            if (value != _password)
            {
                _password = value;
                OnPropertyChanged();
            }
        }
    }

    [Binding]
    public async void Login()
    {
        //#if UNITY_EDITOR_WIN
        //        CheckSession();
        //#elif UNITY_WEBGL
        //        CheckSession();
        //#else
        //        CheckSession();
        //#endif
        
        try
        {
            var user = await _parseClient.Login(Username, Password);
            _config.SessionToken = user.sessionToken;
            _parseClient.SetSessionToken(user.sessionToken);
            CheckSession();
        }
        catch (Exception)
        {
            _navigation.PushModal("Fehler beim Einloggen!", "Zurück", ModalIcon.Error);
        }
    }

    [Binding]
    public void Register()
    {
        _navigation.Push("RegisterPopup", ScreenAnimation.ModalCenter);
    }

    private void OpenWebViewLogin()
    {
        //_uniWebView.AddSslExceptionDomain("rheinahrcampus.de");
        _uniWebView.Load(_config.LoginUrl);
        _uniWebView.Show();
    }

    private async void CheckSession()
    {
        try
        {
            var isSuccess = await FetchPlayer();
            if (isSuccess)
                _navigation.SetRoot("UserScreen", ScreenAnimation.Fade);
            else
                _navigation.SetRoot("StudyProgramScreen", ScreenAnimation.Fade);
        }
        catch (Exception ex)
        {
            _config.SessionToken = string.Empty;
            _navigation.PushModal("Fehler: Session abgelaufen.\nBitte neu einloggen!", "Zurück", ModalIcon.Error);
            //if (loginTried)
            //{
            //    _navigation.PushModal("Fehler: Session abgelaufen.\nBitte neu einloggen!", "Zurück", ModalIcon.Error);
            //    loginTried = false;
            //}
            //else
            //{
            //    Debug.LogError(ex);
            //    Debug.LogError("Open WebLogin");
            //    loginTried = true;
            //    OpenWebViewLogin();
            //}
        }


    }

    public async Task<bool> FetchPlayer()
    {
        var playerSettings = await _parseClient.GetUserMe();
        _playerConfig.StudyProgramId = playerSettings.studyProgramId;
        _playerConfig.AvatarUrl = playerSettings.avatarUrl;
        _playerConfig.PlayerName = playerSettings.playerName;

        if (_playerConfig.StudyProgramId != "")
        {
            var studyProgram = await _parseClient.GetStudyProgram(_playerConfig.StudyProgramId);
            _playerConfig.StudyProgram = studyProgram.name;
            _playerConfig.StudyProgramShort = studyProgram.shortName;

            _playerConfig.StudyProgramSprite = convertImageBase64ToSprite(studyProgram.iconB64);
            return true;
        }
        return false;
    }

    private Sprite convertImageBase64ToSprite(string base64)
    {
        base64 = base64.Substring(base64.IndexOf(',') + 1);
        var base64EncodedBytes = Convert.FromBase64String(base64);

        Texture2D tex = new Texture2D(500, 530);
        ImageConversion.LoadImage(tex, base64EncodedBytes);
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
