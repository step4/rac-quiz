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

    private IWebClient _parseClient;
    private UniWebView _uniWebView;
    private INavigation _navigation;


    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IWebClient>();
        _uniWebView = UniWebViewGO.GetComponent<UniWebView>();
        _navigation = NavigationGO.GetComponent<INavigation>();
        _uniWebView.AddSslExceptionDomain("rheinahrcampus.de");
        UniWebViewLogger.Instance.LogLevel = UniWebViewLogger.Level.Verbose;
        UniWebView.ClearCookies();

        _uniWebView.OnMessageReceived += (webView, message) => {
            SesTok = message.Args["token"];
            print(message.RawMessage);
            _uniWebView.Hide();
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
        //_navigation.Push("StudyProgramScreen");
        _uniWebView.Load(SesTok);
        _uniWebView.Show();
    }



    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
