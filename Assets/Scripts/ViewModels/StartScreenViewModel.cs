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

        UniWebView.ClearCookies();

        _uniWebView.OnMessageReceived += (webView, message) => {
            print(message.RawMessage);
            _uniWebView.Hide();
        };
    }

    [Binding]
    public void Login()
    {
        _navigation.Push("UserScreen");
        //_uniWebView.Load("https://studygraph.step4.de/auth/login");
        //_uniWebView.Show();
    }



    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
