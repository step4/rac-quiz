using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityWeld.Binding;

[Binding]
public class StartScreenViewModel : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField]
    private GameObject ParseClientGO;
    [SerializeField]
    private GameObject UniWebViewGO;
    [SerializeField]
    private GameObject NavigationGO;
    [SerializeField]
    private ConfigSO _config;

    private IParseClient _parseClient;
    private UniWebView _uniWebView;
    private INavigation _navigation;


    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IParseClient>();
        _uniWebView = UniWebViewGO.GetComponent<UniWebView>();
        _navigation = NavigationGO.GetComponent<INavigation>();

        _uniWebView.AddSslExceptionDomain("rheinahrcampus.de");
        //UniWebViewLogger.Instance.LogLevel = UniWebViewLogger.Level.Verbose;
        UniWebView.ClearCookies();

        _uniWebView.OnMessageReceived += (webView, message) => {
            _config.SessionToken =  message.Args["token"];

            _uniWebView.Hide();
            OnLoginSuccess();
        };
    }

    private string _sesTok;
    [Binding]
    public string SesTok
    {
        get => _sesTok;
        set
        {
            if (value!=_sesTok)
            {
                _sesTok = value;
                OnPropertyChanged();
            }
        }
    }

    [Binding]
    public void Login()
    {
#if UNITY_EDITOR_WIN
        _config.SessionToken = "r:5f5988739631c8c8d4088b50069b1023";
        OnLoginSuccess();
#elif UNITY_WEBGL
        _config.SessionToken = "r:5f5988739631c8c8d4088b50069b1023";
        OnLoginSuccess();
#else
        _uniWebView.Load(_config.LoginUrl);
        _uniWebView.Show();
#endif
    }

    private void OnLoginSuccess()
    {
        _navigation.Push("StudyProgramScreen");
    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
