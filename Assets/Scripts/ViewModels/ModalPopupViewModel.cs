using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
using System.Linq;
using System.Threading.Tasks;

[Binding]
public class ModalPopupViewModel : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField]
    private GameObject NavigationGO = default;

    private INavigation _navigation;

    [SerializeField]
    private ModalSettingsSO _modalSettings = default;



    private void Awake()
    {

        _navigation = NavigationGO.GetComponent<INavigation>();
    }

    private void OnEnable()
    {
        Icon = _modalSettings.ModalIcon;
        Message = _modalSettings.Message;
        ButtonText = _modalSettings.ButtonText;
    }

    private void OnDisable()
    {

    }

    private string _message;
    [Binding]
    public string Message
    {
        get => _message;
        set
        {
            if (value != _message)
            {
                _message = value;
                OnPropertyChanged();
            }
        }
    }

    private Sprite _icon;
    [Binding]
    public Sprite Icon
    {
        get => _icon;
        set
        {
            if (value != _icon)
            {
                _icon = value;
                OnPropertyChanged();
            }
        }
    }

    private string _buttonText;
    [Binding]
    public string ButtonText
    {
        get => _buttonText;
        set
        {
            if (value != _buttonText)
            {
                _buttonText = value;
                OnPropertyChanged();
            }
        }
    }


    [Binding]
    public void CloseModal()
    {
        _navigation.PopPopup(ScreenAnimation.Close);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
