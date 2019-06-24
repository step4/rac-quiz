using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityWeld.Binding;
using System.Threading.Tasks;

[Binding]
public class RegisterPopupViewModel : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField]
    private GameObject ParseClientGO = default;
    [SerializeField]
    private GameObject NavigationGO = default;
    [SerializeField]
    private ConfigSO _config = default;

    private IParseClient _parseClient;
    private INavigation _navigation;

    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IParseClient>();
        _navigation = NavigationGO.GetComponent<INavigation>();
    }
    private void Update()
    {
        //var rectTrans = (RectTransform)transform;
        
        //if (TouchScreenKeyboard.visible)
        //{
        //    rectTrans.offsetMax = new Vector2 { y = TouchScreenKeyboard.area.height };
        //}
        //else
        //{
        //    rectTrans.offsetMax = new Vector2 { y = 0 };
        //}

    }

    private string _email;
    [Binding]
    public string Email
    {
        get => _email;
        set
        {
            if (value != _email)
            {
                _email = value;
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

    private string _passwordConfirm;
    [Binding]
    public string PasswordConfirm
    {
        get => _passwordConfirm;
        set
        {
            if (value != _passwordConfirm)
            {
                _passwordConfirm = value;
                OnPropertyChanged();
            }
        }
    }

    [Binding]
    public async void Register()
    {
        if (Password != PasswordConfirm)
        {
            _navigation.PushModal("Passwort stimmt nicht überein!","Zurück");
            return;
        }
        try
        {
            var user = await _parseClient.Register(Username, Password,Email);
            _config.SessionToken = user.sessionToken;
            _parseClient.SetSessionToken(user.sessionToken);
            _navigation.Pop(ScreenAnimation.Close);
        }
        catch (Exception)
        {
            _navigation.PushModal("Fehler beim Registrieren!", "Zurück", ModalIcon.Error);
        }
    }

    [Binding]
    public async void ClosePopup()
    {
        _navigation.Pop(ScreenAnimation.Fade);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
