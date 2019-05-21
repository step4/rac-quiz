using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class GamePopupViewModel : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField]
    private GameObject ParseClientGO = default;
    [SerializeField]
    private GameObject NavigationGO = default;

    private IParseClient _parseClient;
    private INavigation _navigation;

    [SerializeField]
    private PlayerConfigSO PlayerSettings = default;

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
