using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
using System.Linq;

[Binding]
public class GameFinishedPopupViewModel : MonoBehaviour, INotifyPropertyChanged
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
    [SerializeField]
    private GameSO _finishedGame = default;

    private bool popupCreated = false;
    
    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IParseClient>();
        _navigation = NavigationGO.GetComponent<INavigation>();
    }

    private void OnEnable()
    {
        if (!popupCreated)
        {
            popupCreated = true;
        }
    }

    private void OnDisable()
    {
        popupCreated = false;
    }



    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
