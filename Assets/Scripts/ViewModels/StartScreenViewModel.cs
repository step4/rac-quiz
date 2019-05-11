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

    private IWebClient _parseClient;
    private UniWebView _uniWebView;

    public string text = "<Type some text>";
    [Binding]
    public String MyProperty
    {
        get
        {
            return text;
        }
        set
        {
            if (text == value)
            {
                return; // No change.
            }

            text = value;

            OnPropertyChanged();
        }
    }

    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IWebClient>();
        _uniWebView = UniWebViewGO.GetComponent<UniWebView>();
    }

    [Binding]
    public void Send()
    {
        MyProperty = UnityEngine.Random.Range(0, 100).ToString();
        _parseClient.Send();
    }

    [Binding]
    public void toggleWebView() {
        _uniWebView.Hide();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
