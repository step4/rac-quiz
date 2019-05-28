using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
using System.Linq;
using System;

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
            LoadPopup();
            popupCreated = true;
            UploadGameData();
        }
    }

    private async void UploadGameData()
    {
        var gameId = _finishedGame.game.gameId;
        var givenAnswers = _finishedGame.game.givenAnswers;
        await _parseClient.FinishGame(gameId, givenAnswers);
    }

    private void OnDisable()
    {
        popupCreated = false;
    }

    private void LoadPopup()
    {
        AvatarSprite = _playerConfig.Avatar;
        Score = _finishedGame.rightAnswerCount.ToString();
        Count = $"x {_finishedGame.game.questions.Count.ToString()}";
        Difficulty = $"x {_config.SecondsPerDifficulty[_finishedGame.game.difficulty - 1].ToString()}";
        OnTime = _finishedGame.game.withTimer ? "x 2" : "x 1";
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

    private string _score;
    [Binding]
    public string Score
    {
        get => _score;
        set
        {
            if (value != _score)
            {
                _score = value;
                OnPropertyChanged();
            }
        }
    }

    private string _count;
    [Binding]
    public string Count
    {
        get => _count;
        set
        {
            if (value != _count)
            {
                _count = value;
                OnPropertyChanged();
            }
        }
    }

    private string _difficulty;
    [Binding]
    public string Difficulty
    {
        get => _difficulty;
        set
        {
            if (value != _difficulty)
            {
                _difficulty = value;
                OnPropertyChanged();
            }
        }
    }

    private string _onTime;
    [Binding]
    public string OnTime
    {
        get => _onTime;
        set
        {
            if (value != _onTime)
            {
                _onTime = value;
                OnPropertyChanged();
            }
        }
    }

    [Binding]
    public void BackToUserScreen()
    {
        _navigation.SetRoot("UserScreen", ScreenAnimation.Fade);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
