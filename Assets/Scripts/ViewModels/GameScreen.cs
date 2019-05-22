using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityWeld.Binding;
using System.Threading.Tasks;

[Binding]
public class GameScreen : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField]
    private GameObject ParseClientGO = default;
    [SerializeField]
    private GameObject NavigationGO = default;

    private IParseClient _parseClient;
    private INavigation _navigation;
    private IAvatarClient _avatarClient;

    [SerializeField]
    private PlayerConfigSO _playerConfig = default;
    [SerializeField]
    private ConfigSO _config = default;
    [SerializeField]
    private GameSO _currentGame = default;

    [SerializeField]
    private Sprite _rightAnswerSprite = default;
    [SerializeField]
    private Sprite _wrongAnswerSprite = default;


    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IParseClient>();
        _navigation = NavigationGO.GetComponent<INavigation>();
    }

    private void OnEnable()
    {
        _currentGame.currentQuestion = 0;
        _currentGame.score = 0;
        var secondsToPlay = _config.SecondsPerDifficulty[_currentGame.game.difficulty - 1];
        _currentGame.timePerQuestion = new TimeSpan(0, 0, secondsToPlay);

        updateGameInfo();
        nextQuestion();
    }
    private void Update()
    {
        updateGameInfo();
    }

    private void nextQuestion()
    {
        if (_currentGame.currentQuestion==_currentGame.game.questions.Count)
        {
            finishGame();
            return;
        }
        _currentGame.currentQuestion++;


        fillQuestionAndAnswers();
        startQuestion();
    }

    private void finishGame()
    {
        _navigation.SetRoot("UserScreen");
    }

    private void startQuestion()
    {
        print("start");
    }

    private void fillQuestionAndAnswers()
    {
        var currentQuestion = _currentGame.game.questions[_currentGame.currentQuestion-1];
        QuestionText = currentQuestion.questionText;

        Answers = new ObservableList<AnswerViewModel>();
        currentQuestion.answers.ForEach(answer => {
            if (answer.hasLatex)
            {

            }
            var answerViewModel = new AnswerViewModel
            {
                AnswerText = answer.answerText,
                ToggleBG = answer.isRightAnswer ? _rightAnswerSprite : _wrongAnswerSprite,
                AnswerPressed = (isRight) => answerPressed(isRight)
            };
            Answers.Add(answerViewModel);
        });
    }

    private async void answerPressed(bool isRight)
    {
        if (isRight)
        {
            _currentGame.score += 10;
            await Task.Delay(1000);
            nextQuestion();
        }
        else
        {
            await Task.Delay(1000);
            nextQuestion();
        }
    }

    void updateGameInfo()
    {
        QuestionNumber = $"{_currentGame.currentQuestion}/{_currentGame.game.questions.Count}";
        GameTime = $"{_currentGame.timePerQuestion.ToString(@"mm\:ss")}";
        Score = $"{_currentGame.score}";
    }

    private string _questionText;
    [Binding]
    public string QuestionText
    {
        get => _questionText;
        set
        {
            if (value != _questionText)
            {
                _questionText = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableList<AnswerViewModel> _answers;
    [Binding]
    public ObservableList<AnswerViewModel> Answers
    {
        get => _answers;
        set
        {
            if (value != _answers)
            {
                _answers = value;
                OnPropertyChanged();
            }
        }
    }

    [SerializeField]
    private Vector2 _answerSize;
    [Binding]
    public Vector2 AnswerSize
    {
        get => _answerSize;
        set
        {
            if (value != _answerSize)
            {
                _answerSize = value;
                OnPropertyChanged();
            }
        }
    }

    [SerializeField]
    private Vector2 _answerSpacing;
    [Binding]
    public Vector2 AnswerSpacing
    {
        get => _answerSpacing;
        set
        {
            if (value != _answerSpacing)
            {
                _answerSpacing = value;
                OnPropertyChanged();
            }
        }
    }

    private string _questionNumber;
    [Binding]
    public string QuestionNumber
    {
        get => _questionNumber;
        set
        {
            if (value != _questionNumber)
            {
                _questionNumber = value;
                OnPropertyChanged();
            }
        }
    }

    private string _gameTime;
    [Binding]
    public string GameTime
    {
        get => _gameTime;
        set
        {
            if (value != _gameTime)
            {
                _gameTime = value;
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


    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
