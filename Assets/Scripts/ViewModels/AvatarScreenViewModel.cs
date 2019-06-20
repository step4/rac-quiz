using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
using System.Linq;
using System.Threading.Tasks;
using System;

[Binding]
public class AvatarScreenViewModel : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField]
    private GameObject ParseClientGO = default;
    [SerializeField]
    private GameObject AvatarClientGO = default;
    [SerializeField]
    private GameObject NavigationGO = default;

    private IParseClient _parseClient;
    private INavigation _navigation;
    private IAvatarClient _avatarClient;

    [SerializeField]
    private PlayerConfigSO _playerConfig = default;
    [SerializeField]
    private ConfigSO _config = default;


    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IParseClient>();
        _navigation = NavigationGO.GetComponent<INavigation>();
        _avatarClient = AvatarClientGO.GetComponent<IAvatarClient>();

    }

    private void OnEnable()
    {
            populateListView();
    }

    private void OnDisable()
    {
        Picker.Clear();
    }

    private ObservableList<AvatarPickerViewModel> _picker = new ObservableList<AvatarPickerViewModel>();
    [Binding]
    public ObservableList<AvatarPickerViewModel> Picker
    {
        get => _picker;
        set
        {
            if (value != _picker)
            {
                _picker = value;
                OnPropertyChanged();
            }
        }
    }

    private Sprite _avatarSprite;
    [Binding]
    public Sprite AvatarSprite
    {
        get => _avatarSprite;
        set
        {
            if (value != _avatarSprite)
            {
                _avatarSprite = value;
                OnPropertyChanged();
            }
        }
    }


    [Binding]
    public void GoBack()
    {
        _navigation.Pop(ScreenAnimation.Close);
    }

    [Binding]
    public async void Save()
    {
        try
        {
           
        }
        catch (System.Exception ex)
        {
            _navigation.PushModal("Fehler beim Speichern des Avatars!", "Ok", ModalIcon.Error);
        }

    }

    private Dictionary<string, string> picks = new Dictionary<string, string>();

    private async void populateListView()
    {
        try
        {
            _navigation.PushPopup("LoadingPopup", ScreenAnimation.Fade);
            await Task.Delay(500);
            var avatarEnums = _avatarClient.GetEnums();
            Picker = new ObservableList<AvatarPickerViewModel>();

            foreach(var item in avatarEnums) {
                picks.Add(item.Key, item.Value[0]);
                var avaterPickerVM = new AvatarPickerViewModel { 
                Selectables=item.Value,
                TypeText=item.Key,
                SelectedText=item.Value[0],
                PickerPressed=configChanged
                };
                Picker.Add(avaterPickerVM);
            }
            SetAvatar();
            _navigation.PopPopup(ScreenAnimation.Close);
        }
        catch (System.Exception ex)
        {
        }

    }

    private void configChanged(string type, string selectedValue)
    {
        picks[type] = selectedValue;
        SetAvatar();
    }

    public async void SetAvatar()
    {
        var width = 200;
        var (imgData, url) = await _avatarClient.GetAvatar(width,picks);
        var height = imgData.Length / width;
        Texture2D tex = new Texture2D(width, height);
        ImageConversion.LoadImage(tex, imgData);
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        AvatarSprite = sprite;

        //PlayerConfig.Avatar = sprite;
        //PlayerConfig.AvatarUrl = url;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
